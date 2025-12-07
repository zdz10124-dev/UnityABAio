using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedCreatrueAI : MonoBehaviour
{
    public Attributes Attributes;
    // Start is called before the first frame update
    void Start()
    {
        Attributes=gameObject.GetComponent<Attributes>();
    }
    public void Kill()
    {
        Attributes.hp = 0;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
