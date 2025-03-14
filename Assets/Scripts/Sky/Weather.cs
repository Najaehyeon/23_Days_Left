using System.Collections;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public GameObject rainParticle;

    [SerializeField] float nextRainTime;
    [SerializeField] float afterLastRainTime;
    [SerializeField] float rainningTime;

    [Header("Fog Settings")]
    public float clearFogDensity = 0.002f;  // 맑은 날씨의 안개 밀도
    public float rainFogDensity = 0.02f;    // 비 오는 날씨의 안개 밀도
    public Color clearAmbientLight = Color.white;
    public Color rainAmbientLight = Color.gray;
    public float fogChangeSpeed = 0.5f; // 안개 변화 속도

    private void Start()
    {
        nextRainTime = Random.Range(120, 600); // 1 ~ 5일에 한 번씩 비 내림
        rainningTime = Random.Range(60, 120); // 반나절 ~ 하루동안 비내림
    }

    private void Update()
    {
        if (afterLastRainTime < nextRainTime)
        {
            afterLastRainTime += Time.deltaTime;
        }
        else if (!rainParticle.activeSelf)
        {
            StartCoroutine(Rain());
        }
    }

    IEnumerator Rain()
    {
        // 비 시작
        rainParticle.SetActive(true);
        StartCoroutine(ChangeFogDensity(RenderSettings.fogDensity, rainFogDensity));
        StartCoroutine(ChangeAmbientLight(RenderSettings.ambientLight, rainAmbientLight));

        yield return new WaitForSeconds(rainningTime);

        // 비 종료
        rainParticle.SetActive(false);
        StartCoroutine(ChangeFogDensity(RenderSettings.fogDensity, clearFogDensity));
        StartCoroutine(ChangeAmbientLight(RenderSettings.ambientLight, clearAmbientLight));

        // 다음 비 내리는 시간 설정
        afterLastRainTime = 0;
        nextRainTime = Random.Range(120, 600);
    }

    IEnumerator ChangeFogDensity(float start, float target)
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            RenderSettings.fogDensity = Mathf.Lerp(start, target, elapsedTime);
            elapsedTime += Time.deltaTime * fogChangeSpeed;
            yield return null;
        }
        RenderSettings.fogDensity = target;
    }

    IEnumerator ChangeAmbientLight(Color start, Color target)
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            RenderSettings.ambientLight = Color.Lerp(start, target, elapsedTime);
            elapsedTime += Time.deltaTime * fogChangeSpeed;
            yield return null;
        }
        RenderSettings.ambientLight = target;
    }
}
