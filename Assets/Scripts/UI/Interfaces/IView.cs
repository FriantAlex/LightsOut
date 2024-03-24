using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public interface IView
{
    //The view id this view should be assigned to
    AllViews ViewID { get; set; }
    // Popups do not disable the background view 
    bool isPopup { get; set; }

    /// <summary>
    /// Get the Gameobject this component is attached to
    /// </summary>
    /// <returns>GameObject the View is attached to</returns>
    public GameObject GetViewGameObject();
    /// <summary>
    /// Sets the active state the GameObject this view is attached to False
    /// </summary>
    public void TurnOffView();
    /// <summary>
    /// Sets the active state the GameObject this view is attached to True
    /// </summary>
    public void TurnOnView();
}
