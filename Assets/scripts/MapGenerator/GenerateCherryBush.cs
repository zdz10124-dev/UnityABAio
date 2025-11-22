using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCherryBush : MonoBehaviour,IBuildings
{
    public int GenerateWeight { get; set; }=10;
    public GameObject pool;
    public float xbias=1f;
    public float ybias=1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Generate(int x,int y)
    {
        //Debug.Log("俺真的尝试生成了得苏哇");
        pool.gameObject.GetComponent<EverythingPool>().GetItem(new Vector3(x+Random.Range(0,xbias), y+Random.Range(0,ybias), 0));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
