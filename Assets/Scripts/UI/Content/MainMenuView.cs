using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Views
{
    public class MainMenuView : View
    {
        [SerializeField]
        Button startButton;
        [SerializeField]
        Button exitButton;

        public override void Awake()
        {
            base.Awake();
            startButton.onClick.AddListener(StartGame);
            exitButton.onClick.AddListener(() => Application.Quit());
        }

        void OnEnable()
        {
            //Nothing to setup
        }

        void OnDisable()
        {
            // Nothing to clean up
        }

        private void StartGame()
        {
            UIControllerEvents.InvokeOnViewChange(AllViews.GameView);
            UIControllerEvents.InvokeNewGameStarted();
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }


    }
}
