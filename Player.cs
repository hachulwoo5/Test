using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    [Header("# Player Stat")]
    public float baseDamage; // �⺻ ������
    public float critMultiplier; // ġ��Ÿ ���
    public float critChance; // ġ��Ÿ Ȯ��
    public float bulletSpeed;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();

    }

    
    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal"); // Axis -1 ~ 1 
        inputVec.y = Input.GetAxisRaw("Vertical");   // Raw 1�Ǵ� -1��, Raw ���� �� �̲������� ���� ����
    }
    

    void FixedUpdate() // ���� �̵��� Fixed �̿�
    {
        #region ���� �̵� Example
        /*
       // 1. �� �ش�
       rigid.AddForce(inputVec);

       // 2. �ӵ� ����
       rigid.velocity = inputVec;
       */
        #endregion

        // 3. ��ġ �̵�
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // ���� ������ �ϳ��� �Һ�� �ð� = fixedDeltaTime, ��ֶ����� �밢�� �� ��
        rigid.MovePosition(rigid.position + nextVec); // MovePosition�� ��ġ �̵��̶� ����(����) ��ġ�� ���������

    }

    /* InputSystem Asset
    void OnMove(InputValue value) // �Ű�����
    {
        inputVec = value.Get<Vector2>();
    }
    */

     void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude); // ������ ������ ũ�⸸ ������ ��

        if(inputVec.x !=0 )  // == �񱳿����� != 0�� �ƴ� ��, ���� �ٸ� �ǹ��� ������
        {
            spriter.flipX = inputVec.x < 0; // inputVec.x �� ������ ���� ������ true Ʋ���� false�� �ǹ�
        }
    }

    public float ApplyDamage(float Wepdamage,float armor, float miss)
    {
       float damage = Wepdamage * baseDamage * (1 - armor * 0.01f);
       if (Random.value <critChance) // Random.value�� 0���� 1 ������ ������ ���� ��ȯ�մϴ�.
        {
            damage = damage * critMultiplier;
            return damage;
        }
        else
        {
            return damage;
            
        }
        
    }
}
