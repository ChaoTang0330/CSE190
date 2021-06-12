using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ViewManagement : MonoBehaviour
{
    public GameObject leftFrame, rightFrame;
    
    private InfoText info;

    private bool frameStatus = true;
    private int steroMode = 0;
    private float currIOD = 0.065f;

    private float delay = 0.0f;
    private float waitTime = 0.0f;

    private bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        OVRPlugin.systemDisplayFrequency = 90.0f;
        info = GetComponent<InfoText>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Three)) // button X, black frame
        {
            if(!pressed)
            {
                pressed = true;
                if (frameStatus)
                {
                    frameStatus = false;
                    info.ShowTopInfo("Full FOV");
                }
                else
                {
                    frameStatus = true;
                    info.ShowTopInfo("Half FOV");
                }

                leftFrame.SetActive(frameStatus);
                rightFrame.SetActive(frameStatus);
            }
        }
        else if (OVRInput.Get(OVRInput.Button.One)) // button A, stereo mode
        {
            if(!pressed)
            {
                pressed = true;

                steroMode += 1;
                steroMode %= 5;
                P2Utils.RenderingMode currMode = (P2Utils.RenderingMode)steroMode;
                P2Utils.instance.changeRenderingMode(currMode);
                info.ShowTopInfo("Current rendering mode: " + currMode.ToString());
            }
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            if(!pressed)
            {
                pressed = true;

                P2Utils.instance.setIODDistance(0.065f);
                currIOD = 0.065f;
                info.ShowTopInfo("IOD: " + currIOD.ToString("F3"));
            }
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            if (!pressed)
            {
                pressed = true;

                delay += 1.0f;
                if (delay > 10.0f) delay = 10.0f;
                //OVRPlugin.systemDisplayFrequency = 90.0f - delay;
                info.ShowTopInfo("Rendering delay: " + delay.ToString("F0") + " frames");
            }
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            if(!pressed)
            {
                pressed = true;

                delay -= 1.0f;
                if (delay < 0.0f) delay = 0.0f;
                //OVRPlugin.systemDisplayFrequency = 90.0f - delay;
                info.ShowTopInfo("Rendering delay: " + delay.ToString("F0") + " frames");
            }
        }
        else
        {
            float speedFactor = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
            if (Mathf.Abs(speedFactor) > 1e-4)
            {
                currIOD += speedFactor * 0.1f * Time.deltaTime;
                if (currIOD < -0.1f) currIOD = -0.1f;
                else if (currIOD > 0.3f) currIOD = 0.3f;
                P2Utils.instance.setIODDistance(currIOD);
                info.ShowTopInfo("IOD: " + currIOD.ToString("F3"));
            }

            pressed = false;
        }

        float offsetDelay = delay + 50.0f;
        waitTime += (offsetDelay / 110.0f) / (110.0f - offsetDelay);
        int waitTime_ms = (int) (waitTime * 1000);
        if(waitTime_ms > 0)
        {
            Thread.Sleep(waitTime_ms);
            waitTime -= 0.001f * waitTime_ms;
            //Debug.Log("Sleep " + waitTime_ms.ToString() + " ms");
        }
    }
}
