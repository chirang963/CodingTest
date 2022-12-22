using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카메라 움직임(플레이어를 따라다니는 메인카메라) 스크립트
public class MoveCamera : MonoBehaviour
{
    public Transform CenterEyeAnchor; // 플레이어 내 cameraPos오브젝트의 위치
    
    // 매 프레임마다 갱신
    void Update()
    {
        // 이 스크립트가 포함된 오브젝트의 위치 = cameraPos의 위치
        transform.position = CenterEyeAnchor.position;
    }
}
