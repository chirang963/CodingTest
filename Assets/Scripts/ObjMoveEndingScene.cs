using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMoveEndingScene : MonoBehaviour
{
    public Rigidbody chair; // chair의 중력 변수

    public AudioSource objectAudio; // 오디오 소스 오브젝트오디오의 변수

    void Start() 
    {
        StartCoroutine(MoveObject()); // moveobject 코루틴 실행   
    }
    IEnumerator MoveObject() //moveobject 코루틴 메서드
    {
        chair = GetComponent<Rigidbody>(); // rigidbody 컴포넌트를 chair에 할당

        while(true) // true 동안에
        {
            float dir1 = Random.Range(-1f, 1f); // 방향1의 범위가 -1에서 1까지
            float dir2 = Random.Range(-1f, 1f); // 방향2의 범위가 -1에서 1까지

            yield return new WaitForSeconds(2); //2초간 대기
            chair.velocity = new Vector3(dir1, 3, dir2); // chair 속도는 x축은 dir1, y축은 3, z축은 dir2
        }
    }
}
