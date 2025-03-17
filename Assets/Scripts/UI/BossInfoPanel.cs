using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace _23DaysLeft.UI
{
    public class BossInfoPanel : MonoBehaviour, UIBase
    {
        public TextMeshProUGUI NameText;
        public Image FillImage;

        private float maxHealth;

        public void SetInfo(string name, float maxHp)
        {
            NameText.text = name;
            maxHealth = maxHp;
        }

        public void ChangeHealth(float health)
        {
            FillImage.fillAmount = health / maxHealth;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}