using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{

    public float StartOmega = 1500f; // Начальная скорость вращения
    public float StartVelocity = 50f; // Начальная скорость движения
    public float deltaVelocity = 0.05f; // скорость убывания движения
    public float angleCurvePath = 5f;
    public float deltaBoosterVelocity = 0.5f; // Изменение скорости (в долях единицы) от текущей, при столкновении с объектами-бустерами

    public ProfilePlayer _profilePlayer;

    public float CurrentOmega = 5f;// текущая скорость вращения
    public float CurrentVelocity; // Текущая линейная скорость волчка

    protected float LimitOmega = 0.1f; // Обороты, при меньшем значении снимается ограничение на отклонение от оси Y
    protected float maxAngleY = 5f; // Макс отклонение от вертикальной оси при вращении до достижении скорости вращения LimitOmega

    public bool isStart = false; // Флаг о произведенном запуске
    protected Rigidbody _rigidbody;
    protected Transform _transform;



    public void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();

        CurrentOmega = StartOmega;
        CurrentVelocity = StartVelocity;
        _profilePlayer.CurrentPlayerState.SubscribeOnChange(OnChangePlayerState);

        UpdateManager.SubscribeToUpdate(ChangeVerticalAngle);
        UpdateManager.SubscribeToUpdate(UpdateRotate);

    }

    private void OnChangePlayerState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.NotStart:
                break;
            case PlayerState.Start:
                UpdateManager.SubscribeToUpdate(UpdatePositionTop);
                break;
            case PlayerState.SlowMotion:
                break;
            default:
                break;
        }

    }

    private void UpdatePositionTop()
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

    private void ChangeVerticalAngle()
    {
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
    }
    private void UpdateRotate()
    {
        // Вращаем
        Vector3 VectorOS = new Vector3(0f, 1f, 0f);
        _transform.Rotate(VectorOS, CurrentOmega * Time.deltaTime);
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
