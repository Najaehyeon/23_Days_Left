using System;
using UnityEngine;
using UnityEngine.UI;

namespace _23DaysLeft.UI
{
    public class PlayerConditionPanel : MonoBehaviour
    {
        [SerializeField] private Image healthFill;
        [SerializeField] private Image hungerFill;
        public Action<float> OnChangeHealth;
        public Action<float> OnChangeHunger;

        public void Init()
        {
            OnChangeHealth += ChangeHealth;
            OnChangeHunger += ChangeHunger;
        }

        private void ChangeHealth(float value)
        {
            healthFill.fillAmount = value;
        }

        private void ChangeHunger(float value)
        {
            hungerFill.fillAmount = value;
        }
    }
}