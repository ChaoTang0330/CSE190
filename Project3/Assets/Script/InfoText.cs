using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Node = UnityEngine.XR.XRNode;

public class InfoText : MonoBehaviour
{
    public Text topInfoL, bottomInfoL;
    public Text topInfoR, bottomInfoR;

    private float timer;
    private int frameCount = 0;
    private float frameTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        topInfoL.text = "";
        //bottomInfoL.text = OVRPlugin.GetAppFramerate().ToString("F0");
        bottomInfoL.text = "";
        topInfoR.text = "";
        //bottomInfoR.text = OVRPlugin.GetAppFramerate().ToString("F0");
        bottomInfoR.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0.0f)
        {
            timer -= Time.deltaTime;
            if(timer <= 0.0f)
            {
                topInfoL.text = "";
                topInfoR.text = "";
            }
        }

        //bottomInfoL.text = OVRPlugin.GetAppFramerate().ToString("F0");
        //bottomInfoR.text = OVRPlugin.GetAppFramerate().ToString("F0");
        //float currFPS = 1.0f / Time.deltaTime;
        //bottomInfoL.text = currFPS.ToString("F0");
        //bottomInfoR.text = currFPS.ToString("F0");

        frameCount += 1;
        frameTimer += Time.deltaTime;
        if(frameTimer > 1.0f)
        {
            frameTimer -= 1.0f;
            //bottomInfoL.text = "FPS: " + frameCount.ToString();
            //bottomInfoR.text = "FPS: " + frameCount.ToString();
            frameCount = 0;
        }
    }

    public void ShowTopInfo(string info)
    {
        timer = 2.0f;
        topInfoL.text = info;
        topInfoR.text = info;
    }
}
