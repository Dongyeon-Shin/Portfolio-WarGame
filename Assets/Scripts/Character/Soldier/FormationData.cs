using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    public void Setting()
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
    public void GetFirstSquadFormation(FormationInfo currentFormation, int squadSize, ref Queue<Vector3> formation)
    {
        formation.Clear();
        switch (currentFormation.formationName)
        {
            case "SpreadFormation":
            case "SquareFormation":
                for (int i = 0; i < squadSize; i++)
                {
                    if (i > 4)
                    {
                        formation.Enqueue(TranslateFormationInfo(currentFormation.squard1FormationLink[i % 4]));
                    }
                    else
                    {
                        formation.Enqueue(TranslateFormationInfo(currentFormation.squard1FormationLink[i]));
                    }                  
                }                
                break;
            case "ColumnFormation":
                for (int i = 0; i < squadSize; i++)
                {
                    if (i > 1)
                    {
                        formation.Enqueue(TranslateFormationInfo(currentFormation.squard1FormationLink[1]));
                    }
                    else
                    {
                        formation.Enqueue(TranslateFormationInfo(currentFormation.squard1FormationLink[i]));
                    }
                }
                break;
            default:
                for (int i = 0; i < squadSize; i++)
                {
                    if (i > 8)
                    {
                        formation.Enqueue(TranslateFormationInfo(currentFormation.squard1FormationLink[i % 8]));
                    }
                    else
                    {
                        formation.Enqueue(TranslateFormationInfo(currentFormation.squard1FormationLink[i]));
                    }
                }
                break;
        }
    }
    public void GetSecondSquadFormation(FormationInfo currentFormation, int squadSize, ref Queue<Vector3> formation)
    {
        formation.Clear();
        switch (currentFormation.formationName)
        {
            case "SpreadFormation":
            case "SquareFormation":
                for (int i = 0; i < squadSize; i++)
                {
                    if (i > 4)
                    {
                        formation.Enqueue(TranslateFormationInfo(currentFormation.squard2FormationLink[i % 4]));
                    }
                    else
                    {
                        formation.Enqueue(TranslateFormationInfo(currentFormation.squard2FormationLink[i]));
                    }
                }
                break;
            case "ColumnFormation":
                for (int i = 0; i < squadSize; i++)
                {
                    if (i > 1)
                    {
                        formation.Enqueue(TranslateFormationInfo(currentFormation.squard2FormationLink[1]));
                    }
                    else
                    {
                        formation.Enqueue(TranslateFormationInfo(currentFormation.squard2FormationLink[i]));
                    }
                }
                break;
            default:
                for (int i = 0; i < squadSize; i++)
                {
                    if (i > 8)
                    {
                        formation.Enqueue(TranslateFormationInfo(currentFormation.squard2FormationLink[i % 8]));
                    }
                    else
                    {
                        formation.Enqueue(TranslateFormationInfo(currentFormation.squard2FormationLink[i]));
                    }
                }
                break;
        }
    }
    public Vector3 TranslateFormationInfo(Direction direction)
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