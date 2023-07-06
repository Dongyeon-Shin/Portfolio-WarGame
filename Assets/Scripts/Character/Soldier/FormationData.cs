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
        // 병사 개인이 받아오지 말고 리더가 한번에 받아가기?
        // 병사가 알아야 할건 방향 앞과 뒤 병사의 정보는 가지고 있다.
        // 대형마다 몇번째 인덱스에서 다시 몇번째 인덱스로 돌아가면 되는지
        // 기본적으로 원형진을 제외하고는 분대당 4명째 (한줄에 8명) 두번째 줄까지 해서 
        // 진행한뒤 0번이 아니라 1번째 인덱스에서 다시 시작하도록
        // 원형진은 병사수 /2 만큼의 인덱스만큼 정상 진행을 한다. 홀수거나 그리고 진행된 병사의 수가 남은 병사 수와 비교해서 같으면
        // 한칸 밑으로 그리고 대각선 시작 다르면 바로 대각선 시작을 한다
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
