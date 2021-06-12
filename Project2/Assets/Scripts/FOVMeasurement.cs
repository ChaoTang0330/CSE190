using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FOVMeasurement : MonoBehaviour
{
    public Transform TopLineTrans, BottomLineTrans;
    public Transform LeftLineTrans, RightLineTrans;
    public Text horiText, vertText, diagText;
    public Text horiResText, vertResText, diagResText;
    public GameObject horiResObj, vertResObj, diagResObj;

    private float moveSpeed = 1.0f;
    private float centerDis = 7.0f;

    private float hori = 0.0f, vert = 0.0f, diag = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        hori = 2 * Mathf.Atan2(RightLineTrans.localPosition.x - 0.1f, centerDis) * Mathf.Rad2Deg;
        vert = 2 * Mathf.Atan2(TopLineTrans.localPosition.y - 0.1f, centerDis) * Mathf.Rad2Deg;
        diag = 2 * Mathf.Atan2(
            Mathf.Sqrt(
                Mathf.Pow(RightLineTrans.localPosition.x - 0.1f, 2.0f)
                + Mathf.Pow(TopLineTrans.localPosition.y - 0.1f, 2.0f)),
            centerDis) * Mathf.Rad2Deg;

        //Debug.Log(string.Format("right {0}", hori));
        //Debug.Log(string.Format("left {0}", vert));
        horiText.text = hori.ToString("F2") + "°";
        vertText.text = vert.ToString("F2") + "°";
        diagText.text = diag.ToString("F2") + "°";
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 stickVal = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if (Mathf.Abs(stickVal.x) >= Mathf.Abs(stickVal.y) && Mathf.Abs(stickVal.x) > 1e-5)
        {
            float movement = Time.deltaTime * moveSpeed * stickVal.x;
            LeftLineTrans.Translate(Vector3.left * movement);
            RightLineTrans.Translate(Vector3.right * movement);

            Vector3 locScale = TopLineTrans.localScale;
            locScale.x = RightLineTrans.localPosition.x * 2.0f;
            TopLineTrans.localScale = locScale;
            BottomLineTrans.localScale = locScale;

            hori = 2 * Mathf.Atan2(RightLineTrans.localPosition.x - 0.1f, centerDis) * Mathf.Rad2Deg;
            diag = 2 * Mathf.Atan2(
                Mathf.Sqrt(
                    Mathf.Pow(RightLineTrans.localPosition.x - 0.1f, 2.0f)
                    + Mathf.Pow(TopLineTrans.localPosition.y - 0.1f, 2.0f)),
                centerDis) * Mathf.Rad2Deg;

            horiText.text = hori.ToString("F2") + "°";
            diagText.text = diag.ToString("F2") + "°";
        }
        else if (Mathf.Abs(stickVal.y) > 1e-5)
        {
            float movement = Time.deltaTime * moveSpeed * stickVal.y;
            TopLineTrans.Translate(Vector3.up * movement);
            BottomLineTrans.Translate(Vector3.down * movement);

            Vector3 locScale = RightLineTrans.localScale;
            locScale.y = TopLineTrans.localPosition.y * 2.0f;
            RightLineTrans.localScale = locScale;
            LeftLineTrans.localScale = locScale;

            vert = 2 * Mathf.Atan2(TopLineTrans.localPosition.y - 0.1f, centerDis) * Mathf.Rad2Deg;
            diag = 2 * Mathf.Atan2(
                Mathf.Sqrt(
                    Mathf.Pow(RightLineTrans.localPosition.x - 0.1f, 2.0f)
                    + Mathf.Pow(TopLineTrans.localPosition.y - 0.1f, 2.0f)),
                centerDis) * Mathf.Rad2Deg;
            vertText.text = vert.ToString("F2") + "°";
            diagText.text = diag.ToString("F2") + "°";
        }
        else if (OVRInput.Get(OVRInput.Button.One))
        {
            horiResObj.SetActive(true);
            vertResObj.SetActive(true);
            diagResObj.SetActive(true);

            horiResText.text = horiText.text;
            vertResText.text = vertText.text;
            diagResText.text = diagText.text;
        }
    }
}
