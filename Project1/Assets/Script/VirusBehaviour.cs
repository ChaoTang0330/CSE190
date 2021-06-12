using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBehaviour : MonoBehaviour
{
    public MeshRenderer mr;
    private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = mr.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMutant()
    {
        mat.SetColor("_Color", Color.blue);
        //Debug.Log("SetMutant");
    }

    public void SetNormal()
    {
        mat.SetColor("_Color", Color.red);
    }
}
