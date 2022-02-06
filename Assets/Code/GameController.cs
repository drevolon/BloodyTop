using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    GameObject gameObjectCar;

    private InteractiveObject[] _interactiveObjects;
  
    private Car _car;
    Vector3 beginStart;
    
    

    private void Awake()
    {
        

        //car = new Car(GameObject obj);
        //beginStart = gameObjectCar.GetComponent<Transform>().position;

        _car = new Car(2f, spawnPoint);

       // GenerationObj(gameObjectCar);
        
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
            }
               
            
            if (interactiveObject is BoosterObject bObject)
            {
            }

            if (interactiveObject is Car cObject)
            {
                //Debug.Log("MoveCar");
                cObject.Move();

                if (cObject.transform.position.x < -16f)
                {
                    cObject.DestroyCar();
                  //  GenerationObj(gameObjectCar);
                   // _interactiveObjects = FindObjectsOfType<InteractiveObject>();
                }
            }

        }
    }

    void GenerationObj(GameObject car)
    {
        Instantiate(_car, spawnPoint.position, Quaternion.identity);
    }
}
