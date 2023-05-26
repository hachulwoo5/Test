using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �޸𸮿� �ø���
    public static GameManager instance;     // ���⿡ ���� public ��ü���� �ٸ����� ���ϰ� ������~ GameManger �� ����ؼ�~
    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")]
    public int health;
    public int maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp= {10, 30, 60 , 100, 150, 210, 280 , 360 ,450,600};

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;

    [Header("# Ememy Info")]
    public float power;



    private void Awake()
    {
        instance = this; // �ڱ� �ڽ��� ����
    }

    void Start()
    {
        health = maxHealth;
    }
    void Update()
    {
        gameTime += Time.deltaTime;       

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;         
        }
    }

    public void GetExp()  // Exp and Level Manage
    {
        exp++;

            if(exp == nextExp[level])       // nextExp[0] ?= 0 Lv, nextExp[1] ?=  1 Lv
        {
            
            level++;

            exp = 0;
        }

          
    }
}
