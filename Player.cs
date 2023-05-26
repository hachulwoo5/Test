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
    public float baseDamage; // 기본 데미지
    public float critMultiplier; // 치명타 배수
    public float critChance; // 치명타 확률
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
        inputVec.y = Input.GetAxisRaw("Vertical");   // Raw 1또는 -1만, Raw 없을 시 미끄러지게 구현 가능
    }
    

    void FixedUpdate() // 물리 이동은 Fixed 이용
    {
        #region 물리 이동 Example
        /*
       // 1. 힘 준다
       rigid.AddForce(inputVec);

       // 2. 속도 제어
       rigid.velocity = inputVec;
       */
        #endregion

        // 3. 위치 이동
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // 물리 프레임 하나가 소비된 시간 = fixedDeltaTime, 노멀라이즈 대각선 값 보
        rigid.MovePosition(rigid.position + nextVec); // MovePosition은 위치 이동이라 현재(물리) 위치도 더해줘야함

    }

    /* InputSystem Asset
    void OnMove(InputValue value) // 매개변수
    {
        inputVec = value.Get<Vector2>();
    }
    */

     void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude); // 벡터의 순수한 크기만 있으면 됨

        if(inputVec.x !=0 )  // == 비교연산자 != 0이 아닐 때, 서로 다른 의미의 연산자
        {
            spriter.flipX = inputVec.x < 0; // inputVec.x 를 측정해 식이 맞으면 true 틀리면 false를 의미
        }
    }

    public float ApplyDamage(float Wepdamage,float armor, float miss)
    {
       float damage = Wepdamage * baseDamage * (1 - armor * 0.01f);
       if (Random.value <critChance) // Random.value는 0에서 1 사이의 랜덤한 값을 반환합니다.
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
