using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private InteractiveObject[] _interactiveObjects;

    private void Awake()
    {
        _interactiveObjects = FindObjectsOfType<InteractiveObject>();
    }

    private void Update()
    {
        for (int i = 0; i < _interactiveObjects.Length; i++)
        {
            var interactiveObject = _interactiveObjects[i];

            if (interactiveObject == null) continue;


            if (interactiveObject is DestructibleObjects dObject)
            {
                //Debug.Log(nameof(dObject));
                //Debug.Log(typeof(DestructibleObjects));
            }
               
            
            if (interactiveObject is BoosterObject bObject)
            {
                //Debug.Log(nameof(bObject));
                //Debug.Log(typeof(BoosterObject));
            }


        }
    }
}
