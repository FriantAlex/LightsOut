using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeping all enums here for ease of access
/// </summary>
namespace Enums
{
    public enum AllViews: int
    {
        NONE =-1, // for error checking and default values to ensure we change it
        MainMenu =0,
        GameView =1,
        WinView =2,
    }
}
