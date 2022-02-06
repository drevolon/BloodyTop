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

    protected float CurrentOmega = 5f;// текущая скорость вращения
    public float StartOmega = 5f; // Начальная скорость вращения
    protected float StartForce = 100f; // Начальная сила 

    public float StartVelocity = 50f; // Начальная скорость движения
    public float deltaVelocity = 0.05f; // скорость убывания движения
    public GameObject path;
    protected float CurrentVelocity;

    protected float LimitOmega = 0.1f; // Обороты, при меньшем значении волчок падает
    protected float maxAngleY = 5f; // Макс отклонение от вертикальной оси при вращении
    protected bool isStart = false; // Флаг о произведенном запуске
    

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _line = GetComponent<LineRenderer>();
        CurrentOmega = StartOmega;



        for (int nPoint = 1; nPoint < 20; nPoint++)
        {
            Transform childPoint = path.transform.GetChild(nPoint);
            float angleChangeVelocity = StartOmega * Time.deltaTime * nPoint *Mathf.Deg2Rad;

            Vector3 pos = childPoint.localPosition;
            float newPosx = pos.x * Mathf.Cos(-angleChangeVelocity) - pos.z * Mathf.Sin(-angleChangeVelocity);
            float newPosz = pos.x * Mathf.Sin(-angleChangeVelocity) + pos.z * Mathf.Cos(-angleChangeVelocity);

            Debug.Log(newPosx.ToString() + " " + newPosz.ToString());

            childPoint.localPosition = new Vector3(newPosx, pos.y, newPosz);
        }


    }
    public void Move()
    {

        // Отлавливаем нажатие мыши и палец
        if (isStart)
        {

            // Уменьшаем скорость на deltaVelocity
            CurrentVelocity -= deltaVelocity * Time.deltaTime;
            if (CurrentVelocity < 0)
            {
                //_rigidbody.velocity = Vector3.zero;
                return;
            }

            // нехрен летать, приземляем на площадку
            if (_transform.position.y>0.2f)
            _transform.position = new Vector3(_transform.position.x, 0.2f, _transform.position.z);

            // Изменим частоту вращения
            //float angleChangeVelocity = CurrentOmega - StartOmega * CurrentVelocity / StartVelocity;
            float angleChangeVelocity = CurrentOmega * Mathf.Deg2Rad * Time.deltaTime;
            CurrentOmega = StartOmega * CurrentVelocity / StartVelocity;

            float dx = _rigidbody.velocity.x * Mathf.Cos(angleChangeVelocity) + _rigidbody.velocity.z * Mathf.Sin(angleChangeVelocity);
            float dz = (-1)*_rigidbody.velocity.x * Mathf.Sin(angleChangeVelocity) + _rigidbody.velocity.z * Mathf.Cos(angleChangeVelocity);

            Vector3 VelocityDirection = new Vector3(_rigidbody.velocity.x + dx, 0f, _rigidbody.velocity.z + dz).normalized * CurrentVelocity;
            _rigidbody.velocity = VelocityDirection;

            // отлавливаем тап
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

                    _rigidbody.velocity = vectorMove;

//                    _rigidbody.AddForce(vectorMove, ForceMode.Impulse);
                  
                }

            }
         //   Debug.Log(CurrentOmega.ToString()+" "+ CurrentVelocity.ToString());
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
//        VectorOS = _transform.rotation * VectorOS;
        //_transform.RotateAround(VectorOS, CurrentOmega * Time.deltaTime);

        _transform.Rotate(VectorOS, CurrentOmega);

        // Проверка запуска
        if (!isStart) CheckMouseDrag();

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
                //fingerUpPos = Input.mousePosition;
                //_line.positionCount = 2;
                //Vector3[] PosArrow = new Vector3[2];
                //PosArrow[0] = _transform.position;
                //float dx = fingerUpPos.x - fingerDownPos.x;
                //float dy = fingerUpPos.y - fingerDownPos.y;
                //float dl = Mathf.Sqrt(dx * dx + dy * dy) / 25;

                //_line.enabled = true;
                //PosArrow[1] = new Vector3(_transform.position.x - dy/10, _transform.position.y, _transform.position.z + dx/10);
                //_line.SetPositions(PosArrow);


                fingerUpPos = Input.mousePosition;
                float dx = fingerUpPos.x - fingerDownPos.x;
                float dy = fingerUpPos.y - fingerDownPos.y;
                float anglePath = Mathf.Atan2(dx, dy) * Mathf.Rad2Deg - 180f;

                float dl = Mathf.Sqrt(dx * dx + dy * dy)/10;
                int countPoints = Mathf.RoundToInt(dl);

                for (int nPoint =0;nPoint<20;nPoint++)
                {
                    path.transform.GetChild(nPoint).gameObject.SetActive(nPoint <= countPoints);
                }

                path.transform.position = _transform.position;
                Vector3 to = new Vector3(0, anglePath, 0);

                path.transform.eulerAngles = to;
                //Vector3.Lerp(transform.rotation.eulerAngles, to, Time.deltaTime);

                path.SetActive(true);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (fingerDownPos != Vector2.zero)
            {
                fingerUpPos = Input.mousePosition;
                float dx = fingerUpPos.x - fingerDownPos.x;
                float dy = fingerUpPos.y - fingerDownPos.y;
                Vector3 vectorMove = new Vector3(_transform.position.x - dy, 0, _transform.position.z + dx);

                vectorMove = vectorMove.normalized * StartVelocity;
                CurrentVelocity = StartVelocity;

                _rigidbody.velocity = vectorMove;
                //_rigidbody.AddForce(vectorMove, ForceMode.Impulse);

                isStart = true;
                fingerDownPos = Vector2.zero;
                fingerUpPos = Vector2.zero;
                _line.enabled = false;

                path.SetActive(false);

            }
        }
    }
}


