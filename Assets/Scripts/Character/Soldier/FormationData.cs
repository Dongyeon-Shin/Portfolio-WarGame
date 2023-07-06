using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "FormationData", menuName = "Data/Formation")]
public class FormationData : ScriptableObject
{
    [SerializeField]
    private FormationInfo[] formations;
    public FormationInfo[] Formations { get { return formations; } }
    public enum Direction
    {
        forward,
        leftForward,
        rightForward,
        left,
        right,
        backward,
        leftBackward,
        rightBackward
    }
    private Dictionary<string, FormationInfo> combatFormation = new Dictionary<string, FormationInfo>();
    public Dictionary<string, FormationInfo> CombatFormation {  get { return combatFormation; } }
# if UNITY_EDITOR
    private void OnEnable()
    {
        foreach (FormationInfo formation in formations)
        {
            if (combatFormation.ContainsKey(formation.formationName))
            {
                continue;
            }
            combatFormation.Add(formation.formationName, formation);
        }
    }
#endif
    public Vector3 GetPositioninFormation(FormationInfo currentFormation, int index, bool squard1)
    {
        switch (currentFormation.formationName)
        {
            case "SpreadFormation":
            case "SquareFormation":
                if (index > 4)
                {
                    index %= 4;
                }
                break;
            case "ColumnFormation":
                if (index > 1)
                {
                    index = 1;
                }    
                break;
            default:
                if (index > 8)
                {
                    index %= 8;
                }
                break;
        }       
        // ���� ������ �޾ƿ��� ���� ������ �ѹ��� �޾ư���?
        // ���簡 �˾ƾ� �Ұ� ���� �հ� �� ������ ������ ������ �ִ�.
        // �������� ���° �ε������� �ٽ� ���° �ε����� ���ư��� �Ǵ���
        // �⺻������ �������� �����ϰ�� �д�� 4��° (���ٿ� 8��) �ι�° �ٱ��� �ؼ� 
        // �����ѵ� 0���� �ƴ϶� 1��° �ε������� �ٽ� �����ϵ���
        // �������� ����� /2 ��ŭ�� �ε�����ŭ ���� ������ �Ѵ�. Ȧ���ų� �׸��� ����� ������ ���� ���� ���� ���� ���ؼ� ������
        // ��ĭ ������ �׸��� �밢�� ���� �ٸ��� �ٷ� �밢�� ������ �Ѵ�
        if (squard1)
        {
            return MaintainFormation(currentFormation.squard1FormationLink[index]);
        }
        else
        {
            return MaintainFormation(currentFormation.squard2FormationLink[index]);
        }
    }
    public Vector3 MaintainFormation(Direction direction)
    {
        switch (direction)
        {
            case Direction.forward:
                return Vector3.forward;
            case Direction.leftForward:
                return Vector3.forward + Vector3.left;
            case Direction.rightForward:
                return Vector3.forward + Vector3.right;
            case Direction.left:
                return Vector3.left;
            case Direction.right:
                return Vector3.right;
            case Direction.backward:
                return -Vector3.forward;
            case Direction.leftBackward:
                return -Vector3.forward + Vector3.left;
            case Direction.rightBackward:
                return -Vector3.forward + Vector3.right;
            default:
                return Vector3.zero;
        }
    }
    [Serializable]
    public class FormationInfo
    {
        public string formationName;
        public Direction[] squard1FormationLink;
        public Direction[] squard2FormationLink;
        
    }
}
