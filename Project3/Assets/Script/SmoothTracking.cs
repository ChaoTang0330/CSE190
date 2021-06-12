using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTracking : MonoBehaviour
{
    public Transform sphereTrans;
    public Vector3[] posBuffer = new Vector3[46];

    private InfoText info;
    private float windowSize = 1.0f;
    private bool smooth = false;
    private bool pressed = false;
    private int currIdx = 0;

    private Vector3 iniLocPos;
    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<InfoText>();
        iniLocPos = sphereTrans.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
        {
            if(!pressed)
            {
                pressed = true;
                if(smooth)
                {
                    smooth = false;
                    sphereTrans.localPosition = iniLocPos;
                    info.ShowTopInfo("Smooth tracking: off");
                }
                else
                {
                    smooth = true;
                    windowSize = 1.0f;
                    info.ShowTopInfo("Smooth tracking: on");
                }
            }
        }
        else
        {
            pressed = false;

            if(smooth)
            {
                float speedFactor = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
                if (Mathf.Abs(speedFactor) > 1e-4)
                {
                    windowSize += speedFactor * 4.0f * Time.deltaTime;
                    if (windowSize < 1.0f) windowSize = 1.0f;
                    else if (windowSize > 45.0f) windowSize = 45.0f;
                    info.ShowTopInfo("Smooth window size: " + windowSize.ToString("F0"));
                }
            }
        }

        posBuffer[currIdx] = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        currIdx += 1;
        if (currIdx > 45) currIdx = 0;

        if (smooth)
        {
            int startIdx = currIdx - (int)windowSize;
            if (startIdx < 0) startIdx += 46;

            Vector3 sumPos = Vector3.zero;
            while(startIdx != currIdx)
            {
                sumPos += posBuffer[startIdx];
                startIdx += 1;
                if (startIdx > 45) startIdx = 0;
            }

            sphereTrans.position = sumPos / windowSize;
        }
    }
}
