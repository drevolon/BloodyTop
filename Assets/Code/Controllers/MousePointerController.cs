using UnityEngine;

public class MousePointerController : BaseController, IPointerController
{
//    public UIEventController UIEvent { get; set; }


    //public MousePointerController(UIEventController EventClass)
    //{
    //    UIEvent = EventClass;
    //}
    private void Update()
    {
        GetPointerUpdate();
    }

    private void GetPointerUpdate()
    {

        if (Input.GetMouseButtonDown(0))
        {
            UIEventController.instance.DownPointer(Input.mousePosition);
        }

        if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {
            UIEventController.instance.SwipePointer(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            UIEventController.instance.UpPointer(Input.mousePosition);
        }
    }


  
}
