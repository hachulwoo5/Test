using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public EnemyData enemyData;
    public float speed;
    public float health;
    public float maxHealth;
    public float armor;

    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    bool isLive;

    float critChance;
    float critMultiplier;
    public float baseDamage;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait; // next fixed update, stand by

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();

        
    }

void FixedUpdate()         // 물리 이동은 항상 Update가 아닌 Fixed 사용
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))       
            return; // 살아있지 않나요? 밑에서 실행시키지 말고 나가요

        Vector2 dirVec = target.position - rigid.position;   // 방향 = 위치 차이의 정규화 (Normalized) , 벡터의 연산으로 구함, rigid.position 스크립트 주인의 위치
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // 프레임의 영향으로 결과가 달라지지 않게, 앞으로 가야할 다음 위치 
        rigid.MovePosition(rigid.position + nextVec); //지금의 위치 + 다음 가야할 위치
        rigid.velocity = Vector2.zero; // 물리적 속도를 없앰
    }

     void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x; // 자신의 위치와 플레이어 x위치값을 비교해 작으면 True해서 왼쪽을 보게함
    }

    void OnEnable()     // 스크립트가 활성화 될 때 호출됨, Monster Recycle !! Init Value  ^^
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // 타겟을 초기화
        isLive = true; // 활성화 될 때 lslive true
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data) // 초기 속성을 부여하는 함수, Spawner 스크립트의 SpawnData
    {
        anim.runtimeAnimatorController = animCon[data.spriteType]; // 매개 변수의 속성을 몬스터 속성 변경에 활용하기
        speed = data.speed;
        maxHealth = data.health* GameManager.instance.power;
        health = data.health* GameManager.instance.power;
        armor = data.armor;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)      // Death Logic Run continuously Prevent, alive only
            return; // filter


        float hitdamage = GameManager.instance.player.ApplyDamage(collision.GetComponent<Bullet>().damage, armor, 10f);
        health -= hitdamage;
        Debug.Log(hitdamage);
        /*
        float damage = collision.GetComponent<Bullet>().damage *baseDamage;

        if (Random.value <GameManager.instance.player.critChance) // Random.value는 0에서 1 사이의 랜덤한 값을 반환합니다.
        {
            damage = damage* critMultiplier;
            health -= damage;
            Debug.Log("클히티컬" + damage);
        }
        else
        {
            
            health -= damage;
            Debug.Log("기본" + damage);
        }*/

       // health -= collision.GetComponent<Bullet>().damage; // 맞으면 알아서 체력에서 데미지 뺌
       

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            StartCoroutine(KnockBack());        // monster hit, knockback
            // .. Live, Hit Action

        }
        else        // Die
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true) ;
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // next 1 Physics frame delay
        Vector3 playerPos = GameManager.instance.player.transform.position; // player position
        Vector3 dirVec = transform.position - playerPos; // opposite direction = Current Location - player Location
        rigid.AddForce(dirVec.normalized * 3 , ForceMode2D.Impulse); // normal == 1
    }

    void Dead()
    {
        gameObject.SetActive(false); // 풀링을 쓰는 상태라 비활성화로 효율적 관리
    }
}
