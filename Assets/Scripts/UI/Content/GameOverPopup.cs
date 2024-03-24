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
    public class GameOverPopup : View
    {
        [Header("UI")]
        [SerializeField]
        TextMeshProUGUI timeText;
        [SerializeField]
        TextMeshProUGUI movesText;
        [SerializeField]
        Button returnButton;
        [SerializeField]
        Button newGameButton;

        public override void Awake()
        {
            base.Awake();
            returnButton.onClick.AddListener(() => UIControllerEvents.InvokeOnViewChange(AllViews.MainMenu));
            newGameButton.onClick.AddListener(OnNewGamePressed);
        }

        private void OnNewGamePressed()
        {
            UIControllerEvents.InvokeOnViewChange(AllViews.GameView);
            UIControllerEvents.InvokeNewGameStarted();
        }

        void OnEnable()
        {
            movesText.text = $"Moves: {controller.GetTotalMoves()}";
            UpdateTime(controller.GetTotalTime());
        }

        void OnDisable()
        {
            timeText.text = "TIME: 00:00";
            movesText.text = "MOVES: 0";
        }

        public void UpdateTime(float time)
        {
            float minutes = Mathf.FloorToInt(time / 60);
            float seconds = Mathf.FloorToInt(time % 60);

            timeText.text = string.Format("TIME: {0:00}:{1:00}", minutes, seconds);
        }

    }
}
