using System.Numerics;
using InventoryService;

public class ItemSlotPresenter
{
    private readonly IItemSlotView m_view;
    private readonly IInventoryService m_inventory_service;
    private ItemDataBase m_item_db;

    private ToolTipPresenter m_tooltip_presenter;
    private DragSlotPresenter m_drag_slot_presenter;

    private int m_offset;
    private SlotType m_slot_type;

    public ItemSlotPresenter(IItemSlotView view,
                             IInventoryService inventory_service,
                             ItemDataBase item_db,
                             ToolTipPresenter tooltip_presenter,
                             DragSlotPresenter drag_slot_presenter,
                             int offset,
                             SlotType slot_type = SlotType.Inventory)
    {
        m_view = view;
        m_inventory_service = inventory_service;
        m_item_db = item_db;

        m_offset = offset;

        m_tooltip_presenter = tooltip_presenter;
        m_drag_slot_presenter = drag_slot_presenter;

        m_slot_type = slot_type;

        if (m_slot_type == SlotType.Inventory)
        {
            m_inventory_service.OnUpdatedSlot += UpdateSlot;
        }

        m_view.Inject(this);
    }

    public void UpdateSlot(int offset, ItemData item_data)
    {
        if (m_offset == offset)
        {
            if (item_data.Code == ItemCode.NONE)
            {
                m_view.ClearUI();
            }
            else
            {
                var item = m_item_db.GetItem(item_data.Code);

                m_view.UpdateUI(item.Sprite, item.Stackable, item_data.Count);
            }
        }
    }

    private ItemData GetItemData(SlotType slot_type, int offset)
    {
        switch (slot_type)
        {
            case SlotType.Inventory:
                return m_inventory_service.GetItem(offset);

            default:
                return null;
        }
    }

    private void ChangeSlot()
    {
        var draged_item_data = m_drag_slot_presenter.GetItemData();
        var draged_item = m_drag_slot_presenter.GetItem();

        var current_item_data = GetItemData(m_slot_type, m_offset);

        if (CheckNonEquipment(draged_item, draged_item_data, current_item_data))
        {
            return;
        }

        if (CheckShift(draged_item_data, current_item_data))
        {
            return;
        }

        if (CheckCtrl(draged_item_data, current_item_data))
        {
            return;
        }

        SwapSlot(draged_item_data, current_item_data);
    }

    private bool CheckNonEquipment(Item draged_item, ItemData draged_item_data, ItemData current_item_data)
    {
        if (draged_item.Type < ItemType.Equipment_Helmet)
        {
            if (current_item_data.Code != ItemCode.NONE && draged_item_data.Code == current_item_data.Code)
            {
                var changed_slot_count = 0;
                if (m_drag_slot_presenter.Mode == DragMode.SHIFT)
                {
                    changed_slot_count = (int)(draged_item_data.Count * 0.5f);
                    if (changed_slot_count == 0)
                    {
                        UpdateItem(m_offset, 1);
                        m_drag_slot_presenter.Clear();

                        return true;
                    }
                }
                else if (m_drag_slot_presenter.Mode == DragMode.CTRL)
                {
                    changed_slot_count = 1;
                    if (draged_item_data.Count == 1)
                    {
                        UpdateItem(m_offset, 1);
                        m_drag_slot_presenter.Clear();

                        return true;
                    }
                }
                else
                {
                    changed_slot_count = draged_item_data.Count;

                    UpdateItem(m_offset, changed_slot_count);
                    m_drag_slot_presenter.Clear();

                    return true;
                }

                UpdateItem(m_offset, changed_slot_count);
                m_drag_slot_presenter.Updates(-changed_slot_count);

                return true;
            }
        }

        return false;
    }

