using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObject : MonoBehaviour
{
    [SerializeField] GameObject gameObject;
    bool _collisionStay;

    private void Start()
    {
        _collisionStay = false;
    }
    private void Update()
    {
        if (!_collisionStay)
        {
            Debug.Log("Active Death");
       }
    }

   
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("OnTriggerStay");
        _collisionStay = true;
    }
}
