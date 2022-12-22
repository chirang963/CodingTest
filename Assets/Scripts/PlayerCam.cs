using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 카메라 마우스 움직임
public class PlayerCam : MonoBehaviour
{

    public float sensX; // x(horizontal) 민감도
    public float sensY; // y(vertical) 민감도
   
    public Transform CenterEyeAnchor; // 플레이어 내 CenterEyeAnchor 오브젝트의 transform

    float xRotation; // x(horizontal)회전
    float yRotation; // y(vertical)회전

 
    private void Update() {
        // 마우스 입력 받기 - 민감도 만큼
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
  
        // 입력 받은 값만큼 회전값에 더함 및 뺌
        yRotation += mouseX;
        xRotation -= mouseY;
        // 최대 회전값 제한
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 캠 및 방향 회전
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        CenterEyeAnchor.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
