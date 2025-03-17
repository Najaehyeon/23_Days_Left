using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneBase : MonoBehaviour
{
    private static MainSceneBase instance;
    public static MainSceneBase Instance => !instance ? null : instance;
    
    // 씬이 로드되었을 때 할 일
    // 씬에서 사용 할 데이터들

    public void Awake()
    {
        instance = this;
    }
    
    public void OnDestroy()
    {
        instance = null;
    }
}
