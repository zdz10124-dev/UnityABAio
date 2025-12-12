using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AllControl;

public class RobotVision : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    private BoxCollider2D coll;
    public Attributes Attributes;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        ChangeVision();
    }
    public void ChangeVision()
    {
        coll.size = new Vector2(2 * Attributes.VisionRange, 2 * Attributes.VisionRange);
    }
    // Update is called once per frame
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision == enemy.GetComponent<Collider2D>()) return;//如果是自己，不理
        if (collision.CompareTag("Player") && collision.GetComponent<Attributes>().Team == Attributes.Team) return;//同队，不理
        //Debug.LogFormat("俺寻思俺碰到了啊,我碰到tag={0}", collision.gameObject.tag) ;
        if(collision.CompareTag("bullet") && collision.GetComponent<Bullet>().Attributes.Team== Attributes.Team) return;//同队子弹忽略
        if(!enemy.GetComponent<EnemyAI>().CollList.Contains(collision)) enemy.GetComponent<EnemyAI>().CollList.Add(collision);//如果没有该对象再放进去
    }

    void Update()
    {
        GetComponent<Collider2D>().transform.position=enemy.transform.position;
    }
}
