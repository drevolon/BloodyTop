using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    private readonly ResourcePath _viewPath = new ResourcePath { PathResource = "Prefabs/top" };
    private readonly ResourcePath _viewPathobjLine = new ResourcePath { PathResource = "Prefabs/LineTarget" };
    private ProfilePlayer _profilePlayer;
    private PlayerView _playerView;
    private PlayerUIView _playerUIView;
    private MousePointerController _mouseController;
    private TouchController _touchController;
    private GameObject _playerObject;
    private GameObject LoadObject()
    {
        var objView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath));
        AddGameObjects(objView);
        return objView;
    }
    private void LoadUIController<T>() where T : BaseController
    {
        AddController((_playerObject.AddComponent<T>()));
    }

    public PlayerController(ProfilePlayer profilePlayer)
    {
        _profilePlayer = profilePlayer;
        _playerObject = LoadObject();
        _playerView = _playerObject.AddComponent<PlayerView>();
        _playerView._profilePlayer = _profilePlayer;
        _playerView.Init();

        _playerUIView = _playerObject.AddComponent<PlayerUIView>();
        _playerUIView._profilePlayer = _profilePlayer;
        _playerUIView.LineTarget = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPathobjLine));
        _playerUIView._view = _playerView;
        _playerUIView.Init();

        // Загружаем контроллеры переферии (мышь, тач и т.д.)
        LoadUIController<MousePointerController>();
        LoadUIController<TouchController>();

        //Подписываемся на изменение состояния волчка 
        _profilePlayer.CurrentPlayerState.SubscribeOnChange(OnChangePlayerState);
        // Первоначальное состояние 
        _profilePlayer.CurrentPlayerState.Value = PlayerState.NotStart;

    }
    private void OnChangePlayerState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.NotStart:
                _playerUIView.notStartTop();
                break;

            case PlayerState.Start:
                _playerUIView.StartTop();
                break;

            case PlayerState.SlowMotion:
                _playerUIView.notStartTop();
                break;


            default:
                break;
        }
    }
    protected override void OnDispose()
    {
        _profilePlayer.CurrentPlayerState.UnSubscriptionOnChange(OnChangePlayerState);
        base.OnDispose();
    }
}
