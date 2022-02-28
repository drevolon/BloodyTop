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
                UIEventController.DownPointer(touch.position);
            }

            if (touch.phase == TouchPhase.Moved)
            {
                UIEventController.SwipePointer(touch.position);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                UIEventController.UpPointer(touch.position);
            }

        }
    }

}
