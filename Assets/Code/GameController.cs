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

        

        GenerationObj();

        _car = FindObjectOfType<Car>();

        foreach (var dollItem in FindObjectsOfType<RagDollAnim>())
        {
            _dollAnim.Add(dollItem);
        }

        //car = new Car(GameObject obj);
        //beginStart = gameObjectCar.GetComponent<Transform>().position;

       // _car = new Car(2f, spawnPoint);

        

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
                if (dObject.GetComponent<Rigidbody>().velocity.y < -1)
                {
                    //Debug.Log($"Падает объект {dObject.name} V {dObject.GetComponent<Rigidbody>().velocity.y}");

                    Destroy(dObject.gameObject, 2f);


                    foreach (var dollItem in _dollAnim)
                    {
                        if (dollItem is RagDollAnim dollObject)
                        {
                            dollObject.Death();
                        }

                    }
                    foreach (var dollItem in _dollAnim)
                    {
                        if (dollItem is RagDollAnim dollObject)
                        {
                            dollObject.DestroyObject();
                        }
                    }
                    _dollAnim.Clear();

                }
            }


            if (interactiveObject is BoosterObject bObject)
            {
                bObject.Flay();
            }

            

        }


        if (_car != null)
        {
            if (_car.transform.position.x < -16f)
            {
                _car.DestroyCar();
                GenerationObj();
            }
            else
            {
                Debug.Log("_car.Move()");
                _car.Move();
            }
        }
        else
        {
           // GenerationObj();
        }

    }

     

    void GenerationObj()
    {
        _car = FindObjectOfType<Car>();
        Instantiate(gameObjectCar, spawnPoint.position, Quaternion.identity);
    }
}
