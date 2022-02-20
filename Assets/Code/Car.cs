using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : InteractiveObject, IMove
{
    public float Speed { get; set; } = 3f;
    private Transform _transformSpawnPos;
    private Transform transformCar;

    public Car(float speed)
    {
        Speed = speed;
    }

    private void Awake()
    {
        transformCar = GetComponent<Transform>();
    }

    public void Move()
    {
        transformCar.Translate(new Vector3(-Speed*Time.deltaTime, 0f, 0f));
    }

    public void DestroyCar()
    {
        Destroy(gameObject);
    }

    public override void Interaction()
    {
        
    }
}
