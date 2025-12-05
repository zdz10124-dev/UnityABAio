using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrack : MonoBehaviour
{
    private CircleCollider2D coll;
    public Bullet Bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Initialize(float r)
    {
        coll=GetComponent<CircleCollider2D>();
        coll.radius=r;//设置碰撞箱大小
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("vision")) return;//忽略敌人视野的碰撞箱
            
            if (collision.gameObject != Bullet.Attributes.gameObject && Bullet.EnemyFound==null)//不是自己且当前没有追踪对象
            {
                //if(Bullet.Attributes.IsPlayer==1)Debug.LogFormat("检测到类型{0},对象名字是{1}", collision.gameObject.tag, collision.gameObject.name);
                if (collision.CompareTag("Player"))//tag是Player
                {
                    //Debug.Log("成功检测到敌人");
                    Bullet.EnemyFound= collision.gameObject; //传一下追踪的敌人
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
