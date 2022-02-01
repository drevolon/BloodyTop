using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : InteractiveObject, IMove
{
    public float Speed { get; set; } = 2f;
    Transform transform;

    //public Car(float speed, Transform transform)
    //{
    //    //Speed = speed;
    //    //this.transform = transform;
    //}

    public void Move()
    {
        transform.Translate(new Vector3(-Speed*Time.deltaTime, 0f, 0f));
    }

    void Start()
    {
        transform = GetComponent<Transform>();
    }

    void Update()
    {

       
    }
    public void DestroyCar()
    {
        Destroy(gameObject);
    }

    public override void Interaction()
    {
        
    }
}
