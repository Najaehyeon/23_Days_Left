using _23DaysLeft.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _23DaysLeft.UI
{
    public class LoadingPanel : MonoBehaviour
    {
        public Image FillImage;
        public Image VignetteImage;
        
        private readonly Color startColor = Color.white;
        private readonly Color endColor = new(1, 1, 1, 0.85f);
        private const float minDuration = 0.2f;
        private const float maxDuration = 0.5f;

        private void OnEnable()
        {
            StartCoroutine(VignetteCoroutine());
            UIManager.Instance.OnChangeLoadingProgress += SetLoadingProgress;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            UIManager.Instance.OnChangeLoadingProgress -= SetLoadingProgress;
        }

        private IEnumerator VignetteCoroutine()
        {
            while (true)
            {
                var time = 0f;
                var duration = Random.Range(minDuration, maxDuration);
                while (time < duration)
                {
                    time += Time.deltaTime;
                    VignetteImage.color = Color.Lerp(startColor, endColor, time / duration);
                    yield return null;
                }

                duration = Random.Range(minDuration, maxDuration);
                time = 0f;
                while (time < duration)
                {
                    time += Time.deltaTime;
                    VignetteImage.color = Color.Lerp(endColor, startColor, time / duration);
                    yield return null;
                }
            }
        }
        
        private void SetLoadingProgress(float progress)
        {
            FillImage.fillAmount = progress;
        }   
    }
}