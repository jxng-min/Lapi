using UnityEngine.EventSystems;

public class EnemyMouseDetector : MouseDetector
{
    protected override void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        SetCursor(CursorMode.CAN_ATTACK);
    }
}
