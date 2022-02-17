using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : BaseController
{
    private MainMenuController _mainMenuController;
    private GameController _gameController;
    private readonly Transform _placeForUi;

    public MainController()
    {
    }

    public MainController(MainMenuController mainMenuController)
    {
        _mainMenuController = mainMenuController;
    }

    private void OnChangeGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Start:
                _mainMenuController = new MainMenuController(_placeForUi);
                _gameController?.Dispose();
                break;
            case GameState.Game:
               // _gameController = new GameController(_placeForUi);
                _mainMenuController?.Dispose();
                break;

           
            default:
                _mainMenuController?.Dispose();
                _gameController?.Dispose();
                break;
        }
    }

    protected override void OnDispose()
    {
        _mainMenuController?.Dispose();
        _gameController?.Dispose();
        
        base.OnDispose();
    }
}
