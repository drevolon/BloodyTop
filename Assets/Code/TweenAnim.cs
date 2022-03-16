using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenAnim : MonoBehaviour
{
    [SerializeField]
    private float _duration=3f;
    [SerializeField]
    private PathType _pathType = PathType.Linear;
    [SerializeField]
    private Transform[] _points;

    [SerializeField]
    private Vector3 _point=new Vector3(10f,0f, 0f);


    private List<Vector3> _pointPosition = new List<Vector3>();


    private void Start()
    {
        foreach (var point in _points)
            _pointPosition.Add(point.position);
        transform.DOPath(_pointPosition.ToArray(), _duration, _pathType);

        //transform.DOMove(_point, _duration);
    }

}
