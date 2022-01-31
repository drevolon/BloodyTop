using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IMove, IHeal
{
    public int HP { get; set; } = 10;
    public float Speed { get; set; } = 6f;

    protected Rigidbody _rigidbody;
    protected Transform _transform;
    Touch touch;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        
    }
    public void Move()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveVertical, 0f, -moveHorizontal);

        _rigidbody.AddForce(movement * Speed, ForceMode.Impulse);

        _transform.RotateAround(new Vector3(0f, 1f, 0f), Time.deltaTime * 10);

        #if UNITY_ANDROID
                //touch = Input.GetTouch(0);
                //    if (touch.tapCount>0)
                //    Debug.Log("Touch");
            
        #endif
    }

    public void MoveClickMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }
}


