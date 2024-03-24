using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using UI.Controllers;

/// <summary>
/// Views are what is displaied to the user
/// Views are set to enabled or inactive by the UIController
/// Becasue of this we can use OnEnable and OnDisable to setup any data, return any objects to the pool, unsubscribe from events, ect.
/// </summary>
public abstract class View : MonoBehaviour, IView
{
    [SerializeField]
    AllViews m_viewID;
    public AllViews ViewID { get => m_viewID; set => m_viewID = value; }

    [SerializeField]
    bool m_isPopup;
    public bool isPopup { get => m_isPopup; set => m_isPopup = value; }


    public UIController controller;
    public virtual void Awake()
    {
        controller = UIController.Instance;
    }

    public GameObject GetViewGameObject()
    {
        return gameObject;
    }

    public void TurnOffView()
    {
        gameObject.SetActive(false);
    }

    public void TurnOnView()
    {
        gameObject.SetActive(true);
    }
}
