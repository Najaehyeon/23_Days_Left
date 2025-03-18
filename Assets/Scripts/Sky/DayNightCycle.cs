using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0f, 1f)]
    public float currentTime;
    public float fullDayLength = 120; // 하루 길이 (초 단위)
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    public int dayCount = 1; // 현재 일수
    private float elapsedTime; // 경과한 시간

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        elapsedTime = fullDayLength * startTime; // 시작 시간 반영
        currentTime = startTime;
        Global.Instance.UIManager.OnChangeDay(dayCount);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        currentTime = (elapsedTime % fullDayLength) / fullDayLength;

        // 하루가 지나면 일수 증가
        if (elapsedTime >= fullDayLength * dayCount + (fullDayLength * startTime))
        {
            dayCount++;
            Global.Instance.UIManager.OnChangeDay(dayCount);
        }

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(currentTime);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(currentTime);
    }

    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(currentTime);
        lightSource.transform.eulerAngles = (currentTime - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        lightSource.color = gradient.Evaluate(currentTime);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
}
