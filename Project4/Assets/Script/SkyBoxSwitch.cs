using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxSwitch : MonoBehaviour
{
    public Material[] skyboxMat = new Material[2];

    private InfoText info;
    private int currIdx = 0;
    private bool pressed = false;
    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<InfoText>();
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Button.Four))
        {
            if(!pressed)
            {
                pressed = true;

                currIdx += 1;
                currIdx %= 2;
                RenderSettings.skybox = skyboxMat[currIdx];

                info.ShowTopInfo("Skybox " + currIdx.ToString());
            }
        }
        else
        {
            pressed = false;
        }
    }
}
