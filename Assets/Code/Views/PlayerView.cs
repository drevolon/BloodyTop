using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{

    private UIEventController _eventController;
    private MousePointerController _mouseController;

    public int HP { get; set; } = 10;
    public float Speed { get; set; } = 6f;

    public float StartOmega = 5f; // Начальная скорость вращения
    public GameObject LineTarget; // Объект с LineRendere, который рисует траекторию движения
    public float StartVelocity = 50f; // Начальная скорость движения
    public float deltaVelocity = 0.05f; // скорость убывания движения
    public float angleCurvePath = 5f;
    public float deltaBoosterVelocity = 0.5f; // Изменение скорости (в долях единицы) от текущей, при столкновении с объектами-бустерами
    public float SlowMotionRate = 0.2f;

    protected Rigidbody _rigidbody;
    protected Transform _transform;

    protected LineRenderer _line; // LineRenderer для траектории движения

    protected float CurrentOmega = 5f;// текущая скорость вращения
    protected float StartForce = 100f; // Начальная сила 

    //public GameObject path; // Объект, рисующий траекторию (устарело)
    public float CurrentVelocity; // Текущая линейная скорость волчка

    protected float LimitOmega = 0.1f; // Обороты, при меньшем значении снимается ограничение на отклонение от оси Y
    protected float maxAngleY = 5f; // Макс отклонение от вертикальной оси при вращении до достижении скорости вращения LimitOmega
    public bool isStart = false; // Флаг о произведенном запуске
    protected Vector3[] TraceTarget = new Vector3[20]; // Массив, хранящий первоначальную траектрию


    private void Start()
    {
        _eventController = new UIEventController();

        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _mouseController = gameObject.AddComponent<MousePointerController>();
        _mouseController.UIEvent = _eventController;

        CurrentOmega = StartOmega;
        CurrentVelocity = StartVelocity;

        isStart = false;

        // LineRenderer у отдельного объекта
        
        _line = UnityEngine.Object.Instantiate(LineTarget).GetComponent<LineRenderer>();
        _line.enabled = false;

        float deltaTimeBetweenTargetPoint = 1f / StartVelocity;
        for (int nPoint = 0; nPoint < 20; nPoint++)
        {
            Vector3 pos = new Vector3(nPoint + 1f, 0f, 0f);
            float angleChangeVelocity = angleCurvePath * deltaTimeBetweenTargetPoint * nPoint * Mathf.Deg2Rad;
            float newPosx = pos.x * Mathf.Cos(-angleChangeVelocity) - pos.z * Mathf.Sin(-angleChangeVelocity);
            float newPosz = pos.x * Mathf.Sin(-angleChangeVelocity) + pos.z * Mathf.Cos(-angleChangeVelocity);
            TraceTarget[nPoint] = new Vector3(newPosx, pos.y, newPosz);
        }

        _eventController.OnBloodyTopEndTap += OnStartBloodyTop;
        _eventController.OnBloodyTopBeginTap += StartSlowMotion;
        _eventController.OnBloodyTopTargeting += OnUpdateTraceLine;

    }
    private void FixedUpdate()
    {

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
            if (_transform.position.y > 0.2f)
                _transform.position = new Vector3(_transform.position.x, 0.2f, _transform.position.z);

            // Изменим частоту вращения
            //float angleChangeVelocity = CurrentOmega - StartOmega * CurrentVelocity / StartVelocity;
            CurrentOmega = StartOmega * CurrentVelocity / StartVelocity;
            float angleChangeVelocity = angleCurvePath * Mathf.Deg2Rad * Time.deltaTime * CurrentOmega / StartOmega;

            float dx = _rigidbody.velocity.x * Mathf.Cos(angleChangeVelocity) + _rigidbody.velocity.z * Mathf.Sin(angleChangeVelocity);
            float dz = (-1) * _rigidbody.velocity.x * Mathf.Sin(angleChangeVelocity) + _rigidbody.velocity.z * Mathf.Cos(angleChangeVelocity);


            Vector3 VelocityDirection = new Vector3(_rigidbody.velocity.x + dx, 0f, _rigidbody.velocity.z + dz).normalized * CurrentVelocity;
            _rigidbody.velocity = VelocityDirection;

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
        _transform.Rotate(VectorOS, CurrentOmega * Time.deltaTime);

        // приаязываем к волчку линию прицела
        _line.gameObject.transform.position = new Vector3(_transform.position.x, _line.gameObject.transform.position.y, _transform.position.z);

    }

    protected void OnStartBloodyTop(PointerPoint _point)
    {
        if (!isStart)
        {
            if ((_point.fingerDownPos != Vector2.zero) && (_point.fingerUpPos != Vector2.zero))
            {
                float dx = _point.fingerUpPos.x - _point.fingerDownPos.x;
                float dy = _point.fingerUpPos.y - _point.fingerDownPos.y;
                Vector3 vectorMove = new Vector3(_transform.position.x - dy, 0, _transform.position.z + dx);
                vectorMove = vectorMove.normalized * CurrentVelocity;
                _rigidbody.velocity = vectorMove;
            }

            isStart = true;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;

            _line.enabled = false;
        }


    }

    protected void OnUpdateTraceLine(PointerPoint _point)
    {
        if (!isStart)
        {

            // Прицел с использованием LineRenderer
            if (_point.fingerDownPos != Vector2.zero)
            {
                float dx = _point.fingerUpPos.x - _point.fingerDownPos.x;
                float dy = _point.fingerUpPos.y - _point.fingerDownPos.y;
                float anglePath = Mathf.Atan2(dx, dy) * Mathf.Rad2Deg - 180f;

                float dl = Mathf.Sqrt(dx * dx + dy * dy) / 10;
                int countPoints = Mathf.RoundToInt(dl);
                if (countPoints > TraceTarget.Length) countPoints = TraceTarget.Length;
                _line.gameObject.transform.position = new Vector3(_transform.position.x, _line.gameObject.transform.position.y, _transform.position.z);

                _line.positionCount = countPoints;
                Vector3[] posArray = new Vector3[countPoints];
                Array.Copy(TraceTarget, posArray, countPoints);
                _line.SetPositions(posArray);
                _line.enabled = true;

                Vector3 to = new Vector3(0, anglePath, 0);
                _line.gameObject.transform.eulerAngles = to;
            }
        }
    }

    private void StartSlowMotion()
    {
        if (isStart)
        {
            isStart = false;
            Time.timeScale = SlowMotionRate;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            StartCoroutine("WaitSlowMotion");
        }
    }

    IEnumerator WaitSlowMotion()
    {
        yield return new WaitForSeconds(3f * Time.timeScale);
        OnStartBloodyTop(new PointerPoint());
    }


    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "UpSpeed")
        {
            CurrentVelocity = CurrentVelocity * (1 + deltaBoosterVelocity);
            //Debug.Log("Increase Speed");
        }
        if (other.gameObject.tag == "DownSpeed")
        {
            CurrentVelocity = CurrentVelocity * (1 - deltaBoosterVelocity);
            // Debug.Log("Decrease Speed");
        }
    }
}
