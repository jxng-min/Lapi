using UnityEngine;
using UnityEngine.UI;

public class DragSlotView : MonoBehaviour, IDragSlotView
{
    [Header("아이템 이미지")]
    [SerializeField] private Image m_item_image;

    public void OpenUI(Sprite item_image)
    {
        (transform as RectTransform).SetAsLastSibling();

        m_item_image.sprite = item_image;
        SetAlpha(1f);
    }

    public void CloseUI()
    {
        m_item_image.sprite = null;
        SetAlpha(0f);
    }

    public void SetPosition(System.Numerics.Vector2 mouse_position)
    {
        Vector2 target_position = new Vector2(mouse_position.X, mouse_position.Y);
        transform.position = target_position;
    }

    private void SetAlpha(float alpha)
    {
        var color = m_item_image.color;
        color.a = alpha;
        m_item_image.color = color;
    }
}
