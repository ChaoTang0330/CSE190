using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpatialResMeasurement : MonoBehaviour
{
    public GameObject horiLines, vertLines;
    public TextMeshPro disText, resText;
    public GameObject horiResObj, vertResObj;
    public Text horiResText, vertResText;

    private GameObject currPattern;
    private float moveSpeed = 1.0f;
    private float lineInterval = 0.04f;
    private float currRes = 0.87f;
    private float horiRes = 0.87f, vertRes = 0.87f;

    // Start is called before the first frame update
    void Start()
    {
        currPattern = horiLines;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 stickVal = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if (Mathf.Abs(stickVal.y) > 1e-5)
        {
            float movement = Time.deltaTime * moveSpeed * stickVal.y;
            currPattern.transform.Translate(Vector3.forward * movement);

            UpdateRes();
        }
        else if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SwitchPattern();
        }
    }

    void SwitchPattern()
    {
        if (currPattern == horiLines)
        {
            horiLines.SetActive(false);
            vertLines.SetActive(true);
            currPattern = vertLines;
            horiRes = currRes;

            horiResObj.SetActive(true);
            horiResText.text = horiRes.ToString("F3");
        }
        else
        {
            vertLines.SetActive(false);
            horiLines.SetActive(true);
            currPattern = horiLines;
            vertRes = currRes;
            
            vertResObj.SetActive(true);
            vertResText.text = vertRes.ToString("F3");
        }

        UpdateRes();
    }

    void UpdateRes()
    {
        currRes = currPattern.transform.localPosition.z * Mathf.Tan(Mathf.PI / 180.0f) / lineInterval;
        disText.text = currPattern.transform.localPosition.z.ToString("F2");
        resText.text = currRes.ToString("F2");
    }
}
