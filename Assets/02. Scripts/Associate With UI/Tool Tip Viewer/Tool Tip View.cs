using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipView : MonoBehaviour, IToolTipView
{
    [Header("UI 관련 컴포넌트")]
    [Header("캔버스")]
    [SerializeField] private Canvas m_canvas;

    [Header("툴팁 UI")]
    [SerializeField] private GameObject m_tooltip_obj;

    [Header("아이템 이미지")]
    [SerializeField] private Image m_item_image;

    [Header("아이템 이름")]
    [SerializeField] private TMP_Text m_name_label;

    [Header("아이템 타입")]
    [SerializeField] private TMP_Text m_type_label;

    [Header("아이템 설명")]
    [SerializeField] private TMP_Text m_desc_label;

    private ToolTipPresenter m_presenter;

    private void Update()
    {
        if (m_tooltip_obj.activeInHierarchy)
        {
            CalculateMousePosition();
        }
    }

    public void Inject(ToolTipPresenter presenter)
    {
        m_presenter = presenter;
    }

    public void OpenUI()
    {
        m_tooltip_obj.SetActive(true);
    }

    public void UpdateUI(Sprite image, string name, string type, string description)
    {
        m_item_image.sprite = image;
        SetAlpha(1f);

        m_name_label.text = name;
        m_type_label.text = type;
        m_desc_label.text = description;
    }

    public void CloseUI()
    {
        Clear();
        m_tooltip_obj.SetActive(false);
    }

    private void Clear()
    {
        m_item_image.sprite = null;
        SetAlpha(0f);

        m_name_label.text = string.Empty;
        m_type_label.text = string.Empty;
        m_desc_label.text = string.Empty;
    }

    private void SetAlpha(float alpha)
    {
        var color = m_item_image.color;
        color.a = alpha;
        m_item_image.color = color;
    }

    private void CalculateMousePosition()
    {
        var mouse_position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        var canvas_transform = m_canvas.transform as RectTransform;
        var rect_transform = m_tooltip_obj.transform as RectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas_transform,
            mouse_position,
            null,
            out var local_position
        );

        if (mouse_position.x > Screen.width * 0.5f)
        {
            local_position.x -= rect_transform.sizeDelta.x / 2;
        }
        else
        {
            local_position.x += rect_transform.sizeDelta.x / 2;
        }

        if (mouse_position.y < Screen.height * 0.5f)
        {
            local_position.y += rect_transform.sizeDelta.y / 2;
        }
        else
        {
            local_position.y -= rect_transform.sizeDelta.y / 2;
        }

        rect_transform.anchoredPosition = local_position;
    }
}
