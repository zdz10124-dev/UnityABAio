using System.Collections.Generic;
using UnityEngine;
using static AllControl;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Queue<GameObject> availableBullets = new Queue<GameObject>();
    Vector3 StartScale;
    void Start()
    {
        StartScale = bulletPrefab.transform.localScale;
        // 预先创建子弹并放入池中
        for (int i = 0; i < GameManager.Instance.BulletPoolSize; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab);//创建子弹
            newBullet.SetActive(false);//你先在池子里待着吧
            availableBullets.Enqueue(newBullet);//加入可用的部分
        }
    }

    public GameObject GetBullet(Vector3 position)
    {
        GameObject bullet;
        if (availableBullets.Count > 0)
        {
             bullet= availableBullets.Dequeue();
 
        }
        // 池空了，动态扩展
        else
        {
            bullet = Instantiate(bulletPrefab);
        } 
        bullet.SetActive(true);
        bullet.transform.position = position;
        return bullet;
    }

    public void ReturnBullet(GameObject bullet)//回收子弹
    {
        bullet.transform.parent = null;
        bullet.SetActive(false);
        availableBullets.Enqueue(bullet);
        bullet.transform.localScale = StartScale; //防止无限缩放
    }
}