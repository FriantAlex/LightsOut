using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.Events;
using UnityEngine;

namespace UI.Controllers
{
    /// <summary>
    /// Retreives data from the game model for use in the views
    /// controls what views are displaied to the user
    /// </summary>
    public class UIController : Singelton<UIController>
    {
        // Assign the starting view in editor
        [SerializeField]
        private AllViews startingView = (AllViews)(-1);
        //initilize previous view as none 
        AllViews previousView = (AllViews)(-1);
        //initilize current view as none
        AllViews activeView = (AllViews)(-1);

        // List of all views so that the controller can have a refrense to the views to 
        private List<IView> currentViews = new List<IView>();

        // The view that is currently active
        IView _currentView;
        public IView CurrentView { get => _currentView; private set => _currentView = value; }

        // the view that was previously active
        IView _previousView;
        public IView PreviousView { get => _previousView; private set => _previousView = value; }

        LightsOutModel gameModel;

        public override void Init()
        {
            base.Init();

            gameModel = LightsOutModel.Instance;
            UIControllerEvents.onViewChange += EventManager_onViewChange;
            UIControllerEvents.OnLightClicked += UIControllerEvents_OnLightClicked;

            //Search all Children for any views
            foreach (Transform t in transform)
            {
                if (t.TryGetComponent<IView>(out IView newView))
                {
                    currentViews.Add(newView);
                }

            }

            CurrentView = GetView(startingView);
            UIControllerEvents.InvokeOnViewChange(startingView);
        }



        private void OnDestroy()
        {
            UIControllerEvents.onViewChange -= EventManager_onViewChange; 
            UIControllerEvents.OnLightClicked += UIControllerEvents_OnLightClicked;
        }

        private void UIControllerEvents_OnLightClicked(LightButton light, bool value)
        {
            gameModel.UpdatedBoard(light, value);
        }

        private void EventManager_onViewChange(AllViews view)
        {
            // The view we are attempting to show is not in the scene
            if (!ListContainsInvokedPanel(startingView))
            {
                Debug.LogError(string.Format("{0} does not exist in the list", startingView.ToString()));
                return;
            }

            previousView = activeView;
            activeView = view;

            UpdatePanels(view);

            switch (view)
            {
                case AllViews.GameView:
                    gameModel.GenerateGrid();
                    break;
                case AllViews.WinView: // We don't want to clear the game state since the popup needs data from the model
                    break;
                default:
                    gameModel.EndGame();
                    break;

            }
        }

        /// <summary>
        /// Set the incoming view to active and all others to inactive unless the incoming view is a popup
        /// </summary>
        /// <param name="newView"> Incoming view to set active</param>
        private void UpdatePanels(AllViews newView)
        {
            foreach (IView view in currentViews)
            {
                if (view.ViewID == newView)
                {
                    PreviousView = CurrentView;
                    CurrentView = view;
                }
            }

            CurrentView.TurnOnView();
            //We don't want two popups at the same time
            if (PreviousView != null & PreviousView.isPopup)
            {
                PreviousView.TurnOffView();
            }

            // if the incoming view is a popup we do not want to turn off any other active views
            if (CurrentView.isPopup)
                return;

            // Set any visable views to inactive to ensure we do not have any overlaps
            foreach (IView viewObject in currentViews)
            {
                if (viewObject.ViewID != CurrentView.ViewID)
                    viewObject.TurnOffView();
            }

        }

        IView GetView(AllViews invokedPanel)
        {
            return currentViews.FirstOrDefault(view => view.ViewID == invokedPanel);
        }

        bool ListContainsInvokedPanel(AllViews invokedPanel)
        {
            return currentViews.Any(view => view.ViewID == invokedPanel);
        }

        public int GetTotalMoves()
        {
            return gameModel.TotalMoves;
        }

        public float GetTotalTime()
        {
            return gameModel.TotalTime;
        }

        public bool IsGameActive()
        {
            return gameModel.GameActive;
        }

        public Dictionary<string, LightButton> GetGameGrid()
        {
            return gameModel.MatrixMap;
        }
    }
}