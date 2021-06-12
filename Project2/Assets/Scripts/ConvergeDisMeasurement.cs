using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConvergeDisMeasurement : MonoBehaviour
{
    public TextMeshPro disText;
    public Transform sphere;
    public Text convResText;

    private float moveSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.One))
        {
            convResText.text = disText.text;
        }

        Vector2 stickVal = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if (Mathf.Abs(stickVal.y) > 1e-5)
        {
            float movement = Time.deltaTime * moveSpeed * stickVal.y;
            sphere.Translate(Vector3.forward * movement);

            disText.text = sphere.localPosition.z.ToString("F3") + " m";
        }
    }
}
