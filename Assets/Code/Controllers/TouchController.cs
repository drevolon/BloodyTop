using UnityEngine;

public class TouchController : BaseController, IPointerController
{
    //public UIEventController UIEvent { get; set; }

    //public TouchController(UIEventController EventClass)
    //{
    //    UIEvent = EventClass;
    //}
    private void Update()
    {
        GetPointerUpdate();
    }

    private void GetPointerUpdate()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                UIEventController.instance.DownPointer(touch.position);
            }

            if (touch.phase == TouchPhase.Moved)
            {
                UIEventController.instance.SwipePointer(touch.position);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                UIEventController.instance.UpPointer(touch.position);
            }

        }
    }

}
