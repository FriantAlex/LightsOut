using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

namespace UI.Events
{
    public class UIControllerEvents : MonoBehaviour
    {

        public static event Action<AllViews> onViewChange;
        public static void InvokeOnViewChange(AllViews view)
        {
            onViewChange?.Invoke(view);
        }

        public static event Action StartGame;

        public static event Action<LightButton,bool> OnLightClicked;
        public static void InvokeOnLightClicked(LightButton button, bool value)
        {
            OnLightClicked?.Invoke(button,value);
        }

        public static event Action numberOfMovesUpdated;
        public static void InvokeNumberOfMovesUpdated()
        {
            numberOfMovesUpdated?.Invoke();
        }

        public static event Action newGameStarted;
        public static void InvokeNewGameStarted()
        {
            newGameStarted?.Invoke();
        }
    }
}
