using UnityEngine;

public class PlayerController: BaseController
{
    private readonly ResourcePath _viewPath = new ResourcePath { PathResource = "Prefabs/top" };
    private ProfilePlayer _profilePlayer;
    private PlayerView _playerView;

    private PlayerView LoadView()
    {
        var objView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath)); 
        AddGameObjects(objView);

        return objView.GetComponent<PlayerView>();
    }

    public PlayerController(ProfilePlayer profilePlayer)
    {
        _profilePlayer = profilePlayer;
        _playerView = LoadView();
    }
}
