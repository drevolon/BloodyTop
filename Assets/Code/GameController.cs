using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameController(ProfilePlayer profilePlayer)
    {
        //var carController = new Car();
        //AddController(carController);
    }

    private void Awake()
    {

       // _player = FindObjectOfType<Player>();

        _dollAnim = new List<RagDollAnim>();

        GenerationObj();

        foreach (var dollItem in FindObjectsOfType<RagDollAnim>())
        {
            _dollAnim.Add(dollItem);
        }

        CarInit();

        _interactiveObjects = FindObjectsOfType<InteractiveObject>();
    }

   

    private void Update()
    {
<<<<<<< HEAD
        if (_player.CurrentVelocity < 0)
        {
            //Debug.Log("Player Down. Need game over");
           //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
=======
        //if (_player.CurrentVelocity < 0)
        //{
        //    //Debug.Log("Player Down. Need game over");
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
>>>>>>> main

        for (int i = 0; i < _interactiveObjects.Length; i++)
        {
            var interactiveObject = _interactiveObjects[i];

            if (interactiveObject == null) continue;


            if (interactiveObject is DestructibleObjects dObject)
            {
                if (dObject.GetComponent<Rigidbody>().velocity.y < -2)
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


        if (_car.Count>0)
        {
            foreach (var itemCar in _car)
            {
                if (itemCar.transform.position.x < -16f)
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
            GenerationObj();
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
     

    void GenerationObj()
    {
        Instantiate(gameObjectCar, spawnPoint.position, Quaternion.identity);
    }
}
