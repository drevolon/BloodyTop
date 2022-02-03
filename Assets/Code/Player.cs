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
    protected float CurrentOmega = 5f;
    protected float StartOmega = 5f;
    protected float StartForce = 20f;
    protected Vector3 vectorMove = new Vector3(1f, 0, -1f);
    protected float StartVelocity=0f;
    protected float LimitOmega = 0.3f;
    bool f = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        CurrentOmega = StartOmega;
    // _rigidbody.AddForce(vectorMove, ForceMode.Impulse);
    //_rigidbody.AddTorque(new Vector3(0f, 1f, 0f) * 50050f, ForceMode.Impulse);
}
    public void Move()
    {

        // Отлавливаем нажатие мыши
        if (Input.GetKey(KeyCode.Mouse0))
        {
            float ModulV = StartForce; // _rigidbody.velocity.magnitude;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit Po;
            if (Physics.Raycast(ray, out Po))
            {
                Vector3 mousePos = new Vector3(Po.point.x, Po.point.y, Po.point.z);
                Vector3 vectorMove = mousePos - _transform.position;
                vectorMove.y = 0;
                vectorMove = vectorMove.normalized * ModulV;
                _rigidbody.AddForce(vectorMove, ForceMode.Impulse);
                StartVelocity = _rigidbody.velocity.magnitude;
                Debug.Log("START="+_rigidbody.velocity.magnitude.ToString());
                f = true;
            }

        }

        //if (ValueForce > 0) ValueForce -= 0.1f;


        if (StartVelocity>0) CurrentOmega = _rigidbody.velocity.magnitude / StartVelocity;

        if (f) Debug.Log(CurrentOmega);

        if (CurrentOmega > LimitOmega)
        {
            // Ограничиваем прецессию 10 градусами по X и Z
            Vector3 angles = _transform.rotation.eulerAngles;
            float angleX = angles.x;
            float signX = Mathf.Sign(angleX);
            angleX = Mathf.Abs(angleX);

            if (angleX > 350) { angleX = angleX - 350; signX = signX * (-1); }
            float angleZ = angles.z;
            float signZ = Mathf.Sign(angleZ);
            angleZ = Mathf.Abs(angleZ);
            if (angleZ > 350) { angleZ = angleZ - 350; signZ = signZ * (-1); }
     

        if (angleX > 5) angleX = 5;
        if (angleZ > 5) angleZ = 5;

        angleX = angleX - angleX * 0.1f;
        if (angleX < 0) angleX = 0;

        angleZ = angleZ - angleZ * 0.1f;
        if (angleZ < 0) angleZ = 0;


        angles = new Vector3(angleX*signX, angles.y, angleZ*signZ);

        _transform.rotation = Quaternion.Euler(angles);
        }


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


