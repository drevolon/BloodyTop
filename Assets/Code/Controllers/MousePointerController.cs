using UnityEngine;

public class MousePointerController : MonoBehaviour
{
    public UIEventController UIEvent;
    private PointerPoint _point = new PointerPoint();


    public MousePointerController(UIEventController EventClass)
    {
        UIEvent = EventClass;
    }
    private void Update()
    {
        CheckMouseDrag();
    }


    private void CheckMouseDrag()
    {

        if (Input.GetMouseButtonDown(0))
        {
            UIEvent.DownPointer(Input.mousePosition);
        }

        if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {
            UIEvent.SwipePointer(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            UIEvent.UpPointer(Input.mousePosition);
        }
    }


  
}
