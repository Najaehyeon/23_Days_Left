using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̱����� �ƴ� �Ŵ���
/// ��ü �ǹ��� ����
/// </summary>
public class BuildingManager : MonoBehaviour
{
    // ��ü �ǹ��� ����(�ǹ� ������ ������ �ִ� ���� ������?)
    // �ǹ��� ���������� �����ϰ�, �� �ε����� �ǹ��� �־�߸� ���� �ε��� �ǹ��� ���� �� �ְ� �Ѵ�
    // �ǹ��̸��� key�� �ϰ� �ǹ��� value�� ����
    public Dictionary<string, IBuilding> buildingInfos = new Dictionary<string, IBuilding>();

    // ���� �ǹ��� ����� �� ���� �ǹ��� ���� �� �ִ�


    // ���� �÷��̾ ���� ���๰ ����
    public List<IBuilding> playerBuildings = new List<IBuilding>();
    
    // �ǹ� ���� �޼���� ���丮 �޼��� ����
    public BuildingFactory factory;



    // Start is called before the first frame update
    void Start()
    {
        
    }
}
