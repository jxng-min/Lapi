using UnityEngine;

public class MouseRaycasting : MonoBehaviour
{
    [Header("커서 데이터베이스")]
    [SerializeField] private CursorDataBase m_cursor_db;

    private void Update()
    {
        CheckObject();
        InteractionObject();
    }

    private void CheckObject()
    {
        Vector2 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mouse_position, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("NPC"))
            {
                m_cursor_db.SetCursor(CursorMode.CAN_TALK);
            }
            else if (hit.collider.CompareTag("ENEMY"))
            {
                m_cursor_db.SetCursor(CursorMode.CAN_ATTACK);
            }
        }
        else
        {
            m_cursor_db.SetCursor(CursorMode.DEFAULT);
        }
    }

    private void InteractionObject()
    {

    }
}