using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagDollAnim : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private Vector3 _force;
   

    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;

    void Start()
    {
        _rigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();
        _colliders = gameObject.GetComponentsInChildren<Collider>();
        Revive();
    }

    private void Revive()
    {
        SetState(true);
        _animator.enabled = true;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    Death();
        //}
        

    }

    public void Death()
    {
        _animator.enabled = false;
        SetState(false);
        _rigidbodies.First().AddForce(_force, ForceMode.Impulse);
    }

    private void SetState(bool isActive)
    {
        foreach (var body in _rigidbodies)
        {
            body.isKinematic = isActive;
        }

        foreach (var collider in _colliders)
        {
            collider.enabled = !isActive;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit");
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStayEthon");
    }

    
}
