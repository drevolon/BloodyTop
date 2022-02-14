using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public Transform PlaceForUi { get; }

    private readonly ResourcePath _viewPath = new ResourcePath { PathResource = "Prefabs/mainMenu" };
    public MainMenuController(Transform placeForUi)
    {
        PlaceForUi = placeForUi;
    }

    
}
