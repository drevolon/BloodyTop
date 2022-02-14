using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDetector : MonoBehaviour
{
	private Vector2 fingerDownPos;
	private Vector2 fingerUpPos;
    public static UIDetector instance = null;

	public float SWIPE_THRESHOLD = 20f;

	private bool isStart = false;
    public enum typeSwipeDirection
    {
        Left,
        Right,
        Up,
        Down,
        Unknown
    };

    public static typeSwipeDirection SwipeDirection;

    // event ����� ������
    public delegate void BloodyTopStart(Vector3 fingerUpPos, Vector3 fingerDownPos); // �������� ����� �������
    public event BloodyTopStart OnBloodyTopStart; // ������� �� ��� ������ �������

    // event ����� ����������� ������
    public delegate void BloodyTopTargeting(Vector3 fingerUpPos, Vector3 fingerDownPos); // �������� ����� �������
    public event BloodyTopTargeting OnBloodyTopTargeting; // ������� �� ��� ������ �������

    // event ������ ������
    public delegate void BloodyTopRight(); // �������� ����� �������
    public event BloodyTopRight OnBloodyTopRight; // ������� �� ��� ������ �������

    // event ������ �����
    public delegate void BloodyTopLeft(); // �������� ����� �������
    public event BloodyTopLeft OnBloodyTopLeft; // ������� �� ��� ������ �������

    // event ������ �����
    public delegate void BloodyTopUp(); // �������� ����� �������
    public event BloodyTopUp OnBloodyTopUp; // ������� �� ��� ������ �������

    // event ������ ����
    public delegate void BloodyTopDown(); // �������� ����� �������
    public event BloodyTopDown OnBloodyTopDown; // ������� �� ��� ������ �������

    // event ��������� �������������� ����������
    public delegate void EndCorrectDirection(); // �������� ����� �������
    public event EndCorrectDirection OnEndCorrectDirection; // ������� �� ��� ������ �������

    void Awake()
    {
        // ���������, ����� �� ������� ������ ���������
        if (instance == null)
        { // ������� �� �����
            instance = this; // ���������� � ������� ������� ������
        }
    }
    // Update is called once per frame
    void Update ()
	{

		CheckMouseDrag(isStart);
		CheckTouch(isStart);

	}

    private void SwipePointer(Vector3 pos)
    {
        fingerUpPos = pos;
        if (!isStart)
            OnBloodyTopTargeting?.Invoke(fingerDownPos, fingerUpPos);
        else
            switch (DetectSwipe())
            {
                case typeSwipeDirection.Left:
                    OnBloodyTopLeft?.Invoke();
                    break;
                case typeSwipeDirection.Right:
                    OnBloodyTopRight?.Invoke();
                    break;
                case typeSwipeDirection.Up:
                    OnBloodyTopUp?.Invoke();
                    break;
                case typeSwipeDirection.Down:
                    OnBloodyTopDown?.Invoke();
                    break;
            }
    }
    protected void instanceUpPointer(Vector3 pos)
    {
        fingerUpPos = pos;
        if (!isStart)
        {
            isStart = true;
            OnBloodyTopStart?.Invoke(fingerDownPos, fingerUpPos);
        }
        else
        {
            OnEndCorrectDirection?.Invoke();
        }
        fingerDownPos = Vector2.zero;
    }
    private void CheckMouseDrag(bool isStart)
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
                SwipePointer(Input.mousePosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (fingerDownPos != Vector2.zero)
            {
                instanceUpPointer(Input.mousePosition);
            }
        }
    }


    protected void CheckTouch(bool isStart)
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPos = touch.position;
                fingerUpPos = Vector3.zero;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if (fingerDownPos != Vector2.zero)
                {
                    SwipePointer(touch.position);
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (fingerDownPos != Vector2.zero)
                {
                    instanceUpPointer(touch.position);
                }
            }
        }
    }

    typeSwipeDirection DetectSwipe()
    {

        if (VerticalMoveValue() > SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue()) {
            if (fingerDownPos.y - fingerUpPos.y > 0) {
                return typeSwipeDirection.Up;
            } else if (fingerDownPos.y - fingerUpPos.y < 0) {
                return typeSwipeDirection.Down;
            }

        } else if (HorizontalMoveValue() > SWIPE_THRESHOLD && HorizontalMoveValue() > VerticalMoveValue()) {
            if (fingerDownPos.x - fingerUpPos.x > 0) {
                return typeSwipeDirection.Left;
            } else if (fingerDownPos.x - fingerUpPos.x < 0) {
                return typeSwipeDirection.Right;
            }

        }

            return typeSwipeDirection.Unknown;
    }

    float VerticalMoveValue ()
	{
		return Mathf.Abs (fingerDownPos.y - fingerUpPos.y);
	}

	float HorizontalMoveValue ()
	{
		return Mathf.Abs (fingerDownPos.x - fingerUpPos.x);
	}

}
