using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ToolType
{
    None,   // 기본값
    Pick,   // 곡괭이
    Hammer, // 망치
    Saw,    // 톱
    Shovel, // 삽
    Axe,    // 도끼
}


/// <summary>
/// 건물에 사용되는 도구
/// Scriptable Object 사용하여 초기화
/// </summary>
public class WorkTool : TempItems
{
    public ToolType toolType;

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
