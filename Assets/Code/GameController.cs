using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    GameObject gameObjectCar;
    
    //RagDollAnim dollAnim;

    private InteractiveObject[] _interactiveObjects;
    private List<RagDollAnim> _dollAnim;

    Car _car;

    Vector3 beginStart;



    private void Awake()
    {

        _dollAnim = new List<RagDollAnim>();

        //_dollAnim=FindObjectsOfType<RagDollAnim>();

        foreach (var dollItem in FindObjectsOfType<RagDollAnim>())
        {
            _dollAnim.Add(dollItem);
        }

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
                if (dObject.GetComponent<Rigidbody>().velocity.y<-1)
                {
                    //Debug.Log($"Падает объект {dObject.name} V {dObject.GetComponent<Rigidbody>().velocity.y}");

                    Destroy(dObject.gameObject,1f);


                    foreach (var dollItem in _dollAnim)
                    {
                        if (dollItem is RagDollAnim dollObject)
                        {
                            dollObject.Death();
                            
                        }
                       // _dollAnim.Remove(dollItem);
                    }

                    //for (int x = 0; x < _dollAnim.Length; x++)
                    //{
                    //    var dollAnim = _dollAnim[x];
                    //    if (dollAnim is RagDollAnim dollObject)
                    //    {
                    //        dollObject.Death();
                    //    }
                    //}


                }
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
