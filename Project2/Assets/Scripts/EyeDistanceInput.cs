using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EyeDistanceInput : MonoBehaviour
{
    public TextMeshPro[] digitsText = new TextMeshPro[4];
    public Text resultText;

    private int[] digits = new int[4];
    private int currDigIdx = 0;
    private float resultDis;
    private bool isPressed = false;
    // Start is called before the first frame update
    void Start()
    {
        digitsText[0].color = Color.white;
        digits[0] = 0;
        digits[1] = 0;
        digits[2] = 0;
        digits[3] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 stickVal = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if (Mathf.Abs(stickVal.x) >= Mathf.Abs(stickVal.y) && Mathf.Abs(stickVal.x) >= 0.7f)
        {
            if (isPressed) return;
            digitsText[currDigIdx].color = Color.black;
            if(stickVal.x > 0)
            {
                currDigIdx += 1;
                if (currDigIdx == 4) currDigIdx = 0;
            }
            else
            {
                currDigIdx -= 1;
                if (currDigIdx == -1) currDigIdx = 3;
            }
            isPressed = true;
            digitsText[currDigIdx].color = Color.white;
        }
        else if(Mathf.Abs(stickVal.y) >= 0.7f)
        {
            if (isPressed) return;
            if (stickVal.y > 0)
            {
                digits[currDigIdx] += 1;
                if (digits[currDigIdx] == 10) digits[currDigIdx] = 0;
            }
            else
            {
                digits[currDigIdx] -= 1;
                if (digits[currDigIdx] == -1) digits[currDigIdx] = 9;
            }
            digitsText[currDigIdx].text = digits[currDigIdx].ToString();
            isPressed = true;
        }
        else if(OVRInput.Get(OVRInput.Button.One))
        {
            if (isPressed) return;
            resultDis = 10.0f * digits[0] + digits[1] + 0.1f * digits[2] + 0.01f * digits[3];
            resultText.text = resultDis.ToString("F2") + " mm";
            isPressed = true;
        }
        else
        {
            isPressed = false;
        }
    }
}
