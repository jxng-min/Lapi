using UnityEngine;

public abstract class MouseDetector : MonoBehaviour
{
    [Header("커서 데이터베이스")]
    [SerializeField] private CursorDataBase m_cursor_db;

    protected void SetCursor(CursorMode mode)
    {
        m_cursor_db.SetCursor(mode);
    }

    protected abstract void OnMouseEnter();

    protected virtual void OnMouseExit()
    {
        SetCursor(CursorMode.DEFAULT);
    }
}
