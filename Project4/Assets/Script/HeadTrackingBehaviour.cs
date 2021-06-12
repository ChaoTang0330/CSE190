using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Node = UnityEngine.XR.XRNode;

struct CenterStatus
{
    public Vector3 pos;
    public Quaternion rot;
}

public class HeadTrackingBehaviour : MonoBehaviour
{
    public Transform leftEyeAnchor;
    public Transform rightEyeAnchor;
    public Transform centerEyeAnchor;

    private InfoText info;


    private int mode = 0;
    //private CenterStatus[] centerStatusBuffer = new CenterStatus[31];
    private int delay = 0;
    //private int currIdx = 0;

    //private bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<InfoText>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (OVRInput.Get(OVRInput.Button.Two)) // button B, head tracking mode
        //{
        //    if(!pressed)
        //    {
        //        pressed = true;

        //        mode += 1;
        //        mode %= 4;

        //        switch (mode)
        //        {
        //            case 0: info.ShowTopInfo("Head tracking mode: regular"); break;
        //            case 1: info.ShowTopInfo("Head tracking mode: orientation"); break;
        //            case 2: info.ShowTopInfo("Head tracking mode: position"); break;
        //            case 3: info.ShowTopInfo("Head tracking mode: stop"); break;
        //            default: break;
        //        }
        //    }
        //}
        //else
        //{
        //    pressed = false;
        //}

        //centerStatusBuffer[currIdx].pos = Vector3.zero;
        //Vector3 centerEyePosition = Vector3.zero;
        //if (OVRNodeStateProperties.GetNodeStatePropertyVector3(Node.CenterEye, NodeStatePropertyType.Position, OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render, out centerEyePosition))
        //    centerStatusBuffer[currIdx].pos = centerEyePosition;

        //centerStatusBuffer[currIdx].rot = Quaternion.identity;
        //Quaternion centerEyeRotation = Quaternion.identity;
        //if (OVRNodeStateProperties.GetNodeStatePropertyQuaternion(Node.CenterEye, NodeStatePropertyType.Orientation, OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render, out centerEyeRotation))
        //    centerStatusBuffer[currIdx].rot = centerEyeRotation;

        //int delayIdx = currIdx - delay;
        //if (delayIdx < 0) delayIdx += 31;

        //currIdx += 1;
        //if (currIdx > 30) currIdx = 0;

        //if (mode == 0 || mode == 2)
        //{
        //    Vector3 leftEyePosition = Vector3.zero;
        //    if (OVRNodeStateProperties.GetNodeStatePropertyVector3(Node.LeftEye, NodeStatePropertyType.Position, OVRPlugin.Node.EyeLeft, OVRPlugin.Step.Render, out leftEyePosition))
        //        leftEyeAnchor.localPosition = leftEyePosition;

        //    Vector3 rightEyePosition = Vector3.zero;
        //    if (OVRNodeStateProperties.GetNodeStatePropertyVector3(Node.RightEye, NodeStatePropertyType.Position, OVRPlugin.Node.EyeRight, OVRPlugin.Step.Render, out rightEyePosition))
        //        rightEyeAnchor.localPosition = rightEyePosition;


        //    centerEyeAnchor.localPosition = centerStatusBuffer[delayIdx].pos;
        //}

        //if (mode == 0 || mode == 1)
        //{
        //    Quaternion leftEyeRotation = Quaternion.identity;
        //    if (OVRNodeStateProperties.GetNodeStatePropertyQuaternion(Node.LeftEye, NodeStatePropertyType.Orientation, OVRPlugin.Node.EyeLeft, OVRPlugin.Step.Render, out leftEyeRotation))
        //        leftEyeAnchor.localRotation = leftEyeRotation;

        //    Quaternion rightEyeRotation = Quaternion.identity;
        //    if (OVRNodeStateProperties.GetNodeStatePropertyQuaternion(Node.RightEye, NodeStatePropertyType.Orientation, OVRPlugin.Node.EyeRight, OVRPlugin.Step.Render, out rightEyeRotation))
        //        rightEyeAnchor.localRotation = rightEyeRotation;

        //    centerEyeAnchor.localRotation = centerStatusBuffer[delayIdx].rot;
        //}

        Vector3 leftEyePosition = Vector3.zero;
        if (OVRNodeStateProperties.GetNodeStatePropertyVector3(Node.LeftEye, NodeStatePropertyType.Position, OVRPlugin.Node.EyeLeft, OVRPlugin.Step.Render, out leftEyePosition))
            leftEyeAnchor.localPosition = leftEyePosition;

        Vector3 rightEyePosition = Vector3.zero;
        if (OVRNodeStateProperties.GetNodeStatePropertyVector3(Node.RightEye, NodeStatePropertyType.Position, OVRPlugin.Node.EyeRight, OVRPlugin.Step.Render, out rightEyePosition))
            rightEyeAnchor.localPosition = rightEyePosition;

        Quaternion leftEyeRotation = Quaternion.identity;
        if (OVRNodeStateProperties.GetNodeStatePropertyQuaternion(Node.LeftEye, NodeStatePropertyType.Orientation, OVRPlugin.Node.EyeLeft, OVRPlugin.Step.Render, out leftEyeRotation))
            leftEyeAnchor.localRotation = leftEyeRotation;

        Quaternion rightEyeRotation = Quaternion.identity;
        if (OVRNodeStateProperties.GetNodeStatePropertyQuaternion(Node.RightEye, NodeStatePropertyType.Orientation, OVRPlugin.Node.EyeRight, OVRPlugin.Step.Render, out rightEyeRotation))
            rightEyeAnchor.localRotation = rightEyeRotation;

        Vector3 centerEyePosition = Vector3.zero;
        if (OVRNodeStateProperties.GetNodeStatePropertyVector3(Node.CenterEye, NodeStatePropertyType.Position, OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render, out centerEyePosition))
            centerEyeAnchor.localPosition = centerEyePosition;

        Quaternion centerEyeRotation = Quaternion.identity;
        if (OVRNodeStateProperties.GetNodeStatePropertyQuaternion(Node.CenterEye, NodeStatePropertyType.Orientation, OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render, out centerEyeRotation))
            centerEyeAnchor.localRotation = centerEyeRotation;
    }

    public int GetTrackingMode()
    {
        return mode;
    }

    public void SetDelay(int num)
    {
        delay = num;
    }
}
