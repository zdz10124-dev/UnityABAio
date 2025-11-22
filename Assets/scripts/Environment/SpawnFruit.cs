using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AllControl;

public class SpawnFruit : MonoBehaviour
{
    private GameObject fruit;
    public FruitPool FruitPool;
    private int spawning=0;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount==0 && spawning==0)
        {
            Invoke("Spawn", Random.Range(GameManager.Instance.CherrySpawnMinTime,GameManager.Instance.CherrySpawnMaxTime));
            spawning = 1;
        }
    }
    void Spawn()
    {
        fruit = FruitPool.GetFruit(this.transform.position);
        fruit.transform.SetParent(transform);
        spawning = 0;
    }
}
