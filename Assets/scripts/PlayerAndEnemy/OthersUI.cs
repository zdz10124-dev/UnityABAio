using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class OthersUI : MonoBehaviour
{
    private Attributes Attributes;
    private const float LifeBarXscale = 14.06739f;
    [SerializeField] public Image LifeBar;
    public TextMeshProUGUI DamageDisplayUI;
    // Start is called before the first frame update
    void Start()
    {
        Attributes = GetComponent<Attributes>();
    }
    public void DamageDisplay(float damage)
    {
        DamageDisplayUI.gameObject.SetActive(true);
        DamageDisplayUI.text = "-"+((int)damage).ToString();
        DamageDisplayUI.GetComponent<Animation>().Play();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 currentScale = LifeBar.rectTransform.localScale;
        LifeBar.rectTransform.localScale = new Vector3((Attributes.hp / Attributes.MaxHP) * LifeBarXscale, currentScale.y, currentScale.z);//œ‘ æ…˙√¸Ãı
    }
}
