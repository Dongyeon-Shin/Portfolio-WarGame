using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommand : MonoBehaviour
{
    // ��� �ɼ��� �ð�ȭ�ؼ� �����ִ� ui �ʿ�
    // Ű�Է� ��� ui Ȱ��ȭ

    // ��ũ�� ����Ʈ ����
    private List<SoldierController> unit;
    private Soldier head;  // Soldier Ŭ������ ��ӹ޴� squadLeader ��ũ��Ʈ ����
    private Soldier tail;
    private List<List<SoldierController>> subordinateUnits;
    private LayerMask groundLayerMask;
    private GameObject banner;
    // ����ĳ��Ʈ�� ground layerMask �浹 ��ġ�� banner�� ��ġ ǥ��

    // charge, follow, move ,retreat, look
    // shield up, fire at will, ride on horse, dismount
    // battle formation  make line, spread out, make circle, make square, wedge formation, column formation, v formation
    // ������ scriptable object�� ����?
}
