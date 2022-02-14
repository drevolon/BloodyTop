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

    public RagDollAnim(Animator animator, Vector3 force, Rigidbody[] rigidbodies, Collider[] colliders)
    {
        _animator = animator;
        _force = force;
        _rigidbodies = rigidbodies;
        _colliders = colliders;

        
    }

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

    }

    public void Death()
    {

        if (gameObject != null && _animator!=null)
        {
            _animator.enabled = false;
            SetState(false);
            _rigidbodies.First().AddForce(_force, ForceMode.Impulse);
            
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject, 2f);
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

    

    
}
