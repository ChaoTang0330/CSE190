using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrackingPrecMeasurement : MonoBehaviour
{
    public TextMeshPro posText, rotText, infoText;
    public GameObject posResObj, rotResObj;
    public Text posResText, rotResText;

    List<Vector3> positions = new List<Vector3>();
    List<Vector3> rotations = new List<Vector3>();

    private Vector3 avgPos, avgRot;
    private float varPos = 0.0f, varRot = 0.0f;
    private float resPos = 0.0f, resRot = 0.0f;
    private float timer = 0.0f;
    private bool isMeasure = false;
    // Start is called before the first frame update
    void Start()
    {
        avgPos = new Vector3(0.0f, 0.0f, 0.0f);
        avgRot = new Vector3(0.0f, 0.0f, 0.0f);
        infoText.text = "Press Index trigger to start";
    }

    // Update is called once per frame
    void Update()
    {
        if(isMeasure)
        {
            timer += Time.deltaTime;
            if(timer > 5.0f)
            {
                isMeasure = false;
                timer = 0.0f;
                UpdateResult();
                infoText.text = "Press Index trigger to start";
            }
            else
            {
                positions.Add(OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch));
                rotations.Add(OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch).eulerAngles);
            }
        }
        else
        {
            if (OVRInput.Get(OVRInput.Button.One))
            {
                //UpdateResult();
                posResObj.SetActive(true);
                rotResObj.SetActive(true);
                posResText.text = posText.text;
                rotResText.text = rotText.text;
            }
            else if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                isMeasure = true;
                timer = 0.0f;
                positions.Clear();
                rotations.Clear();
                infoText.text = "Measuring";
            }
        }
        
    }

    void UpdateResult()
    {
        avgPos = Vector3.zero;
        avgRot = Vector3.zero;
        varPos = 0.0f;
        varRot = 0.0f;

        if (positions.Count < 10) return;

        float sampleNum = (float) positions.Count;

        Debug.Log("Sample Number: " + positions.Count.ToString());

        for(int i = 0; i < positions.Count; i++)
        {
            avgPos += positions[i] / sampleNum;
            avgRot += rotations[i] / sampleNum;
        }

        for(int i = 0; i < positions.Count; i++)
        {
            varPos += (positions[i] - avgPos).sqrMagnitude / sampleNum;
            varRot += (rotations[i] - avgRot).sqrMagnitude / sampleNum;
        }

        resPos = Mathf.Sqrt(varPos);
        resRot = Mathf.Sqrt(varRot);

        posText.text = resPos.ToString("F4");
        rotText.text = resRot.ToString("F4");
    }
}
