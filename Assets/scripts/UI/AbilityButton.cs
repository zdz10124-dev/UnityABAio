using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    // Start is called before the first frame update
    public TextMeshProUGUI TextDescription;
    public TextMeshProUGUI TextName;
    public TextMeshProUGUI LevelOfAbility;//能力等级
    //private int IsHovering=0;
    private Ability Ability;
    private Action HideButton;
    public void Initialize(Ability ability,Action hidebutton)
    {
        Ability = ability; 
        TextDescription.text=ability.description;
        TextName.text = ability.abilityName;
        LevelOfAbility.text ="lv:"+ ability.AbilityLevel.ToString();
        HideButton = hidebutton;
        TextDescription.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("悬停了\n");
        TextDescription.gameObject.SetActive(true);
        //IsHovering = 1;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("走了\n");
        TextDescription.gameObject.SetActive(false);
        //IsHovering = 0;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(Ability==null)
        {
            Debug.Log("Ability不存在");
            Debug.LogFormat("出现问题的对象名字是{0}", gameObject.name);
        }
        Ability.AbilityLevel++;//能力等级+1
        Ability.unlockAction(Ability.AbilityLevel);
        HideButton();//选中一个能力就隐藏其他所有能力
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
