using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerView Player;
    private Vector3 _offset;

    private void Start()
    {
       // _offset = transform.position - Player.transform.position;
    }

    private void LateUpdate()
    {
       // transform.position = Player.transform.position + _offset;
    }

}
