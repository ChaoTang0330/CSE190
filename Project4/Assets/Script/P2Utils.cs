using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Node = UnityEngine.XR.XRNode;

public class P2Utils : MonoBehaviour
{
    public enum RenderingMode { Stereo, Mono, LeftOnly, RightOnly, Inverted };
    public Camera leftEye;
    public Camera rightEye;
    public GameObject leftParent;
    public GameObject rightParent;
    public static P2Utils instance;
    
    private InfoText info;

    private RenderingMode renderingMode;
    private Vector3 leftPosStart;
    private Vector3 rightPosStart;
    private float iod;

    private HeadTrackingBehaviour headTracking;

    private class CamMat
    {
        public Matrix4x4 lv = Matrix4x4.identity;
        public Matrix4x4 lp = Matrix4x4.identity;
        public Matrix4x4 rv = Matrix4x4.identity;
        public Matrix4x4 rp = Matrix4x4.identity;
    }

    // Start is called before the first frame update
    void Start()
    {
        renderingMode = RenderingMode.Stereo;
        leftPosStart = leftParent.transform.position;
        rightPosStart = rightParent.transform.position;
        iod = 0.065f;
        setIODDistance(iod);
        if (instance == null)
            instance = this;

        headTracking = GetComponent<HeadTrackingBehaviour>();
        info = GetComponent<InfoText>();
    }

    // Update is called once per frame
    void Update()
    {
        if (renderingMode == RenderingMode.Mono)
        {
            var lv = leftEye.GetStereoViewMatrix(Camera.StereoscopicEye.Left);
            var lp = leftEye.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
            rightEye.SetStereoViewMatrix(Camera.StereoscopicEye.Right, lv);
            rightEye.SetStereoProjectionMatrix(Camera.StereoscopicEye.Right, lp);
        }
        else if (renderingMode == RenderingMode.Inverted)
        {
            rightEye.ResetStereoViewMatrices();
            rightEye.ResetStereoProjectionMatrices();
            leftEye.ResetStereoViewMatrices();
            leftEye.ResetStereoProjectionMatrices();

            var lv = leftEye.GetStereoViewMatrix(Camera.StereoscopicEye.Left);
            var lp = leftEye.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
            var rv = rightEye.GetStereoViewMatrix(Camera.StereoscopicEye.Right);
            var rp = rightEye.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
            leftEye.SetStereoViewMatrix(Camera.StereoscopicEye.Left, rv);
            leftEye.SetStereoProjectionMatrix(Camera.StereoscopicEye.Left, rp);
            rightEye.SetStereoViewMatrix(Camera.StereoscopicEye.Right, lv);
            rightEye.SetStereoProjectionMatrix(Camera.StereoscopicEye.Right, lp);
        }
    }

    public void changeRenderingMode(P2Utils.RenderingMode mode)
    {
        renderingMode = mode;

        rightEye.ResetStereoViewMatrices();
        rightEye.ResetStereoProjectionMatrices();
        leftEye.ResetStereoViewMatrices();
        leftEye.ResetStereoProjectionMatrices();

        rightEye.cullingMask = -1;
        leftEye.cullingMask = -1;
        switch (renderingMode)
        {
            default:
            case RenderingMode.Stereo:
                Shader.SetGlobalInt("_RenderingMode", 0);
                break;
            case RenderingMode.Mono:
                Shader.SetGlobalInt("_RenderingMode", 1);
                break;
            case RenderingMode.LeftOnly:
                Shader.SetGlobalInt("_RenderingMode", 2);
                rightEye.cullingMask = 0;
                break;
            case RenderingMode.RightOnly:
                Shader.SetGlobalInt("_RenderingMode", 3);
                leftEye.cullingMask = 0;
                break;
            case RenderingMode.Inverted:
                Shader.SetGlobalInt("_RenderingMode", 4);
                break;
        }
        
    }

    public void disableTracking(bool enabled)
    {
        UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking(leftEye, enabled);
        UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking(rightEye, enabled);
    }

    public void setIODDistance(float distance)
    {
        iod = distance;
        //if (iod == 0.065f)
        //{
        //    resetEyeParents();
        //}

        var direction = new Vector3(-1.0f, 0.0f, 0.0f);
        leftParent.transform.localPosition = leftPosStart + direction * (iod - 0.065f) * 17.5f;
        rightParent.transform.localPosition = rightPosStart - direction * (iod - 0.065f) * 17.5f;
    }

    void resetEyeParents()
    {
        leftParent.transform.localPosition = leftPosStart;
        rightParent.transform.localPosition = rightPosStart;
    }
}
