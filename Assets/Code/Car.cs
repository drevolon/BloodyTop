using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : InteractiveObject, IMove
{
    public float Speed { get; set; } = 3f;
    private Transform _transformSpawnPos;
    private Transform transformCar;

    //public Car(float speed, Transform transformSpawnPos)
    //{
    //    Speed = speed;
    //    _transformSpawnPos = transformSpawnPos;
    //}

    private void Awake()
    {
        transformCar = GetComponent<Transform>();
    }

    public void Move()
    {
        transformCar.Translate(new Vector3(-Speed*Time.deltaTime, 0f, 0f));
    }

    void Start()
    {
        //transform = GetComponent<Transform>();
    }

    void Update()
    {
       // Debug.Log("Move Car");
        //Move();
    }
    public void DestroyCar()
    {
        Destroy(gameObject);
    }

    public override void Interaction()
    {
        
    }
}
