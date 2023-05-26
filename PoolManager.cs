using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    // ,, ��������� ������ ����
    public GameObject[] prefabs; // �迭 ����

    // .. Ǯ ����� �ϴ� ����Ʈ��
    List<GameObject>[] pools; // ���� �迭��

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length]; // ������ �迭�� ũ��� �����ؾ���, ���� �迭�� �ʱ�ȭ �ϰ� ������ ����Ʈ �³״� �ʱ�ȭ�� �ȵ�
        
        // ����Ʈ���� �ʱ�ȭ
        for (int index =0; index<pools.Length;index++)
        {
            pools[index] = new List<GameObject>(); // ������ ����Ʈ�� ��ȸ�ϸ鼭 �ʱ�ȭ
        }

       
    }

    #region ���� ���� �ֿ� ����

    
    public GameObject Get(int index) // ���� ������Ʈ�� ��ȯ�ϴ� �Լ� , ������ ������Ʈ�� ������ �����ϴ� �Ű����� index �߰� ex Get(0)
    {
        GameObject select = null; // Get(0) = select�� 0�� ���͸� �ְ� return�ؼ� ��ȯ��

        // ... ������ Ǯ�� ��Ȱ��ȭ�� ���ӿ�����Ʈ ����
        foreach(GameObject item in pools[index])        // �迭 ����Ʈ���� �����͸� ���������� �����ϴ� �ݺ���, ���⼭ ������ ���� ���� ��������(Gameobject item)�� ��������
        {
            if(!item.activeSelf)        // ��Ȱ��ȭ �� �� ã��
            {
                // ... �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // ... �� ���� �ִٸ�?
      
        if (!select)
        {
            // ... ���Ӱ� �����ϰ� select ������ �Ҵ�
            select = Instantiate(prefabs[index], transform); // transform == ���ڽ� poolManager�� ����, select��  prefabs���� �ϳ� ������ ���� ����
            pools[index].Add(select);                                                               // ���õ� select�� pools �迭�� ���� ����
        }

        return select;
    }
    #endregion
}
