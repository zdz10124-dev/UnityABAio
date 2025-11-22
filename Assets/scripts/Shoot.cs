using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    int CD = 0;
    public GameObject Pool;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CD > 0) CD--;
    }
    public void shoot(Rigidbody2D rb,Vector2 direction,Attributes Attributes)//rb是射出者
    {
        //Debug.Log("我可射了");
        if (CD>0) return;
        CD = Attributes.AttackTime;
        GameObject newBullet = Pool.GetComponent<BulletPool>().GetBullet(rb.transform.position);//创建子弹
        newBullet.SetActive(true);
        Bullet Bullet = newBullet.GetComponent<Bullet>();//获取子弹挂载的脚本
        Bullet.Initialize(direction, Attributes);//传递参数
    }
}
