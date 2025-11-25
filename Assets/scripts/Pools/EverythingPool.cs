using System.Collections.Generic;
using UnityEngine;
using static AllControl;

public class EverythingPool : MonoBehaviour
{
    public GameObject ItemPrefab;
    public int PoolSize;
    private Queue<GameObject> availableItem = new Queue<GameObject>();
    Vector3 StartScale;
    void Start()
    {
        StartScale = ItemPrefab.transform.localScale;
        // 预先创建子弹并放入池中
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject newItem = Instantiate(ItemPrefab);//创建子弹
            newItem.SetActive(false);//你先在池子里待着吧
            availableItem.Enqueue(newItem);//加入可用的部分
        }
    }

    public GameObject GetItem(Vector3 position)
    {
        GameObject Item;
        if (availableItem.Count > 0)
        {
            Item = availableItem.Dequeue();

        }
        // 池空了，动态扩展
        else
        {
            Item = Instantiate(ItemPrefab);
        }
        Item.SetActive(true);
        Item.transform.position = position;
        return Item;
    }

    public void ReturnItem(GameObject Item)//回收子弹
    {
        Item.transform.parent = null;
        Item.SetActive(false);
        Item.transform.localScale = StartScale; //防止无限缩放
        availableItem.Enqueue(Item);
    }
}