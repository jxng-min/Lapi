using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotView : MonoBehaviour, IItemSlotView
{
    [Header("아이템 이미지")]
    [SerializeField] private Image m_item_image;

    [Header("아이템 개수")]
    [SerializeField] private TMP_Text m_count_label;

    [Header("쿨타임 이미지")]
    [SerializeField] private Image m_cooldown_image;

    [Header("슬롯 마스크")]
    [SerializeField] private ItemType m_slot_type;

    private ItemSlotPresenter m_presenter;

    public void Inject(ItemSlotPresenter presenter)
    {
        m_presenter = presenter;
    }

    public void ClearUI()
    {
        m_item_image.sprite = null;
        SetAlpha(0f);

        m_count_label.text = string.Empty;
        m_count_label.gameObject.SetActive(false);

        m_cooldown_image.gameObject.SetActive(false);
    }

    public void UpdateUI(Sprite item_image, bool stackable, int count)
    {
        m_item_image.sprite = item_image;
        SetAlpha(1f);

        if (stackable)
        {
            m_count_label.gameObject.SetActive(true);
            m_count_label.text = NumberFormatter.FormatNumber(count);
        }

        m_cooldown_image.gameObject.SetActive(false);
    }

    public bool IsMask(ItemType type)
    {
        return ((int)m_slot_type & (int)type) != 0;
    }

    private void SetAlpha(float alpha)
    {
        var color = m_item_image.color;
        color.a = alpha;
        m_item_image.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_presenter.OnPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_presenter.OnPointerExit();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DragMode drag_mode;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            drag_mode = DragMode.SHIFT;
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            drag_mode = DragMode.CTRL;
        }
        else
        {
            drag_mode = DragMode.DEFAULT;
        }

        var mouse_position = new System.Numerics.Vector2(eventData.position.x, eventData.position.y);
        m_presenter.OnBeginDrag(mouse_position, drag_mode);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var mouse_position = new System.Numerics.Vector2(eventData.position.x, eventData.position.y);
        m_presenter.OnDrag(mouse_position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_presenter.OnEndDrag();
    }

    public void OnDrop(PointerEventData eventData)
    {
        m_presenter.OnDrop();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
