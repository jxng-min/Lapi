using UnityEngine;

public abstract class MouseDetector : MonoBehaviour
{
    private ICursorDataBase m_cursor_db;

    public void Inject(ICursorDataBase cursor_db)
    {
        m_cursor_db = cursor_db;
    }

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
