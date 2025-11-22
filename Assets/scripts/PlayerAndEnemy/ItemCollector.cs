using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static AllControl;

public class ItemCollector : MonoBehaviour
{
    //[SerializeField] private AudioSource CollectEffect;
    //[SerializeField] private Text cherriesText;
    // Start is called before the first frame update
    private Attributes Attributes;
    void Start()
    {
        Attributes= GetComponent<Attributes>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.LogFormat("吃樱桃器查询碰撞中，当前碰到物品标签为{0}", collision.gameObject.tag);
        if (collision.gameObject.CompareTag("fruit"))
        {
            //Debug.Log("食物被吃: " + collision.name);
            //Debug.Log("食物位置: " + collision.transform.position);
            //Debug.Log("我的位置: " + transform.position);
            //Debug.Log("双方距离: " + Vector2.Distance(transform.position, collision.transform.position));
            collision.gameObject.GetComponent<FindPool>().MyPool.gameObject.GetComponent<FruitPool>().ReturnFruit(collision.gameObject);  //水果储存了自己的对象池索引，所以可以对所有水果各回各家  
            //CollectEffect.Play();
            Attributes.xp+= GameManager.Instance.XPperCherry;
            if(GameManager.Instance.CherryHPUp+Attributes.hp>=Attributes.MaxHP)Attributes.hp=Attributes.MaxHP;
            else Attributes.hp += GameManager.Instance.CherryHPUp;

            //cherriesText.text = "Cherries:" + cherries;
            //GameManager.Instance.score = cherries;
        }    
    }
}
