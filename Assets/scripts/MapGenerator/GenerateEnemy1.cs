
using UnityEngine;

public class GenerateEnemy1 : MonoBehaviour,IBuildings
{
    public int GenerateWeight { get; set; } = 1;
    public int MaxCount { get; set; } = -1;
    public GameObject pool;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Generate(int x,int y)
    {
        
        pool.gameObject.GetComponent<EverythingPool>().GetItem(new Vector3(x,y,0));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
