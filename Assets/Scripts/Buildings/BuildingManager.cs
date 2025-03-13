using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 싱글턴이 아닌 매니저
/// 전체 건물을 관리
/// </summary>
public class BuildingManager : MonoBehaviour
{
    // 전체 건물을 관리(건물 정보는 가지고 있는 편이 좋을까?)
    // 건물을 순차적으로 저장하고, 앞 인덱스의 건물이 있어야만 뒤의 인덱스 건물을 지을 수 있게 한다
    // 건물이름을 key로 하고 건물을 value로 저장
    public Dictionary<string, IBuilding> buildingInfos = new Dictionary<string, IBuilding>();

    // 앞의 건물을 지어야 그 다음 건물을 지을 수 있다


    // 현재 플레이어가 지은 건축물 관리
    public List<IBuilding> playerBuildings = new List<IBuilding>();
    
    // 건물 관리 메서드는 팩토리 메서드 패턴
    public BuildingFactory factory;



    // Start is called before the first frame update
    void Start()
    {
        
    }
}
