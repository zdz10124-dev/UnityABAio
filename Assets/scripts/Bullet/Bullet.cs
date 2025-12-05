using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public Attributes Attributes;
    private Vector2 dir;
    private Vector3 InitialPos;
    private Rigidbody2D rb;
    public GameObject Pool;//子弹库
    public GameObject EnemyFound;//追踪的敌人
    //脚本引用
    public BulletTrack BulletTrack;//追踪用
    // Start is called before the first frame update
    public void Initialize(Vector2 direction,Attributes myAttributes)
    {
        Attributes= myAttributes;
        dir = direction;
        InitialPos=Attributes.gameObject.GetComponent<Rigidbody2D>().transform.position;//获取当前角色坐标
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x/myAttributes.AbilitySniperSmallerBulletScale, gameObject.transform.localScale.y / myAttributes.AbilitySniperSmallerBulletScale, gameObject.transform.localScale.z / myAttributes.AbilitySniperSmallerBulletScale);//狙击手子弹变小的天赋
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * myAttributes.AbilityFirePowerBiggerBulletScale, gameObject.transform.localScale.y * myAttributes.AbilityFirePowerBiggerBulletScale, gameObject.transform.localScale.z * myAttributes.AbilityFirePowerBiggerBulletScale);//火力流子弹变大的天赋
        //Debug.Log("老子没给你定义吗你啊？");
        rb =GetComponent<Rigidbody2D>();
        rb.velocity = dir*Attributes.BulletSpeed;
        BulletTrack.Initialize(Attributes.AbilityFirePowerTrackRange);//设置追踪范围
    }
    void Start()
    {
        
    }   
    public void GetHit(Collider2D collision)
    {
        if (collision == null) return;
        if(collision.CompareTag("rock"))
        {
            Pool.GetComponent<BulletPool>().ReturnBullet(this.gameObject);
        }
        if(collision.CompareTag("Player") && collision.gameObject != Attributes.gameObject)//子弹碰到人且不是自己
        {
            //Debug.Log("我们确实击中了");
            collision.gameObject.GetComponent<Attributes>().GetDamage(Attributes,Attributes.AttackPower);
            if (Attributes.AbilityThornsHurtEachOther == 1 && !collision.gameObject.GetComponent<Attributes>().LiningPlayers.Contains(Attributes))//如果有互相伤害能力并且对方没有与自己连线
            {
                GameObject line=Attributes.DamageLinePool.GetComponent<EverythingPool>().GetItem(new Vector3(0,0,0));//创建一个线
                line.GetComponent<LinePosition>().Initialize(Attributes.gameObject,collision.gameObject);//给线设置两端
                collision.gameObject.GetComponent<Attributes>().LiningPlayers.Add(Attributes);//设置发出者
                collision.gameObject.GetComponent<Attributes>().DamageLines.Add(line);//记录这条线
            }
            if(Attributes.AbilityFirePowerExplodeRange>0.1)//会爆炸
            {
                Explode();
            }
            Pool.GetComponent<BulletPool>().ReturnBullet(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("我不update吗啊？");
        if(Attributes == null) Pool.GetComponent<BulletPool>().ReturnBullet(this.gameObject);//如果子弹在打中人前，自己先死了，那子弹一起消失
        //Debug.LogFormat("距离初始位置距离={0}", Vector2.Distance(rb.transform.position, InitialPos));
        if (Vector2.Distance(rb.transform.position, InitialPos)>Attributes.AttackRange)
        {
            Pool.GetComponent<BulletPool>().ReturnBullet(this.gameObject);
        }
        // rb.transform.Translate(dir * Attributes.BulletSpeed * Time.deltaTime);
        //Debug.LogFormat("我看看怎么个事{0}", EnemyFound);
        //if (EnemyFound != null) Debug.Log("有追踪中的敌人");
        if (EnemyFound != null) TrackEnemy();//追踪敌人

    }
    List<GameObject>  GetExplodeRange()//获得被爆炸波及的游戏对象
    {
        List<GameObject> players = new List<GameObject>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, Attributes.AbilityFirePowerExplodeRange);

        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Player"))
            {
                if(col.gameObject!=Attributes.gameObject)players.Add(col.gameObject);
            }
        }

        return players;
    }
    void Explode()
    {
        List<GameObject> a = GetExplodeRange();
        for(int i = 0; i < a.Count; i++)
        {
            a[i].GetComponent<Attributes>().GetDamage(Attributes, Attributes.AbilityFirePowerExplodeDamageRate * Attributes.AttackPower);//造成爆炸伤害
        }
    }
    void TrackEnemy()
    {
        //Debug.Log("在追踪了");
        if (!EnemyFound.activeSelf || Vector2.Distance(EnemyFound.transform.position, rb.transform.position) > Attributes.AbilityFirePowerTrackRange) EnemyFound = null;//如果敌人死了或者跑远了就停止追踪
        if (EnemyFound != null) rb.velocity=GetDirectionTo(EnemyFound.transform.position)*Attributes.BulletSpeed;//更改速度
    }
    private Vector3 GetDirectionTo(Vector3 targetPosition)
    {
        return (targetPosition - transform.position).normalized;
    }
}
