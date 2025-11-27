using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHideCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public Attributes Attributes;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("cover"))
        {
            Attributes.Hide=true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("cover"))
        {
            Attributes.Hide = false;
        }
    }
}
