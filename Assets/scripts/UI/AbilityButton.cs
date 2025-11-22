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
    private int IsHovering=0;
    private Ability Ability;
    private Action HideButton;
    public void Initialize(Ability ability,Action hidebutton)
    {
        Ability = ability; 
        TextDescription.text=ability.description;
        TextName.text = ability.abilityName;
        HideButton = hidebutton;
        TextDescription.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("悬停了\n");
        TextDescription.gameObject.SetActive(true);
        IsHovering = 1;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("走了\n");
        TextDescription.gameObject.SetActive(false);
        IsHovering = 0;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("我按了");
        Ability.isUnlocked= true;//标记已经解锁
        Ability.unlockAction();
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
