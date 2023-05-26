using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    // 콜라이더 밖으로 나가면 아래 구문 실행

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))                //Area 태그가 아니면은 아래 것을 실행하지 않는다
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float dirX = playerPos.x - myPos.x;                         // 맵 이동용
        float dirY = playerPos.y - myPos.y;



        Vector3 playerDir = GameManager.instance.player.inputVec;   // 몬스터용

        float diffX = Mathf.Abs(dirX);     // 절댓값으로 양수만 추출 , X축의 거리
        float diffY = Mathf.Abs(dirY);

        
         dirX = dirX > 0 ? 1 : -1;              // 플레이어의 위치가 내 위치보다 우측이라면? 1, 오른쪽인지 왼쪽인지 방향 구별
         dirY = dirY > 0 ? 1 : -1;

        switch (transform.tag)
        {
            case "Ground":
                if(diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);  // 좌표이동이 아닌 이동할 양을 설정, dirX를 통해 왼 오 둘다 수렴 
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);  
                }
                break;

            case "Enemy": // 너무 멀리가면 플레이어 못봄, 시체는 필요 없음
                if(coll.enabled) // 죽으면 콜라이더를 off할건데 타변수 안가져오고 해결하기 위해 사용
                {
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f,3f),Random.Range(-3f, 3f),0f));                            // 플레이어의 이동 방향에 따라 맞은 편에서 등장하도록, 랜덤 위치
                }
                break;
           
        }
    }

}
