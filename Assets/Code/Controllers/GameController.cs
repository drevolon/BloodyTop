using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

public class GameController : BaseController
{
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    GameObject gameObjectCar;
    
    //RagDollAnim dollAnim;

    private InteractiveObject[] _interactiveObjects;
    private List<RagDollAnim> _dollAnim;

    private List<Car> _car;

    Vector3 beginStart;

    private Player _player;

    Rigidbody rigidbodyInteractive;
    Vector3 vectorForceInteractive;



    public GameController(ProfilePlayer profilePlayer)
    {
        //var carController = new Car();
        //AddController(carController);

    }

    private void Awake()
    {

        // _player = FindObjectOfType<Player>();
        EventController.OnStoped += OnStopedTop; //Подписались на событие падения волчка;
        EventController.OnCollision += OnCollisionTop; //Подписались на столкновение


        _dollAnim = new List<RagDollAnim>();

        GenerationObj();

        foreach (var dollItem in FindObjectsOfType<RagDollAnim>())
        {
            _dollAnim.Add(dollItem);
        }

        CarInit();

        _interactiveObjects = FindObjectsOfType<InteractiveObject>();
    }

    private void OnCollisionTop(Vector3 arg1, Collider collisionColliderObject)
    {
        var collisionColliderObjects = collisionColliderObject.GetComponentsInChildren<DestructibleObjects>();

        

        if (collisionColliderObjects.Length > 0)
        {
            collisionColliderObject.GetComponent<Collider>().isTrigger = true;

            foreach (var item in collisionColliderObjects)
            {
                rigidbodyInteractive= item.GetComponent<Rigidbody>();
                rigidbodyInteractive.isKinematic = false;
                //vectorForceInteractive = new Vector3(arg1.x, arg1.y * Random.Range(1f, 3f), arg1.z);

                vectorForceInteractive = new Vector3( transform.position.x * Random.Range(30f, 50f), transform.position.y * Random.Range(300f, 500f), transform.position.z * Random.Range(30f, 50f));

                rigidbodyInteractive.AddForce(vectorForceInteractive);
            }
        }
        else
        {


            //rigidbodyInteractive = collisionColliderObject.GetComponent<Rigidbody>();


            //if (rigidbodyInteractive != null)
            //{
            //    // Debug.Log($"волчок столкнулся x={arg1.x} y={arg1.y} z={arg1.z} {collisionColliderObject}");
            //    rigidbodyInteractive.isKinematic = false;
            //    vectorForceInteractive = new Vector3(arg1.x, arg1.y * Random.Range(100f, 300f), arg1.z);
            //    rigidbodyInteractive.AddForce(vectorForceInteractive);
            //}
        }
        //if (collisionColliderObject!=null)
        //collisionColliderObject.GetComponent<Collider>().isTrigger = false;
    }

    private void OnStopedTop() // Волчок упал
    {
        Debug.Log("Player Down. Need game over");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
      //  if (_player.CurrentVelocity < 0)
      //  {
            //Debug.Log("Player Down. Need game over");
           //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      //  }

        for (int i = 0; i < _interactiveObjects.Length; i++)
        {
            var interactiveObject = _interactiveObjects[i];

            if (interactiveObject == null) continue;


            if (interactiveObject is DestructibleObjects dObject)
            {
                rigidbodyInteractive = dObject.GetComponent<Rigidbody>();

                if (rigidbodyInteractive.velocity.y < -5f)
                {
                    //Debug.Log($"Падает объект {dObject.name} V {dObject.GetComponent<Rigidbody>().velocity.y}");

                    //Destroy(dObject.gameObject, 3f);

                    //DestroyObject(dObject);

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
                //if (_collisionTop)
                //{
                //    rigidbodyInteractive.isKinematic = false;
                //    rigidbodyInteractive.AddForce(transform.up * Random.Range(50, 100));
                
                //}
            }


            if (interactiveObject is BoosterObject bObject)
            {
                bObject.Flay();
            }

            

        }


        if (_car.Count>0)
        {
            foreach (var itemCar in _car)
            {
                if (itemCar.transform.position.x > 70f)
                {
                    if (_car!=null)
                    itemCar?.DestroyCar();
                    _car.Remove(itemCar);
                    break;
                    //GenerationObj();
                }
                else
                {
                   //Debug.Log("_car.Move()");
                    itemCar?.Move();
                }
            }
            //_car.Clear();


        }
        else
        {
           // GenerationObj();
            CarInit();
        }

    }

    void CarInit()
    {
        _car = new List<Car>();

        foreach (var item in FindObjectsOfType<Car>())
        {
            _car.Add(item);
        }
    }

    private void DestroyObject(GameObject dObject)
    {
        if (dObject!=null)
        Destroy(dObject.gameObject, 3f);
    }


    void GenerationObj()
    {
        Instantiate(gameObjectCar, spawnPoint.position, Quaternion.AngleAxis(-90, Vector3.up));
    }
}
