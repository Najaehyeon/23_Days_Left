using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TempResourceType
{
    None,
    Wood,   // ����
    Stone,  // ��
    Steel,  // ö
             

}


/// <summary>
/// �ڿ� (�ӽ�)
/// </summary>
public class TempResource : TempItems
{
    public TempResourceType resourceType;
    
    // Start is called before the first frame update
    void Start()
    {
        // Scriptable Object �ʱ�ȭ
    }
}