    private bool CheckShift(ItemData draged_item_data, ItemData current_item_data)
    {
        if (m_drag_slot_presenter.Mode != DragMode.SHIFT)
        {
            return false;
        }

        var changed_slot_count = (int)(draged_item_data.Count * 0.5f);
        if (changed_slot_count == 0)
        {
            if (current_item_data.Code == ItemCode.NONE)
            {
                m_inventory_service.SetItem(m_offset, draged_item_data.Code, 1);
            }
            else
            {
                UpdateItem(m_offset, 1);
            }
            m_drag_slot_presenter.Clear();

            return true;
        }

        if (current_item_data.Code == ItemCode.NONE)
        {
            m_inventory_service.SetItem(m_offset, draged_item_data.Code, changed_slot_count);
        }
        else
        {
            UpdateItem(m_offset, changed_slot_count);
        }
        m_drag_slot_presenter.Updates(-changed_slot_count);

        return true;
    }

    private bool CheckCtrl(ItemData draged_item_data, ItemData current_item_data)
    {
        if (m_drag_slot_presenter.Mode != DragMode.CTRL)
        {
            return false;
        }

        if (current_item_data.Code == ItemCode.NONE)
        {
            m_inventory_service.SetItem(m_offset, draged_item_data.Code, 1);
        }
        else
        {
            UpdateItem(m_offset, 1);
        }

        if (draged_item_data.Count == 1)
        {
            m_drag_slot_presenter.Clear();
        }
        else
        {
            m_drag_slot_presenter.Updates(-1);
        }

        return true;
    }

    private void SwapSlot(ItemData draged_item_data, ItemData current_item_data)
    {
        var temp_data = new ItemData(current_item_data.Code, current_item_data.Count);

        m_inventory_service.SetItem(m_offset, draged_item_data.Code, draged_item_data.Count);

        if (temp_data.Code != ItemCode.NONE)
        {
            m_drag_slot_presenter.Set(temp_data.Code, temp_data.Count);
        }
        else
        {
            m_drag_slot_presenter.Clear();
        }
    }

    private void UpdateItem(int offset, int count)
    {
        switch (m_slot_type)
        {
            case SlotType.Inventory:
                m_inventory_service.UpdateItem(offset, count);
                break;

            case SlotType.Equipment:
                break;

            case SlotType.Shortcut:
                break;
        }
    }

    public void OnPointerEnter()
    {
        var code = m_inventory_service.GetItem(m_offset).Code;
        if (code == ItemCode.NONE)
        {
            return;
        }

        m_tooltip_presenter.OpenUI(code);
    }

    public void OnPointerExit()
    {
        m_tooltip_presenter.CloseUI();
    }

    public void OnBeginDrag(Vector2 mouse_position, DragMode drag_mode)
    {
        var item_data = GetItemData(m_slot_type, m_offset);
        if (item_data == null || item_data.Code == ItemCode.NONE)
        {
            return;
        }

        m_tooltip_presenter.CloseUI();
        m_drag_slot_presenter.OpenUI(m_slot_type, m_offset, drag_mode);
        m_drag_slot_presenter.SetPosition(mouse_position);
    }

    public void OnDrag(Vector2 mouse_position)
    {
        var item_data = GetItemData(m_slot_type, m_offset);
        if (item_data == null || item_data.Code == ItemCode.NONE)
        {
            return;
        }

        m_drag_slot_presenter.SetPosition(mouse_position);
    }

    public void OnEndDrag()
    {
        m_drag_slot_presenter.CloseUI();
    }

    public void OnDrop()
    {
        var item = m_drag_slot_presenter.GetItem();
        if (item.Code == ItemCode.NONE)
        {
            return;
        }

        if (m_drag_slot_presenter.Mode == DragMode.SHIFT || m_drag_slot_presenter.Mode == DragMode.CTRL)
        {
            var draged_item_data = m_drag_slot_presenter.GetItemData();
            var current_item_data = GetItemData(m_slot_type, m_offset);

            if (current_item_data.Code != ItemCode.NONE && current_item_data.Code != draged_item_data.Code)
            {
                return;
            }
        }

        if (!m_view.IsMask(item.Type))
            {
                return;
            }

        ChangeSlot();
    }
}