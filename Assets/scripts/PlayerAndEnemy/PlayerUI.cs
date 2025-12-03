using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour
{
    private Attributes Attributes;
    private const float XPbarXscale=14.06739f;
    [SerializeField] public Image XPbar;
    public TextMeshProUGUI TipUI;
    // Start is called before the first frame update
    void Start()
    {
        Attributes= GetComponent<Attributes>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentScale = XPbar.rectTransform.localScale;
        //Debug.LogFormat("下一级需要{0},百分比{1}",Attributes.NextLevelXP,Attributes.xp / Attributes.NextLevelXP);
        XPbar.rectTransform.localScale = new Vector3((Attributes.xp / Attributes.NextLevelXP)*XPbarXscale , currentScale.y, currentScale.z);//显示经验条
    }
    public void UpdateTips(string a)
    {
        TipUI.gameObject.SetActive(true);
        TipUI.text = a;
        TipUI.GetComponent<Animation>().Play();
    }
}
