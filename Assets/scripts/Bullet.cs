using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    private Attributes Attributes;
    private Vector2 dir;
    private Vector3 InitialPos;
    private Rigidbody2D rb;
    public GameObject Pool;
    // Start is called before the first frame update
    public void Initialize(Vector2 direction,Attributes myAttributes)
    {
        Attributes= myAttributes;
        dir = direction;
        InitialPos=Attributes.gameObject.GetComponent<Rigidbody2D>().transform.position;//获取当前角色坐标
        //Debug.Log("老子没给你定义吗你啊？");
        rb=GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("rock"))
        {
            Pool.GetComponent<BulletPool>().ReturnBullet(this.gameObject);
        }
        if(collision.CompareTag("Player") && collision.gameObject != Attributes.gameObject)//子弹碰到人且不是自己
        {
            //Debug.Log("我们确实击中了");
            collision.gameObject.GetComponent<Attributes>().GetDamage(Attributes,Attributes.AttackPower);
            Pool.GetComponent<BulletPool>().ReturnBullet(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Attributes == null) Pool.GetComponent<BulletPool>().ReturnBullet(this.gameObject);//如果子弹在打中人前，自己先死了，那子弹一起消失
        //Debug.LogFormat("距离初始位置距离={0}", Vector2.Distance(rb.transform.position, InitialPos));
        if (Vector2.Distance(rb.transform.position, InitialPos)>Attributes.AttackRange)
        {
            Pool.GetComponent<BulletPool>().ReturnBullet(this.gameObject);
        }
        rb.transform.Translate(dir * Attributes.BulletSpeed * Time.deltaTime);

    }
}
