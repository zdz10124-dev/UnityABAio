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
        if (Attributes.IsPlayer == 3) return;
        //Debug.LogFormat("吃樱桃器查询碰撞中，当前碰到物品标签为{0}", collision.gameObject.tag);

        if (collision.gameObject.CompareTag("fruit"))
        {
            if (Attributes.IsPlayer == 2 && !Attributes.Owner.SummonHelpEat) return;
            collision.gameObject.GetComponent<FindPool>().MyPool.gameObject.GetComponent<FruitPool>().ReturnFruit(collision.gameObject);  //水果储存了自己的对象池索引，所以可以对所有水果各回各家  
                                                                                                                                          //CollectEffect.Play();
            Attributes.AddHP(GameManager.Instance.CherryHPUp);
            if (Attributes.IsPlayer==0)
            {
                Attributes.xp+= GameManager.Instance.XPperCherry*GameManager.Instance.ExtraEXP;//人机额外经验
            }
            else if (Attributes.IsPlayer == 1)
            {
                Attributes.xp += GameManager.Instance.XPperCherry;
            }
            else if (Attributes.IsPlayer == 2)
            {
                Attributes.Owner.xp += GameManager.Instance.XPperCherry;
            }


            //cherriesText.text = "Cherries:" + cherries;
            //GameManager.Instance.score = cherries;
        }    
    }
}
