using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static AllControl;



public class OthersUI : MonoBehaviour
{
    private Attributes Attributes;
    private const float LifeBarXscale = 14.06739f;
    [SerializeField] public Image LifeBar;
    public TextMeshProUGUI DamageDisplayUI;
    public float TotalDamage = 0;
    private int DamaeDisplayCD;//伤害显示的cd
    // Start is called before the first frame update
    void Start()
    {
        Attributes = GetComponent<Attributes>();
        DamaeDisplayCD = GameManager.Instance.DamaeDisplayCD;//设置伤害显示的间隔
    }
    public void DamageDisplay(float damage)
    {
        DamageDisplayUI.gameObject.SetActive(true);
        DamageDisplayUI.text = "-"+(((float)((int)(damage*10)))/10).ToString();//保留一位小数
        DamageDisplayUI.GetComponent<Animation>().Play();
    }
    // Update is called once per frame
    void Update()
    {
        if (DamaeDisplayCD > 0) DamaeDisplayCD--;//进行内置伤害显示时间读条
        if (DamaeDisplayCD == 0 && TotalDamage>0)
        {
            //Debug.LogFormat("TotalDamage={0}", TotalDamage);
            DamageDisplay(TotalDamage);
            DamaeDisplayCD = GameManager.Instance.DamaeDisplayCD;
            TotalDamage = 0f;
        }
        Vector3 currentScale = LifeBar.rectTransform.localScale;
        LifeBar.rectTransform.localScale = new Vector3((Attributes.hp / Attributes.MaxHP) * LifeBarXscale, currentScale.y, currentScale.z);//显示生命条
        
    }
}
