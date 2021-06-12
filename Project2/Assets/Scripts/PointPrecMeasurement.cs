using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointPrecMeasurement : MonoBehaviour
{
    public TextMeshPro hitText, timeText, disText;
    public Transform sphere, rightController;
    public Text PointPrecResText;

    private int hitTimes = 0;
    private float timer = 0.0f;
    private bool isMeasuring = false;
    private float moveSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        disText.text = sphere.position.z.ToString("F2") + " m";
    }

    void OnEnable()
    {
        sphere.gameObject.SetActive(true);
        isMeasuring = false;
    }

    void OnDisable()
    {
        sphere.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isMeasuring)
        {
            timer -= Time.deltaTime;
            if(timer <= 0.0f)
            {
                isMeasuring = false;
                timer = 0.0f;
            }
            timeText.text = timer.ToString("F0");

            // cast ray
            RaycastHit hitInfo;
            bool hitFlag;
            Ray stareAt = new Ray(rightController.position, rightController.forward);
            hitFlag = Physics.Raycast(stareAt, out hitInfo);

            if (hitFlag && hitInfo.transform == sphere)
            {
                if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    hitTimes += 1;
                    hitText.text = hitTimes.ToString();
                }
            }
        }
        else
        {
            Vector2 stickVal = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            if (Mathf.Abs(stickVal.y) > 1e-5)
            {
                float movement = Time.deltaTime * moveSpeed * stickVal.y;
                sphere.Translate(Vector3.forward * movement);

                disText.text = sphere.localPosition.z.ToString("F2") + " m";
            }
            else if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                isMeasuring = true;
                hitTimes = 0;
                timer = 20.0f;
                timeText.text = timer.ToString("F0");
                hitText.text = hitTimes.ToString();
            }
            else if (OVRInput.Get(OVRInput.Button.One))
            {
                if(hitTimes <= 10)
                {
                    PointPrecResText.text = disText.text;
                }
            }
        }
    }
}
