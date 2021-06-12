using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public GameObject virusPrefab;
    public Transform virusesTrans;
    public LineRenderer laser;
    public TextMesh timeText;
    public TextMesh numText;
    public Transform rightController;
    public GameObject canvas;

    private GameObject[] viruses = new GameObject[25];
    private float timer, mutTimer;
    private int timeLeft;
    private int STOP = 0;
    private int START = 1;
    private int FINISH = 2;
    private int status;
    private int currMut;
    private int mutNum;

    // Start is called before the first frame update
    void Start()
    {
        float iniX = -0.5f, stepX = 0.56f;
        float iniY = 2.8f, stepY = -0.56f;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                viruses[i + 5 * j] = Instantiate(virusPrefab,
                    new Vector3(iniX + stepX * i, iniY + stepY * j, virusesTrans.localPosition.z),
                    Quaternion.identity, virusesTrans);
            }
        }

        timer = 0.0f;
        mutTimer = 0.0f;
        status = STOP;
        currMut = -1;

        timeLeft = 30;
        mutNum = 0;
        timeText.text = timeLeft.ToString();
        numText.text = mutNum.ToString();
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
        }
        else
        {
            laser.SetPosition(1, new Vector3(0.0f, 0.0f, 5.0f));
        }

        if (status == STOP)
        {
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                status = START;
                currMut = (int)Random.Range(0.0f, 24.99999f);

                //viruses[currMut].GetComponent<Outline>().enabled = true;
                viruses[currMut].GetComponent<VirusBehaviour>().SetMutant();

                //Debug.Log(string.Format("Time:{0}, CurrMut:{1}", timeLeft, currMut));
            }
        }
        else if (status == START)
        {
            if(currMut != -1)
            {
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    if (hitInfo.collider.gameObject == viruses[currMut])
                    {
                        mutNum += 1;
                        numText.text = mutNum.ToString();
                        viruses[currMut].GetComponent<VirusBehaviour>().SetNormal();
                        currMut = -1;

                        mutTimer = 1.0f;
                    }
                } 
            }

            if (timer >= 1.0f)
            {
                timer -= 1.0f;
                timeLeft -= 1;
                timeText.text = timeLeft.ToString();

                if (timeLeft <= 0)
                {
                    status = FINISH;
                    canvas.SetActive(true);
                }
            }

            if (mutTimer >= 1.0f)
            {
                mutTimer -= 1.0f;

                if (currMut != -1)
                {
                    //viruses[currMut].GetComponent<Outline>().enabled = false;
                    viruses[currMut].GetComponent<VirusBehaviour>().SetNormal();
                    currMut = -1;
                }

               currMut = (int)Random.Range(0.0f, 24.99999f);
               //viruses[currMut].GetComponent<Outline>().enabled = true;
               viruses[currMut].GetComponent<VirusBehaviour>().SetMutant();

               //Debug.Log(string.Format("Time:{0}, CurrMut:{1}", timeLeft, currMut));
            }
        }
        else if(status == FINISH)
        {
            if(timer > 5.0f)
            {
                status = STOP;
                timeLeft = 30;
                timer = 0.0f;
                mutTimer = 0.0f;
                mutNum = 0;
                timeText.text = timeLeft.ToString();
                numText.text = mutNum.ToString();
                canvas.SetActive(false);

                if (currMut != -1)
                {
                    viruses[currMut].GetComponent<VirusBehaviour>().SetNormal();
                    currMut = -1;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(status == START || status == FINISH)
        {
            timer += Time.fixedDeltaTime;
            mutTimer += Time.fixedDeltaTime;
        }
    }
}
