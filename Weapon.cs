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
        player = GetComponentInParent<Player>();       // �θ� ������Ʈ�� ������Ʈ  ����
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

   
    public void WeaponLevelUp(float damage, int count)        // ���������� �������� ���� �ø�
    {
        this.damage += damage;       // 10
        this.count += count;        // +1
       
        if (id == 0)    // �Ӽ� ����� ���ÿ� ���� ������ ��� ��ġ�� �ʿ��ϴ� Batch
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
        for (int index = 0; index < count; index++)   // ���� ���������� �ݺ�
        {
            Transform bullet;

            #region ������Ʈ Ǯ�������� �����Դ� ���� ���� ������ ��Ȱ���ϰ� ���ڶ��� Ǯ������ ����Ұ�

            if (index < transform.childCount) 
            {
                bullet = transform.GetChild(index); // ������ �ִٸ� Ǯ �Ŵ������� �������� �ʰ� ���� ���ڴ�
                //  bullet.parent = transform; // player- weapon �Ʒ��� ������Ʈ�� ���ܳ�, transform�� �ڱ� �ڽ� ,, �� ������ ���� ���
            }
            else // 2�� �ִµ� 2���� �Ѿ���?
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform; // ������ ������Ʈ�� Transform�� ���� ������ ����
                bullet.parent = transform; // player- weapon �Ʒ��� ������Ʈ�� ���ܳ�, transform�� �ڱ� �ڽ�
            }
            #endregion
           

            bullet.localPosition = Vector3.zero;        // ������ �ʱ�ȭ, �÷��̾� �����̸� ���� ������ ����
            bullet.localRotation = Quaternion.identity; // ȸ�� 000 �ʱ�ȭ

            // ȸ�� ���� �ο��ؼ� count �ݺ��� �� ���� �ٸ� ��ġ�� ���������� ������
            Vector3 rotVec = Vector3.forward * 360 * index / count;  // ī��Ʈ�� ���� 360�� ����.         
            bullet.Rotate(rotVec);                                   // ������ ȸ���� n ���� bullet�� Ʈ�������̶� ������Ʈ �ٷ� ���
            bullet.Translate(bullet.up * 1.5f, Space.World);         // �÷��̾�� 1.5f �̵�, �տ��� �۾��ؼ� Space World�� ����
                                                                     // �̻� ����                                             // ��ȯ

            bullet.GetComponent<Bullet>().Init(damage, -1,Vector3.zero);
                                      // here damage getting it in inspector and delivery to Bullet scripts
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)            // �÷��̾��� ��ĳ�� ��ũ��Ʈ �� ������ ����
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
