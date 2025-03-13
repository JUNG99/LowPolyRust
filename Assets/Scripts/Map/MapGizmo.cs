using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGizmo : MonoBehaviour
{
    // 색상 옵션을 위한 enum 정의
    public enum PredefinedColor
    {
        Red,
        Green,
        Blue,
        Yellow,
    }

    public PredefinedColor gizmoColorChoice = PredefinedColor.Green;
    public Vector3 gizmoSize;

    private Color GetColorFromEnum(PredefinedColor colorEnum)
    {
        switch (colorEnum)
        {
            case PredefinedColor.Red:
                return Color.red;
            case PredefinedColor.Green:
                return Color.green;
            case PredefinedColor.Blue:
                return Color.blue;
            case PredefinedColor.Yellow:
                return Color.yellow;
            default:
                return Color.green;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GetColorFromEnum(gizmoColorChoice);
        Gizmos.DrawWireCube(transform.position, gizmoSize);
    }
}
