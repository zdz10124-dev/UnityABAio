using System.Collections.Generic;
using UnityEngine;
using static AllControl;

public class FruitPool : MonoBehaviour
{
    public GameObject FruitPrefab;
    private Queue<GameObject> availableFruit = new Queue<GameObject>();
    Vector3 StartScale;
    void Start()
    {
        StartScale = FruitPrefab.transform.localScale;
        // 预先创建子弹并放入池中
        for (int i = 0; i < GameManager.Instance.FruitPoolSize; i++)
        {
            GameObject newFruit = Instantiate(FruitPrefab);//创建子弹
            newFruit.SetActive(false);//你先在池子里待着吧
            availableFruit.Enqueue(newFruit);//加入可用的部分
        }
    }

    public GameObject GetFruit(Vector3 position)
    {
        GameObject Fruit;
        if (availableFruit.Count > 0)
        {
            Fruit = availableFruit.Dequeue();

        }
        // 池空了，动态扩展
        else
        {
            Fruit = Instantiate(FruitPrefab);
        }
        Fruit.SetActive(true);
        Fruit.transform.position = position;
        return Fruit;
    }

    public void ReturnFruit(GameObject Fruit)//回收水果
    {
        Fruit.transform.parent = null;
        Fruit.SetActive(false);
        Fruit.transform.localScale= StartScale; //防止无限缩放
        availableFruit.Enqueue(Fruit);
    }
}