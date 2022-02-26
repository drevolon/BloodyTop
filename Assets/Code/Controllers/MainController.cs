using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : BaseController
{
    private MainMenuController _mainMenuController;
    private GameController _gameController;
    private SpawnController _spawnController;

    private PlayerController _playerController;


    private readonly ProfilePlayer profilePlayer;
    
    private readonly Transform _placeForUi;

    public MainController(Transform placeForUi, ProfilePlayer profilePlayer)
    {
        _placeForUi = placeForUi;
        this.profilePlayer = profilePlayer;
        OnChangeGameState(profilePlayer.CurrentState.Value);
        profilePlayer.CurrentState.SubscribeOnChange(OnChangeGameState);
    }

    private void OnChangeGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Start:
                _mainMenuController = new MainMenuController(_placeForUi, profilePlayer);
                _gameController?.Dispose();
                _spawnController?.Dispose();
                break;

            case GameState.Game:
                _spawnController = new SpawnController(profilePlayer);
                _mainMenuController?.Dispose();

                _playerController = new PlayerController(profilePlayer);
                
                break;

           
            default:
                _mainMenuController?.Dispose();
                _gameController?.Dispose();
                _spawnController?.Dispose();
                break;
        }
    }

    protected override void OnDispose()
    {
        _mainMenuController?.Dispose();
        _gameController?.Dispose();
        _spawnController?.Dispose();
        profilePlayer.CurrentState.UnSubscriptionOnChange(OnChangeGameState);
        base.OnDispose();
    }
}
