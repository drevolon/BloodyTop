using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : BaseController
{
    public Transform PlaceForUi { get; }
   
    private readonly MainMenuView _view;

    private readonly ResourcePath _viewPath = new ResourcePath { PathResource = "Prefabs/mainMenu" };
    public MainMenuController(Transform placeForUi)
    {
        PlaceForUi = placeForUi;
        _view = LoadView(placeForUi);
        _view.Init(StartGame, ExitGame);
    }

    private void ExitGame()
    {
        
    }

    private void StartGame()
    {
        
    }

    private MainMenuView LoadView(Transform placeForUi)
    {
        var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), placeForUi, false);
        AddGameObjects(objectView);

        return objectView.GetComponent<MainMenuView>();
    }


}
