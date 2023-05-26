using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    public int weaponlevel;
    float timer;
    Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();       // 부모 오브젝트의 컴포넌트  접근
    }
    void Start()
    {
        Init();
    }
    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);

                break;
        
            default:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;         // shot speed
                    Fire();
                }
                break;

        }

        // .. Test Code..

        if (Input.GetButtonDown("Jump")) 
        {
            WeaponLevelUp(10, 1);
            GameManager.instance.power *= 1.1f;
        }

        if (weaponlevel != GameManager.instance.level)
        {
            weaponlevel = GameManager.instance.level;
            /*if (id == 0)
                WeaponLevelUp(10, 1);

            if (id == 1)
                WeaponLevelUp(3, 1);
            */
          
           
        }

    }

   
    public void WeaponLevelUp(float damage, int count)        // 레벨업마다 데미지와 갯수 늘림
    {
        this.damage += damage;       // 10
        this.count += count;        // +1
       
        if (id == 0)    // 속성 변경과 동시에 근접 무기의 경우 배치가 필요하니 Batch
              Batch();
        
        
    }
    public void Init()          // Start game, This figure designation
    {
        switch (id)
        {
            case 0:
                speed = 150;
                Batch();
                break;
            
            default:
                speed = 0.3f; // rapid shot speed
                break;

        }
    }

    void Batch()        // Sword Function
    {
        for (int index = 0; index < count; index++)   // 무기 개수에따라 반복
        {
            Transform bullet;

            #region 오브젝트 풀링에서만 가져왔던 것을 내가 가진걸 재활용하고 모자란걸 풀링으로 충당할게

            if (index < transform.childCount) 
            {
                bullet = transform.GetChild(index); // 가지고 있다면 풀 매니저에서 꺼내오지 않고 내꺼 쓰겠다
                //  bullet.parent = transform; // player- weapon 아래로 오브젝트가 생겨남, transform은 자기 자신 ,, 위 구문이 동일 기능
            }
            else // 2개 있는데 2개가 넘었다?
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform; // 가져온 오브젝트의 Transform을 지역 변수로 저장
                bullet.parent = transform; // player- weapon 아래로 오브젝트가 생겨남, transform은 자기 자신
            }
            #endregion
           

            bullet.localPosition = Vector3.zero;        // 포지션 초기화, 플레이어 움직이면 값이 수정됨 방지
            bullet.localRotation = Quaternion.identity; // 회전 000 초기화

            // 회전 값을 부여해서 count 반복될 때 마다 다른 위치에 성공적으로 생성함
            Vector3 rotVec = Vector3.forward * 360 * index / count;  // 카운트에 따라 360을 나눔.         
            bullet.Rotate(rotVec);                                   // 무기의 회전값 n 넣음 bullet은 트랜스폼이라 로테이트 바로 사용
            bullet.Translate(bullet.up * 1.5f, Space.World);         // 플레이어에서 1.5f 이동, 앞에서 작업해서 Space World로 가능
                                                                     // 이상 동문                                             // 소환

            bullet.GetComponent<Bullet>().Init(damage, -1,Vector3.zero);
                                      // here damage getting it in inspector and delivery to Bullet scripts
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)            // 플레이어의 스캐너 스크립트 속 변수에 접근
            return; // filter

        Vector3 targetPos = player.scanner.nearestTarget.position;      // Location
        Vector3 dir = targetPos - transform.position;                   // Direction
        dir = dir.normalized;                                           // Direction Maintenance, Size = 1

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;   // Create Bullet in pool
        bullet.position = transform.position;                                   // player position's shot
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);           // around the specified axis , toward target rotate
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
        bullet.parent = GameManager.instance.pool.transform.GetChild(1);

    }                             // here damage and count getting it in inspector and delivery to Bullet scripts
}
