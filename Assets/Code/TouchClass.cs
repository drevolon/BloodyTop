using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchClass  
{
    public Vector2 startPos;
    public Vector2 direction;
    public bool directionChosen;

    protected Rigidbody _rigidbody;
    protected Transform _transform;
    protected float StartForce = 20f;
    private void Start()
    {

    }
    void Update()
    {
        // Track a single touch as a direction control. if (Input.touchCount > 0)
        {
            //            Touch touch = Input.GetTouch(0);

          
        }
    }
}