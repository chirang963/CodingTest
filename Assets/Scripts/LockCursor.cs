using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursor : MonoBehaviour
{
    [SerializeField] Texture2D cursorImg;   // 커서 이미지

    void Awake() 
    {
        Cursor.lockState = CursorLockMode.Locked;   // 커서를 중앙으로 고정
        Cursor.visible = true;  // 커서를 보이게 설정
        Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.ForceSoftware); // 커서 설정
    }

    void Update()
    {   
        Cursor.visible = true;  // 커서를 보이게 설정
    }
    
}
