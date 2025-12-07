using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using static AllControl;

public class EnemyAI : MonoBehaviour
{
    int CD;//因为敌人和玩家攻击的形式不一样（一个用鼠标触发，一个指令）所以CD也只能在这里多做一份
    public List<Collider2D> CollList;
    public Collider2D coll;
    private Rigidbody2D rb;
    private Attributes Attributes;
    private Collider2D Fruit;
    private Collider2D Player;//视野中其他的player
    //脚本引用
    public AttackFunction AttackFunction;

    private int WalkTime = 0;
    private int state = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        Attributes = GetComponent<Attributes>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CD > 0) CD--;//设置攻击间隔
        for (int i = 0; i < CollList.Count; i++)
        {
            if (Vector2.Distance(CollList[i].transform.position, rb.transform.position)>Attributes.VisionRange || !CollList[i].gameObject.activeSelf)CollList.RemoveAt(i);//在视野外的物体去除
        }
        //Debug.LogFormat("当前状态是{0}", state);
        switch(state)
        {
            case 0://随机漫步
            {
                RandomWalk();
                break;
            }
            case 1:
            {
                GetFood(); 
                break;
            }
            case 2:
                {
                    ChasePlayer(); break;
                }
            case 3:
                {
                    FleeFromPlayer(); break;
                }

        }     
    }
    void RandomWalk()
    {
        for (int i = 0; i < CollList.Count; i++)
        {
            coll = CollList[i];
            if(coll == null)
            {
                CollList.RemoveAt(i);
                continue;
            }
            if(!coll.gameObject.activeSelf)
            {
                CollList.RemoveAt(i);
                continue;
            }
            if (coll != null)
            {
                //Debug.LogFormat("碰撞箱类型:{0}", coll.tag);
                if (coll.CompareTag("fruit"))//从漫步到干饭
                {
                    Fruit = coll;
                    state = 1;
                }
                if (coll.CompareTag("Player") && !coll.gameObject.GetComponent<Attributes>().Hide)//从漫步到追人 前提是人没有躲草
                {
                    Player = coll;
                    state = 2;
                }
            }
        }

        if (state != 0)
        {
            rb.velocity = new Vector2(0, 0);
            WalkTime = 0;
            return;
        }
        //Debug.LogFormat("walktime={0}", WalkTime);
        if(WalkTime==0)//时间间隔归零，改变随机漫步方向
        {
            WalkTime=Random.Range(GameManager.Instance.RandomMinWalkTime,GameManager.Instance.RandomMaxWalkTime);
            int randomValue1 = Random.Range(-1, 2);
            int randomValue2 = Random.Range(-1, 2);
            rb.velocity = new Vector2(randomValue1 * Attributes.MoveSpeed, randomValue2 * Attributes.MoveSpeed);
        }
        WalkTime--;



    }
    void GetFood()
    {

        for (int i = 0; i < CollList.Count; i++)
        {
            coll = CollList[i];
            if (coll == null)
            {
                CollList.RemoveAt(i);
                continue;
            }
            if (!coll.gameObject.activeSelf)
            {
                CollList.RemoveAt(i);
                continue;
            }
            if (Fruit != null) if (Vector2.Distance(rb.transform.position, Fruit.transform.position) < GameManager.Instance.MinStep)
                {
                    state = 0;
                }
            if (coll != null)
            {
                if (coll.CompareTag("Player") && !coll.gameObject.GetComponent<Attributes>().Hide)//从干饭到追人
                {
                    Player = coll;
                    state = 2;
                }
                else if (coll.CompareTag("fruit"))
                {
                    if (Vector2.Distance(Fruit.transform.position, transform.position) > Vector2.Distance(coll.transform.position, transform.position))
                    {
                        Fruit = coll;
                        //吃更近的水果
                    }
                }
            }
        }
        if (Fruit == null || !Fruit.gameObject.activeSelf) state = 0;


        if (state != 1)
        {
            Fruit = null;
            return;
        }
        Vector2 direction = (Fruit.transform.position - rb.transform.position).normalized;//获取方向向量
        rb.velocity = direction * Attributes.MoveSpeed;
    }
    void ChasePlayer()
    {
        if (Player != null)
        {
            //退出：脱离视野 或 血量太低
            if (Vector2.Distance(Player.transform.position, rb.transform.position) >= Attributes.VisionRange + Attributes.ExtraChaseRange || !Player.gameObject.activeSelf)//脱离范围则回到漫步状态
            {
                Player = null;
                state = 0;
            }
            if(Attributes.hp/Attributes.MaxHP <= Attributes.FleeHPPersent)
            {
                state = 3;
            }
        }
        if(Player==null)
        {
            state = 0;
        }
        if (state != 2)
        {
            return;
        }

        Vector2 direction = (Player.transform.position - rb.transform.position).normalized;
        HitPlayer(direction);
        rb.velocity = direction*Attributes.MoveSpeed;
    }
    void HitPlayer(Vector2 direction)
    {
        if (CD > 0) return;
        //Debug.LogFormat("为什么射不出来好难受,方向是{0}",direction);
        AttackFunction.shoot(rb, direction, Attributes);//向玩家射击
        CD = Attributes.AttackTime;
    }
    void FleeFromPlayer()
    {
        if(Player!=null) 
        {
            if(Vector2.Distance(Player.transform.position, rb.transform.position) >= Attributes.ExtraFleeRange)
            {
                state=0;

            }
        }

        if (state != 3)
        {
            Player = null;
            return;
        }

        Vector2 direction = (Player.transform.position - rb.transform.position).normalized;
        rb.velocity = -direction * Attributes.MoveSpeed;
    }
}
