using UnityEngine;

public class EnemyMouseDetector : MouseDetector
{
    protected override void OnMouseEnter()
    {
        SetCursor(CursorMode.CAN_ATTACK);
    }
}
