using System;
using UnityEngine;
using UnityEngine.Events;

public class UIEventController  
{
    public static UIEventController instance;

    private PointerPoint _point = new PointerPoint();
    // event старт волчка
    public Action<PointerPoint> OnBloodyTopEndTap;

    // event старт волчка
    public Action OnBloodyTopBeginTap;

    // event Выбор направления старта
    public Action<PointerPoint> OnBloodyTopTargeting;

    // event Волчок вправо
    public Action OnBloodyTopRight; // Создать на его основе событие

    // event Волчок влево
    public Action OnBloodyTopLeft; // Создать на его основе событие

    // event Волчок вверх
    public Action OnBloodyTopUp; // Создать на его основе событие

    // event Волчок вниз
    public Action OnBloodyTopDown; // Создать на его основе событие

    // event закончили корректировать траекторию
    public Action OnEndCorrectDirection; // Создать на его основе событие

    // event single tap
    public Action OnBloodyTopTap; // Создать на его основе событие




    public UIEventController()
    {
        // Проверяем, задан ли инстанс нашего менеджера
        if (instance == null)
        { // Инстанс не задан
            instance = this; // Установить в инстанс текущий объект
        }
    }
    public void SwipePointer(Vector3 pos)
    {
        _point.fingerUpPos = pos;
        OnBloodyTopTargeting?.Invoke(_point);

        switch (DetectSwipe())
        {
            case SwipeDirection.Left:
                OnBloodyTopLeft?.Invoke();
                break;
            case SwipeDirection.Right:
                OnBloodyTopRight?.Invoke();
                break;
            case SwipeDirection.Up:
                OnBloodyTopUp?.Invoke();
                break;
            case SwipeDirection.Down:
                OnBloodyTopDown?.Invoke();
                break;
        }
    }
    public void DownPointer(Vector3 pos)
    {
        if (_point.fingerDownPos == Vector2.zero)
        {
            _point.fingerDownPos = pos;
            _point.fingerUpPos = Vector2.zero;
            OnBloodyTopBeginTap?.Invoke();
        }
    }

    public void UpPointer(Vector3 pos)
    {
        _point.fingerUpPos = pos;
        if (DetectSwipe() != SwipeDirection.Unknown)
        {
            OnBloodyTopEndTap?.Invoke(_point);
            OnEndCorrectDirection?.Invoke();
        }
        else
        {
            OnBloodyTopTap?.Invoke();
        }

        _point.fingerDownPos = Vector2.zero;
    }

    private SwipeDirection DetectSwipe()
    {

        if (VerticalMoveValue() > _point.SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue())
        {
            if (_point.fingerDownPos.y - _point.fingerUpPos.y > 0)
            {
                return SwipeDirection.Up;
            }
            else if (_point.fingerDownPos.y - _point.fingerUpPos.y < 0)
            {
                return SwipeDirection.Down;
            }

        }
        else if (HorizontalMoveValue() > _point.SWIPE_THRESHOLD && HorizontalMoveValue() > VerticalMoveValue())
        {
            if (_point.fingerDownPos.x - _point.fingerUpPos.x > 0)
            {
                return SwipeDirection.Left;
            }
            else if (_point.fingerDownPos.x - _point.fingerUpPos.x < 0)
            {
                return SwipeDirection.Right;
            }

        }

        return SwipeDirection.Unknown;
    }

    private float VerticalMoveValue()
    {
        return Mathf.Abs(_point.fingerDownPos.y - _point.fingerUpPos.y);
    }

    private float HorizontalMoveValue()
    {
        return Mathf.Abs(_point.fingerDownPos.x - _point.fingerUpPos.x);
    }
}
