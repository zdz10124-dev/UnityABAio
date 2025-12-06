using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnMyself : MonoBehaviour
{
    public EverythingPool MyPool;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ReturnMe()
    {
        MyPool.ReturnItem(gameObject);//字面意思，用于动画事件，淡出结束后把自己扔回对象池
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
