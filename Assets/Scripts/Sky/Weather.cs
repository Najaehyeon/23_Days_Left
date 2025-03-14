using System.Collections;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public GameObject rainParticle;

    // 무작위 시간에 무작위 시간 동안 비가 내리게 한다.

    [SerializeField] float nextRainTime;
    [SerializeField] float afterLastRainTime;

    [SerializeField] float rainningTime;


    private void Start()
    {
        nextRainTime = Random.Range(120, 600); // 1 ~ 5일에 한 번씩 비 내림 (120, 600)
        rainningTime = Random.Range(60, 120); // 반나절 ~ 하루동안 비내림 (60, 120)
    }

    private void Update()
    {
        if (afterLastRainTime < nextRainTime)
        {
            afterLastRainTime += Time.deltaTime;
        }
        else if (!rainParticle.activeSelf)  // 비가 오지 않을 때만 실행
        {
            StartCoroutine(Rain());
        }
    }

    IEnumerator Rain()
    {
        rainParticle.SetActive(true);
        yield return new WaitForSeconds(rainningTime);
        rainningTime = Random.Range(60, 120); // 반나절 ~ 하루동안 비내림 (60, 120)
        rainParticle.SetActive(false);
        afterLastRainTime = 0;
        nextRainTime = Random.Range(120, 600); // 1 ~ 5일에 한 번씩 비 내림(120, 600)
    }
}
