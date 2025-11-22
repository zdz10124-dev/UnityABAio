using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AllControl;

public class RobotVision : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    private BoxCollider2D coll;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        coll.size = new Vector2(2 * enemy.GetComponent<Attributes>().VisionRange, 2 * enemy.GetComponent<Attributes>().VisionRange);
    }

    // Update is called once per frame
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision == enemy.GetComponent<Collider2D>()) return;
        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.CompareTag("enemy")) return;
        }
        //Debug.LogFormat("俺寻思俺碰到了啊,我碰到tag={0}", collision.gameObject.tag) ;
        if(!enemy.GetComponent<EnemyAI>().CollList.Contains(collision)) enemy.GetComponent<EnemyAI>().CollList.Add(collision);//如果没有该对象再放进去
    }

    void Update()
    {
        GetComponent<Collider2D>().transform.position=enemy.transform.position;
    }
}
