using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitCheck : MonoBehaviour
{
    public Bullet Bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)//防止子物体调用父物体的该函数造成混乱，把该功能挪到子物体里
    {
        Bullet.GetHit(collision);//射中
    }
}
