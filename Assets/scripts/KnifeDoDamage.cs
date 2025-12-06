using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDoDamage : MonoBehaviour
{
    private Attributes Attributes;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Initialize(Attributes attributes)
    {
        Attributes = attributes;
    }
    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null) return;
        if(collision.gameObject.CompareTag("Player"))
        {
            if(collision.gameObject!=Attributes.gameObject)
            {
                collision.gameObject.GetComponent<Attributes>().GetDamage(Attributes,Attributes.AttackPower/60);
            }    
        }
    }
    void Update()
    {
        
    }
}
