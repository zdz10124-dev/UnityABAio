using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GoodTools;
using static AllControl;

public class AttackFunction : MonoBehaviour
{
    // Start is called before the first frame update
    public EverythingPool KnifePool;

    void Start()
    {
        
    }

    // Update is called once per frame
    public void SwingKnife(Attributes Attacker,Vector3 pos)
    {
        GameObject Knife=KnifePool.GetItem(new Vector3(Attacker.transform.position.x,Attacker.transform.position.y+GameManager.Instance.SwingKnifeDertaY,Attacker.transform.position.z));
        Knife.GetComponent<KnifeDoDamage>().Initialize(Attacker);
        Knife.transform.localScale = Knife.transform.localScale * Attacker.AbilityAssassinBiggerKnife;//能力：大刀光
        RotateAroundBToLine(Knife,Attacker.gameObject,pos);
    }
    void Update()
    {
        
    }
}
