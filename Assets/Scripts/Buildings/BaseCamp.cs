using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �� �־����� �ǹ���, �Ǽ��� �� ����
/// �����ð����� �Ƿε��� ȸ�����ش�
/// </summary>
public class BaseCamp : MonoBehaviour, IBuilding, IRecoverable
{
    public float amount;    // ȸ����
    public float RecoverAmount { get => amount; set => amount = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // �ð����� player�� �Ƿε� ȸ��

    }
    public void RecoverPerSecond()
    {
        throw new System.NotImplementedException();
    }

}
