using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FormationData", menuName = "Data/Formation")]
public class CombatFormation : ScriptableObject
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

    [Serializable]
    public class FormationInfo
    {
        public string formationName;
        [SerializeField]
        private Direction[] squard1FormationLink;
        [SerializeField]
        private Direction[] squard2FormationLink;
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
    }
}
