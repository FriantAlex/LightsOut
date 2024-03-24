using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Controllers;
using UI.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class GameView : View
    {
        [Header("UI")]
        [SerializeField]
        TextMeshProUGUI timerText;
        [SerializeField]
        TextMeshProUGUI movesText;
        [SerializeField]
        RectTransform gridContentRect;
        [SerializeField]
        Button exitButton;

        public override void Awake()
        {
            base.Awake();
            exitButton.onClick.AddListener(OnExit);
            UIControllerEvents.numberOfMovesUpdated += UpdateMoves;
            UIControllerEvents.newGameStarted += UIEvents_newGameStarted;
        }

        void OnDisable()
        {
            timerText.text = "TIME: 00:00";
            movesText.text = "MOVES: 0";
            ClearGameBoard();
        }

        private void OnExit()
        {
            UIControllerEvents.InvokeOnViewChange(AllViews.MainMenu);
        }


        private void UIEvents_newGameStarted()
        {
            ClearGameBoard();
            SetUpBaord();
            UpdateMoves();
            StartCoroutine(TrackGameState());
        }

        private IEnumerator TrackGameState()
        {
            while (controller.IsGameActive())
            {
                UpdateTime(controller.GetTotalTime());
                yield return new WaitForFixedUpdate();
            }

            UIControllerEvents.InvokeOnViewChange(AllViews.WinView);
        }


        private void SetUpBaord()
        {
            Dictionary<string, LightButton> gameMatrix = controller.GetGameGrid();

            foreach(LightButton b in gameMatrix.Values)
            {
                b.transform.SetParent(gridContentRect.transform);
            }
        }

        public void UpdateTime(float time)
        {
            float minutes = Mathf.FloorToInt(time / 60);
            float seconds = Mathf.FloorToInt(time % 60);

            timerText.text = string.Format("TIME: {0:00}:{1:00}", minutes, seconds);
        }

        public void UpdateMoves()
        {
            movesText.text = $"Moves: {controller.GetTotalMoves()}";
        }

        public void ClearGameBoard()
        {
            foreach(Transform t in gridContentRect.transform)
            {
                Destroy(t.gameObject);
            }
        }
    }
}
