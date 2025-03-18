using System;
using System.Collections;
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
            StopAllCoroutines();
            StartCoroutine(ChangeHealthCoroutine(value));
        }
        
        private IEnumerator ChangeHealthCoroutine(float value)
        {
            const float duration = 0.5f;
            float time = 0f;
            float current = healthFill.fillAmount;

            while (time < duration)
            {
                time += Time.deltaTime;
                healthFill.fillAmount = Mathf.Lerp(current, value, time / duration);
                yield return null;
            }
        }

        private void ChangeHunger(float value)
        {
            hungerFill.fillAmount = value;
        }
    }
}