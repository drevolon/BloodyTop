using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IMove, IHeal
{
    public int HP { get; set; } = 10;
    public float Speed { get; set; } = 6f;

    protected Rigidbody _rigidbody;
    protected Transform _transform;
    private Vector2 fingerDownPos = Vector2.zero;
    private Vector2 fingerUpPos = Vector2.zero;
    protected LineRenderer _line;
    Touch touch;
    protected float CurrentOmega = 5f;
    protected float StartOmega = 5f;
    protected float StartForce = 200f;
    protected Vector3 vectorMove = new Vector3(1f, 0, -1f);
    public float StartVelocity = 0f;
    protected float LimitOmega = 0.3f;
    protected float maxAngleY = 5f;
    protected bool isStart = false;
    

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _line = GetComponent<LineRenderer>();
        CurrentOmega = StartOmega;
        // _rigidbody.AddForce(vectorMove, ForceMode.Impulse);
        //_rigidbody.AddTorque(new Vector3(0f, 1f, 0f) * 50050f, ForceMode.Impulse);
    }
    public void Move()
    {
        if (StartVelocity == 0)
            if (isStart)
                StartVelocity = _rigidbody.velocity.magnitude;

        // Отлавливаем нажатие мыши и палец
        if (StartVelocity > 0)
        {
            if (Input.GetMouseButton(0) || (Input.touchCount > 0))
            {
                Vector3 InputPosition;
                if (Input.GetMouseButton(0))
                {
                    InputPosition = Input.mousePosition;
                }
                if (Input.touchCount > 0)
                {
                    InputPosition = Input.GetTouch(0).position;
                }

                float ModulV = _rigidbody.velocity.magnitude;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit Po;
                if (Physics.Raycast(ray, out Po))
                {
                    Vector3 mousePos = new Vector3(Po.point.x, Po.point.y, Po.point.z);
                    Vector3 vectorMove = mousePos - _transform.position;
                    vectorMove.y = 0;
                    vectorMove = vectorMove.normalized * ModulV;
                    _rigidbody.AddForce(vectorMove, ForceMode.Impulse);
//                    StartVelocity = _rigidbody.velocity.magnitude;
                    // Debug.Log("START=" + _rigidbody.velocity.magnitude.ToString());
                  
                }

            }
        }

        if (StartVelocity > 0)
        {
            CurrentOmega = _rigidbody.velocity.magnitude / StartVelocity;
            Debug.Log("omega="+CurrentOmega.ToString());
        }


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


            if (angleX > maxAngleY) angleX = maxAngleY;
            if (angleZ > maxAngleY) angleZ = maxAngleY;

            angleX = angleX - angleX * 0.1f;
            if (angleX < 0) angleX = 0;

            angleZ = angleZ - angleZ * 0.1f;
            if (angleZ < 0) angleZ = 0;


            angles = new Vector3(angleX * signX, angles.y, angleZ * signZ);

            _transform.rotation = Quaternion.Euler(angles);
        }


        // Вращаем
        Vector3 VectorOS = new Vector3(0f, 1f, 0f);
        VectorOS = _transform.rotation * VectorOS;
        _transform.RotateAround(VectorOS, CurrentOmega);

        // Проверка запуска
        if (!isStart) CheckMouseDrag();

        //_rigidbody.AddTorque(VectorOS * 500f, ForceMode.Acceleration);

        //_transform.rotation = Quaternion.AngleAxis(CurrentOmega, Vector3.up);
        // _transform.Rotate(new Vector3(0f, 1f, 0f), CurrentOmega* Time.deltaTime);

    }

    private void CheckMouseDrag()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (fingerDownPos == Vector2.zero)
            {
                fingerDownPos = Input.mousePosition;
                fingerUpPos = Vector2.zero;
            }
        }

        if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {

            if (fingerDownPos != Vector2.zero)
            {
                fingerUpPos = Input.mousePosition;
                _line.positionCount = 2;
                Vector3[] PosArrow = new Vector3[2];
                PosArrow[0] = _transform.position;
                float dx = fingerUpPos.x - fingerDownPos.x;
                float dy = fingerUpPos.y - fingerDownPos.y;
                float dl = Mathf.Sqrt(dx * dx + dy * dy)/5;
                _line.enabled = true;
                PosArrow[1] = new Vector3(_transform.position.x - dy, _transform.position.y, _transform.position.z + dx);
                _line.SetPositions(PosArrow);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (fingerDownPos != Vector2.zero)
            {
                fingerUpPos = Input.mousePosition;
                float dx = fingerUpPos.x - fingerDownPos.x;
                float dy = fingerUpPos.y - fingerDownPos.y;
                vectorMove = new Vector3(_transform.position.x - dy, 0, _transform.position.z + dx);

                vectorMove = vectorMove.normalized * StartForce;

                _rigidbody.AddForce(vectorMove, ForceMode.Impulse);

                isStart = true;
                fingerDownPos = Vector2.zero;
                fingerUpPos = Vector2.zero;
                _line.enabled = false;
            }
        }
    }
}


