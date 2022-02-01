using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private InteractiveObject[] _interactiveObjects;
    [SerializeField]
    private Car _car;
    Vector3 beginStart;

    private void Awake()
    {
        _interactiveObjects = FindObjectsOfType<InteractiveObject>();

        //car = new Car(GameObject obj);
        beginStart = _car.GetComponent<Transform>().position;

        //car = new Car(2f, beginStart);
    }

    private void Update()
    {
        for (int i = 0; i < _interactiveObjects.Length; i++)
        {
            var interactiveObject = _interactiveObjects[i];

            if (interactiveObject == null) continue;


            if (interactiveObject is DestructibleObjects dObject)
            {
            }
               
            
            if (interactiveObject is BoosterObject bObject)
            {
            }

            if (interactiveObject is Car cObject)
            {
                cObject.Move();

                if (cObject.transform.position.x < -16f)
                {
                    cObject.DestroyCar();
                    GenerationObj(cObject);
                }
            }

        }
    }

    void GenerationObj(Car car)
    {
        Instantiate(_car, beginStart, Quaternion.identity);
    }
}
