using System.Collections;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public GameObject rainParticle;

    [SerializeField] float nextRainTime;
    [SerializeField] float afterLastRainTime;
    [SerializeField] float rainningTime;

    [Header("Fog Settings")]
    public float clearFogDensity = 0.002f;  // ���� ������ �Ȱ� �е�
    public float rainFogDensity = 0.02f;    // �� ���� ������ �Ȱ� �е�
    public Color clearAmbientLight = Color.white;
    public Color rainAmbientLight = Color.gray;
    public float fogChangeSpeed = 0.5f; // �Ȱ� ��ȭ �ӵ�

    private void Start()
    {
        nextRainTime = Random.Range(120, 600); // 1 ~ 5�Ͽ� �� ���� �� ����
        rainningTime = Random.Range(60, 120); // �ݳ��� ~ �Ϸ絿�� �񳻸�
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
        // �� ����
        rainParticle.SetActive(true);
        StartCoroutine(ChangeFogDensity(RenderSettings.fogDensity, rainFogDensity));
        StartCoroutine(ChangeAmbientLight(RenderSettings.ambientLight, rainAmbientLight));

        yield return new WaitForSeconds(rainningTime);

        // �� ����
        rainParticle.SetActive(false);
        StartCoroutine(ChangeFogDensity(RenderSettings.fogDensity, clearFogDensity));
        StartCoroutine(ChangeAmbientLight(RenderSettings.ambientLight, clearAmbientLight));

        // ���� �� ������ �ð� ����
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
