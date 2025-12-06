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
        RotateAroundBToLine(Knife,Attacker.gameObject,pos);
        if(Attacker.AbilityAssassinEnhancedAttackCount>0) 
        {
            Knife.GetComponent<KnifeDoDamage>().Initialize(Attacker,Attacker.AbilityAssassinEnhancedAttackHigherDamage);//如果是强化攻击，伤害变高
            Knife.transform.localScale = Knife.transform.localScale * Attacker.AbilityAssassinBiggerKnife;//如果是强化攻击，刀光变大
            Attacker.PlayerUI.EnhancedAttackPicture.gameObject.SetActive(false);//强普标识去掉
            Attacker.AbilityAssassinEnhancedAttackCount = 0;
        }       
        else Knife.GetComponent<KnifeDoDamage>().Initialize(Attacker);//不是强化攻击，正常创建
        Attacker.EnhancedAttackCD = Attacker.AbilityAssassinEnhancedAttackCD;//只要你攻击了，就重置强化普攻cd
        Knife.transform.localScale = Knife.transform.localScale * Attacker.AbilityAssassinBiggerKnife;//能力：大刀光

    }
    void Update()
    {
        
    }
}
