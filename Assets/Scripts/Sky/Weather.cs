using System.Collections;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public GameObject rainParticle;

    // ������ �ð��� ������ �ð� ���� �� ������ �Ѵ�.

    [SerializeField] float nextRainTime;
    [SerializeField] float afterLastRainTime;

    [SerializeField] float rainningTime;


    private void Start()
    {
        nextRainTime = Random.Range(120, 600); // 1 ~ 5�Ͽ� �� ���� �� ���� (120, 600)
        rainningTime = Random.Range(60, 120); // �ݳ��� ~ �Ϸ絿�� �񳻸� (60, 120)
    }

    private void Update()
    {
        if (afterLastRainTime < nextRainTime)
        {
            afterLastRainTime += Time.deltaTime;
        }
        else if (!rainParticle.activeSelf)  // �� ���� ���� ���� ����
        {
            StartCoroutine(Rain());
        }
    }

    IEnumerator Rain()
    {
        rainParticle.SetActive(true);
        yield return new WaitForSeconds(rainningTime);
        rainningTime = Random.Range(60, 120); // �ݳ��� ~ �Ϸ絿�� �񳻸� (60, 120)
        rainParticle.SetActive(false);
        afterLastRainTime = 0;
        nextRainTime = Random.Range(120, 600); // 1 ~ 5�Ͽ� �� ���� �� ����(120, 600)
    }
}
