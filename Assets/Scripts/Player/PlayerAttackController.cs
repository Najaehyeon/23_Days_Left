using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    // 플레이어 손(or손목) 오브젝트 아래 무기들 배치
    // 장착하는 무기에 따라서 해당 무기만 활성화

    // 플레이어 몹 공격력, 나무 공격력, 광석 공격력, (내구도) 선언

    // 장착 무기에 따라서, 능력치 할당.

    // 좌클릭 시 공격
    // 공격시 애니메이션 발동
    // 애니메이션에서 타격 지점에 Hit 메서드 실행
    // Hit 매서드에서 크로스헤어에 대상이 있는 지 없는 지에 따라서 다르게 실행
    // 대상이 있고, 만약 그 대상이 나무(광석, 몬스터)일 경우, 나무(광석, 몬스터) 공격력 전달.
}
