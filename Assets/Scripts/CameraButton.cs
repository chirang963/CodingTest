using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraButton : MonoBehaviour
{
    public GameObject SelectCamera;
    public GameObject button;
    public void SelectSet() 
    {
        Debug.Log("SelectSet");
        SelectCamera.SetActive(true);

    }


}
