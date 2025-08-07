using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cursor DataBase", menuName = "SO/DB/Cursor DataBase")]
public class CursorDataBase : ScriptableObject, ICursorDataBase
{
    [Header("커서 데이터의 목록")]
    [SerializeField] private List<CursorData> m_cursor_datas;

    private Dictionary<CursorMode, CursorData> m_cursor_dict;
    private CursorMode m_current_mode;

#if UNITY_EDITOR
    private void OnEnable()
    {
        Initialize();
    }
#endif

    private void Initialize()
    {
        if (m_cursor_dict != null)
        {
            return;
        }

        m_cursor_dict = new();

        if (m_cursor_datas == null || m_cursor_datas.Count.Equals(0))
        {
            return;
        }

        foreach (var data in m_cursor_datas)
        {
            m_cursor_dict.TryAdd(data.Mode, data);
        }
    }

    public void SetCursor(CursorMode mode)
    {
        if (m_cursor_dict == null)
        {
            Initialize();
        }

        if (m_current_mode.Equals(mode))
        {
            return;
        }

        if (m_cursor_dict.TryGetValue(mode, out var data))
        {
            var pivot = new Vector2(data.Cursor.width * data.Hotspot.x, data.Cursor.height * data.Hotspot.y);
            Cursor.SetCursor(data.Cursor, pivot, UnityEngine.CursorMode.Auto);

            m_current_mode = mode;
        }
    }
}
