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
    // �ݶ��̴� ������ ������ �Ʒ� ���� ����

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))                //Area �±װ� �ƴϸ��� �Ʒ� ���� �������� �ʴ´�
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float dirX = playerPos.x - myPos.x;                         // �� �̵���
        float dirY = playerPos.y - myPos.y;



        Vector3 playerDir = GameManager.instance.player.inputVec;   // ���Ϳ�

        float diffX = Mathf.Abs(dirX);     // �������� ����� ���� , X���� �Ÿ�
        float diffY = Mathf.Abs(dirY);

        
         dirX = dirX > 0 ? 1 : -1;              // �÷��̾��� ��ġ�� �� ��ġ���� �����̶��? 1, ���������� �������� ���� ����
         dirY = dirY > 0 ? 1 : -1;

        switch (transform.tag)
        {
            case "Ground":
                if(diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);  // ��ǥ�̵��� �ƴ� �̵��� ���� ����, dirX�� ���� �� �� �Ѵ� ���� 
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);  
                }
                break;

            case "Enemy": // �ʹ� �ָ����� �÷��̾� ����, ��ü�� �ʿ� ����
                if(coll.enabled) // ������ �ݶ��̴��� off�Ұǵ� Ÿ���� �Ȱ������� �ذ��ϱ� ���� ���
                {
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f,3f),Random.Range(-3f, 3f),0f));                            // �÷��̾��� �̵� ���⿡ ���� ���� ���� �����ϵ���, ���� ��ġ
                }
                break;
           
        }
    }

}
