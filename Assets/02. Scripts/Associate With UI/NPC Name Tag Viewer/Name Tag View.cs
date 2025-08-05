using TMPro;
using UnityEngine;

public class NameTagView : MonoBehaviour, INameTagView
{
    [Header("UI 관련 컴포넌트")]
    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    [Header("이름")]
    [SerializeField] private TMP_Text m_name_label;

    public void OpenUI(string name, System.Numerics.Vector2 position)
    {
        transform.position = new Vector2(position.X, position.Y);
        
        m_canvas_group.alpha = 1f;
        m_name_label.text = name;
    }

    public void CloseUI()
    {
        m_canvas_group.alpha = 0f;
        m_name_label.text = string.Empty;
    }
}
