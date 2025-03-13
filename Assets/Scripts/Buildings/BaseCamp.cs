using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 시작할 때 주어지는 건물로, 건설할 수 없다
/// 일정시간마다 피로도를 회복해준다
/// </summary>
public class BaseCamp : MonoBehaviour, IBuilding, IRecoverable
{
    public float amount;    // 회복량
    public float RecoverAmount { get => amount; set => amount = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 시간마다 player의 피로도 회복

    }
    public void RecoverPerSecond()
    {
        throw new System.NotImplementedException();
    }

}
