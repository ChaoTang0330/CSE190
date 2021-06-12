using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManagement : MonoBehaviour
{
    public Transform leftEyeTrans, rightEyeTrans;
    public Transform CAVELeftTrans, CAVERightTrans, CAVEFloorTrans;
    public GameObject DebugLinesL, DebugLinesR;
    public LineRenderer[] leftEyelines = new LineRenderer[7];
    public LineRenderer[] rightEyelines = new LineRenderer[7];
    public Transform[] headAnchor = new Transform[2];
    public Transform[] handAnchor = new Transform[2];
    public OffscreenRendering CAVELeftEye, CAVERightEye;
    public InfoText info;

    private bool HHMode = false;
    private bool FreezeMode = false;
    private bool BPressed = false;
    private bool DebugMode = false;
    private Vector3[] cornerPos = new Vector3[7];

    // Start is called before the first frame update
    void Start()
    {
        cornerPos[0] = CAVELeftTrans.TransformPoint(new Vector3(5f, 0, -5f));
        cornerPos[1] = CAVELeftTrans.TransformPoint(new Vector3(-5f, 0, -5f));
        cornerPos[2] = CAVERightTrans.TransformPoint(new Vector3(-5f, 0, -5f));
        cornerPos[3] = CAVELeftTrans.TransformPoint(new Vector3(5f, 0, 5f));
        cornerPos[4] = CAVELeftTrans.TransformPoint(new Vector3(-5f, 0, 5f));
        cornerPos[5] = CAVERightTrans.TransformPoint(new Vector3(-5f, 0, 5f));
        cornerPos[6] = CAVEFloorTrans.TransformPoint(new Vector3(5f, 0, 5f));

        DebugLinesL.SetActive(false);
        DebugLinesR.SetActive(false);

        //left
        for (int i = 0; i < 7; i++)
        {
            leftEyelines[i].useWorldSpace = true;
            leftEyelines[i].SetPosition(1, cornerPos[i]);
        }

        //right
        for (int i = 0; i < 7; i++)
        {
            rightEyelines[i].useWorldSpace = true;
            rightEyelines[i].SetPosition(1, cornerPos[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            if(!HHMode)
            {
                HHMode = true;
                leftEyeTrans.SetParent(handAnchor[0]);
                rightEyeTrans.SetParent(handAnchor[1]);

                leftEyeTrans.localPosition = Vector3.zero;
                leftEyeTrans.localRotation = Quaternion.identity;
                rightEyeTrans.localPosition = Vector3.zero;
                rightEyeTrans.localRotation = Quaternion.identity;

                info.ShowTopInfo("Head-in-hand Mode On");
            }
            else
            {
                if(OVRInput.Get(OVRInput.Button.One))
                {
                    if(!DebugMode)
                    {
                        DebugMode = true;
                        DebugLinesL.SetActive(true);
                        DebugLinesR.SetActive(true);

                        info.ShowTopInfo("Debug Mode On");
                    }
                    else
                    {
                        //left
                        for(int i = 0; i < 7; i++)
                        {
                            //leftEyelines[i].SetPosition(1, headAnchor[0].InverseTransformPoint(cornerPos[i]));
                            leftEyelines[i].SetPosition(0, headAnchor[0].position);
                        }

                        //right
                        for(int i = 0; i < 7; i++)
                        {
                            //rightEyelines[i].SetPosition(1, headAnchor[1].InverseTransformPoint(cornerPos[i]));
                            rightEyelines[i].SetPosition(0, headAnchor[1].position);
                        }
                    }
                }
                else
                {
                    if(DebugMode)
                    {
                        DebugMode = false;
                        DebugLinesL.SetActive(false);
                        DebugLinesR.SetActive(false);

                        info.ShowTopInfo("Debug Mode Off");
                    }
                }
            }
        }
        else
        {
            if (HHMode)
            {
                HHMode = false;
                leftEyeTrans.SetParent(headAnchor[0]);
                rightEyeTrans.SetParent(headAnchor[1]);
                
                leftEyeTrans.localPosition = Vector3.zero;
                leftEyeTrans.localRotation = Quaternion.identity;
                rightEyeTrans.localPosition = Vector3.zero;
                rightEyeTrans.localRotation = Quaternion.identity;

                info.ShowTopInfo("Head-in-hand Mode Off");
            }

            if (DebugMode)
            {
                DebugMode = false;
                DebugLinesL.SetActive(false);
                DebugLinesR.SetActive(false);
            }
        }
        
        
        if(OVRInput.Get(OVRInput.Button.Two))
        {
            if(!BPressed)
            {
                BPressed = true;

                if(FreezeMode)
                {
                    FreezeMode = false;
                    CAVELeftEye.ResumeCapturing();
                    CAVERightEye.ResumeCapturing();

                    info.ShowTopInfo("Freeze Mode Off");
                }
                else
                {
                    FreezeMode = true;
                    CAVELeftEye.StopCapturing();
                    CAVERightEye.StopCapturing();

                    info.ShowTopInfo("Freeze Mode On");
                }
            }
        }
        else
        {
            BPressed = false;
        }
    }
}
