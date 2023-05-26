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
        // 데미지 건들 수 있는 구간

        this.damage = damage ; // this : 해당 클래스의 변수로 접근, Bullet이 가진 데미지 / 지금 받고 있는 매개변수 데미
        this.per = per;
        // 근접의 per는 -1로고정, 총알은 변수 받아옴

        if (per>-1)  // -1 sword, 총알만 해당
        {
            rigid.velocity = dir* GameManager.instance.player.bulletSpeed ;     // dir == 1, bullt speed , 15 부분 변수화 필요
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
