using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;
    public int maximumRange;
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(gameObject.name == "Bullet 1(Clone)")
        {
            float Range = Vector2.Distance(transform.position, GameManager.instance.player.transform.position);
            if (Range > maximumRange)
            {
                rigid.velocity = Vector2.zero; // Acitve false previously velocity Init
                gameObject.SetActive(false);        // Object pooling 
            }
        }
      
    }
    public void Init(float damage, int per, Vector3 dir)        // Received variables from Weapon scripts
    {
        // ������ �ǵ� �� �ִ� ����

        this.damage = damage ; // this : �ش� Ŭ������ ������ ����, Bullet�� ���� ������ / ���� �ް� �ִ� �Ű����� ����
        this.per = per;
        // ������ per�� -1�ΰ���, �Ѿ��� ���� �޾ƿ�

        if (per>-1)  // -1 sword, �Ѿ˸� �ش�
        {
            rigid.velocity = dir* GameManager.instance.player.bulletSpeed ;     // dir == 1, bullt speed , 15 �κ� ����ȭ �ʿ�
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per ==-1 )                 //  || = or
            return;                                                      // Sword or Not enemy ? Return. Only Gun Weapon

        per--;          // 1 Monster Hit -1 per

        if(per == -1)
        {
            rigid.velocity = Vector2.zero; // Acitve false previously velocity Init
            gameObject.SetActive(false);        // Object pooling 
        }

       
    }

}
