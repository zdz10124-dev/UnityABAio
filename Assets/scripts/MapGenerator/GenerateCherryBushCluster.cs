using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCherryBushCluster : MonoBehaviour,IBuildings
{
    public int GenerateWeight { get; set; } = 10;
    public int MaxCount { get; set; } = -1;//-1代表不限制
    public GameObject pool;
    private List<float> dx= new List<float> { 0.5f, 0.25f, 0f, -0.25f, -0.5f, -0.25f, 0f, 0.25f };
    private List<float> dy = new List<float> { 0f, 0.25f, 0.5f, 0.25f, 0f, -0.25f, -0.5f, -0.25f };

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Generate(int x, int y)
    {
        //Debug.Log("俺真的尝试生成了得苏哇");
        for(int i=0;i<8;i++)pool.gameObject.GetComponent<EverythingPool>().GetItem(new Vector3(x + dx[i], y + dy[i], 0));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
