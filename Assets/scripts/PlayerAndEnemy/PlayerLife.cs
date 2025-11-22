using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class PlayerLife : MonoBehaviour
{
    private Attributes Attributes;
    // Start is called before the first frame update
    void Start()
    {
        Attributes = GetComponent<Attributes>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Attributes.hp <= 0)
        {
            die();
        }
    }
    void die()
    {
        if (transform.parent != null)
        {
            if(transform.parent.CompareTag("enemy"))
            {
                transform.parent.gameObject.GetComponent<FindPool>().MyPool.gameObject.GetComponent<EverythingPool>().ReturnItem(transform.parent.gameObject);
            }
        }
        Destroy(this.gameObject);
    }
}
