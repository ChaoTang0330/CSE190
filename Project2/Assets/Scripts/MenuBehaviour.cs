using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehaviour : MonoBehaviour
{
    public GameObject eyeDistanceObj, fieldofViewObj, spatialResObj;
    public GameObject pointingPrecObj, convergeDisObj, trackingPrecObj;

    private GameObject prevObj = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEyeDisBtnClick()
    {
        if (prevObj) prevObj.SetActive(false);
        prevObj = eyeDistanceObj;
        prevObj.SetActive(true);
    }

    public void OnFOVBtnClick()
    {
        if (prevObj) prevObj.SetActive(false);
        prevObj = fieldofViewObj;
        prevObj.SetActive(true);
    }

    public void OnSpaResBtnClick()
    {
        if (prevObj) prevObj.SetActive(false);
        prevObj = spatialResObj;
        prevObj.SetActive(true);
    }

    public void OnPointPrecBtnClick()
    {
        if (prevObj) prevObj.SetActive(false);
        prevObj = pointingPrecObj;
        prevObj.SetActive(true);
    }

    public void OnConvDisBtnClick()
    {
        if (prevObj) prevObj.SetActive(false);
        prevObj = convergeDisObj;
        prevObj.SetActive(true);
    }

    public void OnTrackPrecBtnClick()
    {
        if (prevObj) prevObj.SetActive(false);
        prevObj = trackingPrecObj;
        prevObj.SetActive(true);
    }
}
