using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class BossInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public Image FillImage;

    private float maxHealth;
    
    public void Init(string name, float maxHp)
    {
        NameText.text = name;
        maxHealth = maxHp;
    }

    public void ChangeHealth(float health)
    {
        FillImage.fillAmount = health / maxHealth;
    }

    public void Inactive()
    {
        gameObject.SetActive(false);
    }
}
