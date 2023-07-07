using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommand : MonoBehaviour
{
    // 명령 옵션을 시각화해서 보여주는 ui 필요
    // 키입력 명령 ui 활성화

    // 링크드 리스트 개념
    private List<SoldierController> unit;
    private SquadLeader head;  // Soldier 클래스를 상속받는 squadLeader 스크립트 구현 혹은 squadLeader를 인터페이스로?
    // followMe 시 head는 플레이어 본인?  혹은 head의 이동 목표가 플레이어
    private Soldier tail;
    private List<List<SoldierController>> subordinateUnits;
    private LayerMask groundLayerMask;
    private GameObject banner;
    // 레이캐스트로 ground layerMask 충돌 위치에 banner로 위치 표시

    // charge, follow, move ,retreat, look
    // shield up, fire at will, ride on horse, dismount
    // battle formation  make line, spread out, make circle, make square, wedge formation, column formation, v formation
    // 대형도 scriptable object로 관리?
    private void FollowMe()
    {
    }
}
