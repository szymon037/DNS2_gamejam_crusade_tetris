using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDetector : MonoBehaviour
{
    public enum Controller
    {
        NONE,
        PS4,
        XBOX1,
    }

    public List<Controller> CurrentControllers;

    void Start()
    {
        CurrentControllers = new List<Controller>();
        //"DUALSHOCK®4 USB Wireless Adaptor"
        //"Controller (Xbox One For Windows)"
    }

    void Update()
    {
        StartCoroutine(DetectNewControllers(CurrentControllers));
    }

    private IEnumerator DetectNewControllers(List<Controller> controllers)
    {
        while(true)
        {
            CheckControllers(controllers);
            HandleNewController(controllers);
            yield return null;
        }
    }

    private Controller ControllerType(string controllerName)
    {
        Controller result = Controller.NONE;

        if (controllerName != "")
        {
            if (controllerName == "DUALSHOCK®4 USB Wireless Adaptor")
            {
                result = Controller.PS4;
            }
            else if (controllerName == "Controller (Xbox One For Windows)")
            {
                result = Controller.XBOX1;
            }
        }

        return result;
    }

    private void CheckControllers(List<Controller> controlles)
    {
        var controllerNames = Input.GetJoystickNames();
        for(int i = 0; i < controlles.Count; ++i)
        {
            controlles[i] = ControllerType(controllerNames[i]);
        }
    }

    private void HandleNewController(List<Controller> controllers)
    {
        if(controllers.Count < Input.GetJoystickNames().Length)
        {
            Controller NewController = ControllerType(Input.GetJoystickNames()[controllers.Count]);

            if (NewController != Controller.NONE)
            {
                controllers.Add(NewController);
            }
        }
    }
}
