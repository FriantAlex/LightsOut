using System;
using System.Collections;
using System.Collections.Generic;
using UI.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LightButton : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    Color onColor;
    [SerializeField]
    Color offColor;
    
    Button button;
    bool _isOn = false;
    public bool IsOn { get => _isOn; }

    protected void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnToggleClicked);
        SetState(_isOn);
    }

    void OnToggleClicked()
    {
        ToggleState();
        UIControllerEvents.InvokeOnLightClicked(this, _isOn);
        UIControllerEvents.InvokeNumberOfMovesUpdated();
        button.targetGraphic.color = _isOn ? onColor : offColor;
    }

    public void ToggleState()
    {
        SetState(!_isOn);
    }

    void SetState(bool value)
    {
        _isOn = value;
        button.targetGraphic.color = value ? onColor : offColor;
    }

}
