using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorBehaviour : MonoBehaviour
{
    public LineRenderer laser;
    public Transform rightController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // cast ray
        RaycastHit hitInfo;
        bool hitFlag;
        Ray stareAt = new Ray(rightController.position, rightController.forward);
        hitFlag = Physics.Raycast(stareAt, out hitInfo, 100f);

        if (hitFlag)
        {
            laser.SetPosition(1, rightController.InverseTransformPoint(hitInfo.point));

            if(hitInfo.collider.gameObject.tag == "MenuButton")
            {
                if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    Button btn = hitInfo.collider.gameObject.GetComponent<Button>();
                    btn.onClick.Invoke();
                }
            }
        }
        else
        {
            laser.SetPosition(1, new Vector3(0.0f, 0.0f, 5.0f));
        }
    }
}
