using _23DaysLeft.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _23DaysLeft.UI
{
    public class CreatureHealthBar : MonoBehaviour
    {
        public Image fillImage;
        public Vector3 offset;

        private Transform targetTr;
        private Camera mainCamera;
        private float maxHealth;

        public void SetInfo(Transform target, float maxHp)
        {
            if (!mainCamera) mainCamera = Camera.main;
            transform.localScale = Vector3.one;
            maxHealth = maxHp;
            targetTr = target;
        }

        private void Update()
        {
            if (!targetTr) return;
            transform.position = targetTr.position + offset;
            transform.LookAt(mainCamera.transform);
        }

        public void ChangeHealth(float health)
        {
            fillImage.fillAmount = health / maxHealth;
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