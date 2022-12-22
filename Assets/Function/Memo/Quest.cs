using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public static bool QuestActivated = false;  // 인벤토리 활성화 여부. true가 되면 카메라 움직임과 다른 입력을 막을 것이다.

    [SerializeField]
    private GameObject go_QuestBase; // Inventory_Base 이미지
    

     private Texts[] texts;  // 슬롯들 배열

    void Start()
    {
        
    }

    void Update()
    {
        TryOpenQuest();
    }

    private void TryOpenQuest()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
           QuestActivated = !QuestActivated;

            if (QuestActivated)
                OpenQuest();
            else
                CloseQuest();

        }
    }

    private void OpenQuest()
    {
        go_QuestBase.SetActive(true);
    }

    private void CloseQuest()
    {
        go_QuestBase.SetActive(false);
    }
     

    
}