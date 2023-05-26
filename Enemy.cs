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

void FixedUpdate()         // ���� �̵��� �׻� Update�� �ƴ� Fixed ���
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))       
            return; // ������� �ʳ���? �ؿ��� �����Ű�� ���� ������

        Vector2 dirVec = target.position - rigid.position;   // ���� = ��ġ ������ ����ȭ (Normalized) , ������ �������� ����, rigid.position ��ũ��Ʈ ������ ��ġ
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // �������� �������� ����� �޶����� �ʰ�, ������ ������ ���� ��ġ 
        rigid.MovePosition(rigid.position + nextVec); //������ ��ġ + ���� ������ ��ġ
        rigid.velocity = Vector2.zero; // ������ �ӵ��� ����
    }

     void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x; // �ڽ��� ��ġ�� �÷��̾� x��ġ���� ���� ������ True�ؼ� ������ ������
    }

    void OnEnable()     // ��ũ��Ʈ�� Ȱ��ȭ �� �� ȣ���, Monster Recycle !! Init Value  ^^
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // Ÿ���� �ʱ�ȭ
        isLive = true; // Ȱ��ȭ �� �� lslive true
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data) // �ʱ� �Ӽ��� �ο��ϴ� �Լ�, Spawner ��ũ��Ʈ�� SpawnData
    {
        anim.runtimeAnimatorController = animCon[data.spriteType]; // �Ű� ������ �Ӽ��� ���� �Ӽ� ���濡 Ȱ���ϱ�
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

        if (Random.value <GameManager.instance.player.critChance) // Random.value�� 0���� 1 ������ ������ ���� ��ȯ�մϴ�.
        {
            damage = damage* critMultiplier;
            health -= damage;
            Debug.Log("Ŭ��Ƽ��" + damage);
        }
        else
        {
            
            health -= damage;
            Debug.Log("�⺻" + damage);
        }*/

       // health -= collision.GetComponent<Bullet>().damage; // ������ �˾Ƽ� ü�¿��� ������ ��
       

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
        gameObject.SetActive(false); // Ǯ���� ���� ���¶� ��Ȱ��ȭ�� ȿ���� ����
    }
}