using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TempResourceType
{
    None,
    Wood,   // 나무
    Stone,  // 돌
    Steel,  // 철
             

}


/// <summary>
/// 자원 (임시)
/// </summary>
public class TempResource : TempItems
{
    public TempResourceType resourceType;
    
    // Start is called before the first frame update
    void Start()
    {
        // Scriptable Object 초기화
    }
}
