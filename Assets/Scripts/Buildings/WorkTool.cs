using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ToolType
{
    None,   // �⺻��
    Pick,   // ���
    Hammer, // ��ġ
    Saw,    // ��
    Shovel, // ��
    Axe,    // ����
}


/// <summary>
/// �ǹ��� ���Ǵ� ����
/// Scriptable Object ����Ͽ� �ʱ�ȭ
/// </summary>
public class WorkTool : TempItems
{
    public ToolType toolType;

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
