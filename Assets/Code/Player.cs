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
    protected Vector3 vectorMove = new Vector3(1f, 0, -1f) * 10f;
    protected float CurrentOmega = 80f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        //_rigidbody.AddForce(vectorMove);
        //_rigidbody.AddTorque(new Vector3(0f, 1f, 0f) * 50050f, ForceMode.Impulse);
    }
    public void Move()
    {

        // Отлавливаем нажатие мыши
        if (Input.GetKey(KeyCode.Mouse0))
        {
            float ModulV = vectorMove.magnitude;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit Po;
            if (Physics.Raycast(ray, out Po))
            {
                Vector3 mousePos = new Vector3(Po.point.x, Po.point.y, Po.point.z);
                vectorMove = mousePos - _transform.position;
                vectorMove.y = 0;
                vectorMove = vectorMove.normalized * ModulV;
                _rigidbody.AddForce(vectorMove, ForceMode.Impulse);
            }

        }

        //if (ValueForce > 0) ValueForce -= 0.1f;



        // Ограничиваем прецессию 10 градусами по X и Z
        Vector3 angles = _transform.rotation.eulerAngles;
        float angleX = angles.x;
        float signX = Mathf.Sign(angleX);
        angleX = Mathf.Abs(angleX);

        Debug.Log(angleX);

        if (angleX > 350) { angleX = angleX - 350; signX = signX * (-1); }
        float angleZ = angles.z;
        float signZ = Mathf.Sign(angleZ);
        angleZ = Mathf.Abs(angleZ);
        if (angleZ > 350) { angleZ = angleZ - 350; signZ = signZ * (-1); }

        Debug.Log(angleZ);

        if (angleX > 10) angleX = 0 * signX;
        if (angleZ > 10) angleZ = 0 * signZ;
        angles = new Vector3(angleX, angles.y, angleZ);

        _transform.rotation = Quaternion.Euler(angles);


        // Вращаем
        Vector3 VectorOS = new Vector3(0f, 1f, 0f);
        VectorOS = _transform.rotation * VectorOS;
        _transform.RotateAround(VectorOS, CurrentOmega);

        //_rigidbody.AddTorque(VectorOS * 500f, ForceMode.Acceleration);

        //_transform.rotation = Quaternion.AngleAxis(CurrentOmega, Vector3.up);
        // _transform.Rotate(new Vector3(0f, 1f, 0f), CurrentOmega* Time.deltaTime);

    }

    public void MoveClickMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }
}


