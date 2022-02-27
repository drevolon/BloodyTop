using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIView : MonoBehaviour
{
    [SerializeField]
    float SlowMotionRate = 0.2f;

    public ProfilePlayer _profilePlayer;
    protected Rigidbody _rigidbody;
    protected Transform _transform;
    public PlayerView _view;

    protected float LimitOmega = 0.1f; // Обороты, при меньшем значении снимается ограничение на отклонение от оси Y
    protected float maxAngleY = 5f; // Макс отклонение от вертикальной оси при вращении до достижении скорости вращения LimitOmega
    public GameObject LineTarget;
    private LineRenderer _line; // LineRenderer для прицеливания
    public bool isStart = false; // Флаг о произведенном запуске
    protected Vector3[] TraceTarget = new Vector3[20]; // Массив, хранящий первоначальную траектрию

    public void Init()
    {

        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();

        // LineRenderer у отдельного объекта
        
        _line = UnityEngine.Object.Instantiate(LineTarget).GetComponent<LineRenderer>();
        _line.enabled = false;

        float deltaTimeBetweenTargetPoint = 1f / _view.StartVelocity;
        for (int nPoint = 0; nPoint < 20; nPoint++)
        {
            Vector3 pos = new Vector3(nPoint + 1f, 0f, 0f);
            float angleChangeVelocity = _view.angleCurvePath * deltaTimeBetweenTargetPoint * nPoint * Mathf.Deg2Rad;
            float newPosx = pos.x * Mathf.Cos(-angleChangeVelocity) - pos.z * Mathf.Sin(-angleChangeVelocity);
            float newPosz = pos.x * Mathf.Sin(-angleChangeVelocity) + pos.z * Mathf.Cos(-angleChangeVelocity);
            TraceTarget[nPoint] = new Vector3(newPosx, pos.y, newPosz);
        }
       
    }
    private void UpdatePosLineTarget()
    {
        // приаязываем к волчку линию прицела
        _line.gameObject.transform.position = new Vector3(_transform.position.x, _line.gameObject.transform.position.y, _transform.position.z);

    }

    public void OnStartBloodyTop(PointerPoint _point)
    {
        if ((_point.fingerDownPos != Vector2.zero) && (_point.fingerUpPos != Vector2.zero))
        {
            float dx = _point.fingerUpPos.x - _point.fingerDownPos.x;
            float dy = _point.fingerUpPos.y - _point.fingerDownPos.y;
            Vector3 vectorMove = new Vector3(_transform.position.x - dy, 0, _transform.position.z + dx);
            vectorMove = vectorMove.normalized * _view.CurrentVelocity;
            _rigidbody.velocity = vectorMove;
        }
        
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        _line.enabled = false;

        _profilePlayer.CurrentPlayerState.Value = PlayerState.Start;


    }

    public void OnUpdateTraceLine(PointerPoint _point)
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

    public void StartSlowMotion()
    {
        Time.timeScale = SlowMotionRate;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        StartCoroutine("WaitSlowMotion");
        _profilePlayer.CurrentPlayerState.Value = PlayerState.SlowMotion;
    }

    IEnumerator WaitSlowMotion()
    {
        yield return new WaitForSeconds(3f * Time.timeScale);
        OnStartBloodyTop(new PointerPoint());
    }
    public void notStartTop()
    {
        UIEventController.instance.OnBloodyTopTargeting += OnUpdateTraceLine;
        UIEventController.instance.OnBloodyTopBeginTap -= StartSlowMotion;
        UIEventController.instance.OnBloodyTopEndTap += OnStartBloodyTop;
        UpdateManager.SubscribeToUpdate(UpdatePosLineTarget);
    }
    public void StartTop()
    {
        UIEventController.instance.OnBloodyTopTargeting -= OnUpdateTraceLine;
        UIEventController.instance.OnBloodyTopBeginTap += StartSlowMotion;
        UIEventController.instance.OnBloodyTopEndTap -= OnStartBloodyTop;
        UpdateManager.UnsubscribeFromUpdate(UpdatePosLineTarget);

    }
}
