using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Node = UnityEngine.XR.XRNode;

struct HandStatus
{
    public Vector3 LTouchPos;
    public Vector3 RTouchPos;
    public Quaternion LTouchRot;
    public Quaternion RTouchRot;
}

public class HandTrackingBehaviour : MonoBehaviour
{
    public Transform LTouchParent;
    public Transform RTouchParent;
    
    private InfoText info;

    private HandStatus[] statusBuffer = new HandStatus[31];
    private int currStatusIdx = 0;
    private int delay = 0;
    private HeadTrackingBehaviour headTracking;

    private bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        headTracking = GetComponent<HeadTrackingBehaviour>();
        info = GetComponent<InfoText>();
    }

    // Update is called once per frame
    void Update()
    {
        //update tracking delay
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            if(!pressed)
            {
                pressed = true;

                delay += 1;
                if (delay > 30) delay = 30;
                //P2Utils.instance.setDelay(delay);
                headTracking.SetDelay(delay);
                info.ShowTopInfo("Tracking lag: " + delay.ToString() + " frames");
            }
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            if(!pressed)
            {
                pressed = true;

                delay -= 1;
                if (delay < 0) delay = 0;
                //P2Utils.instance.setDelay(delay);
                headTracking.SetDelay(delay);
                info.ShowTopInfo("Tracking lag: " + delay.ToString() + " frames");
            }
        }
        else
        {
            pressed = false;
        }

        // fill buffer
        statusBuffer[currStatusIdx].LTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        statusBuffer[currStatusIdx].RTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        statusBuffer[currStatusIdx].LTouchRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        statusBuffer[currStatusIdx].RTouchRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);

        //update controller tracking
        int delayStatusIdx = currStatusIdx - delay;
        if (delayStatusIdx < 0) delayStatusIdx += 31;

        currStatusIdx += 1;
        if (currStatusIdx > 30) currStatusIdx = 0;

        LTouchParent.localPosition = statusBuffer[delayStatusIdx].LTouchPos;
        RTouchParent.localPosition = statusBuffer[delayStatusIdx].RTouchPos;
        LTouchParent.localRotation = statusBuffer[delayStatusIdx].LTouchRot;
        RTouchParent.localRotation = statusBuffer[delayStatusIdx].RTouchRot;

        //LTouchParent.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        //RTouchParent.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        //LTouchParent.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        //RTouchParent.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
    }
}
