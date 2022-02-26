using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuController : BaseController
{
    
    private readonly ProfilePlayer _profilePlayer;
    private readonly MainMenuView _view;

    private readonly ResourcePath _viewPath = new ResourcePath { PathResource = "Prefabs/mainMenu" };


    public MainMenuController(Transform placeForUi, ProfilePlayer profilePlayer)
    {
        _profilePlayer = profilePlayer;
        _view = LoadView(placeForUi);
        
        //_view.Start += StartGame;
        //_view.Exit += ExitGame;

        _view.Init(StartGame, ExitGame);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
     
        _profilePlayer.CurrentState.Value = GameState.Game;
    }

    private MainMenuView LoadView(Transform placeForUi)
    {
        var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), placeForUi, false);
        AddGameObjects(objectView);

        return objectView.GetComponent<MainMenuView>();
    }


}
