using UnityEngine.EventSystems;

public class SlotDragHandler
{
    private IItemSlotContext m_slot_context;
    private ToolTipPresenter m_tooltip_presenter;
    private DragSlotPresenter m_drag_slot_presenter;

    private ICursorDataBase m_cursor_db;

    private SlotType m_slot_type;
    private int m_offset;

    private bool IsShopOrCraft => m_slot_type == SlotType.Shop || m_slot_type == SlotType.Craft;

    public SlotDragHandler(IItemSlotContext slot_context,
                           ToolTipPresenter tooltip_presenter,
                           DragSlotPresenter drag_slot_presenter,
                           ICursorDataBase cursor_db)
    {
        m_slot_context = slot_context;
        m_tooltip_presenter = tooltip_presenter;
        m_drag_slot_presenter = drag_slot_presenter;
        m_cursor_db = cursor_db;
    }

    public void OnBeginDrag(System.Numerics.Vector2 mouse_position, DragMode drag_mode, SlotType slot_type, int offset)
    {
        m_slot_type = slot_type;
        m_offset = offset;

        if (!CanDrag())
        {
            return;
        }

        m_tooltip_presenter.CloseUI();
        m_drag_slot_presenter.OpenUI(m_slot_type, m_offset, drag_mode);
        m_drag_slot_presenter.SetPosition(mouse_position);

        m_cursor_db.SetCursor(CursorMode.GRAB);
        SoundManager.Instance.PlaySFX("Default");
    }

    public void OnDrag(System.Numerics.Vector2 mouse_position)
    {
        if (!CanDrag())
        {
            return;
        }

        m_drag_slot_presenter.SetPosition(mouse_position);

        m_cursor_db.SetCursor(CursorMode.GRAB);
    }

    public void OnEndDrag()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (m_drag_slot_presenter.Type == SlotType.Shortcut)
            {
                m_drag_slot_presenter.Clear();
            }
            m_cursor_db.SetCursor(CursorMode.DEFAULT);
        }
        else
        {
            m_cursor_db.SetCursor(CursorMode.CAN_GRAB);
        }

        m_drag_slot_presenter.CloseUI();
    }
    
    private bool CanDrag()
    {
        var item_data = m_slot_context.Get(m_slot_type, m_offset);
        if (item_data == null || item_data.Code == ItemCode.NONE)
        {
            return false;
        }

        if (IsShopOrCraft)
        {
            return false;
        }

        if (m_slot_type == SlotType.Skill)
        {
            var count = m_slot_context.Get(m_slot_type, m_offset).Count;
            if (count <= 0)
            {
                return false;
            }
        }

        return true;
    }
}
