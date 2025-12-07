using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDoDamage : MonoBehaviour
{
    private Attributes Attributes;
    private float HigherDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Initialize(Attributes attributes,float higherDamage=1f)
    {
        Attributes = attributes;
        HigherDamage = higherDamage;
    }
    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null) return;
        if(collision.gameObject.CompareTag("Player"))
        {
            if(collision.gameObject.GetComponent<Attributes>().Team!=Attributes.Team)
            {
                collision.gameObject.GetComponent<Attributes>().GetDamage(Attributes,Attributes.AttackPower/60*HigherDamage);
            }    
        }
    }
    void Update()
    {
        
    }
}
