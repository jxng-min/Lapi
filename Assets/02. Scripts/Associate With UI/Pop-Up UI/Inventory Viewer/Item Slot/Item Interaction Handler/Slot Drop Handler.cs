using InventoryService;

public class SlotDropHandler
{
    private IInventoryService m_inventory_service;

    private IItemSlotContext m_slot_context;
    private ToolTipPresenter m_tooltip_presenter;
    private DragSlotPresenter m_drag_slot_presenter;

    private ICursorDataBase m_cursor_db;

    private SlotType m_slot_type;
    private int m_offset;

    private bool IsShopOrCraft => m_slot_type == SlotType.Shop || m_slot_type == SlotType.Craft;

    public SlotDropHandler(IInventoryService inventory_service,
                           IItemSlotContext slot_context,
                           ToolTipPresenter tooltip_presenter,
                           DragSlotPresenter drag_slot_presenter,
                           ICursorDataBase cursor_db)
    {
        m_inventory_service = inventory_service;
        m_slot_context = slot_context;
        m_tooltip_presenter = tooltip_presenter;
        m_drag_slot_presenter = drag_slot_presenter;
        m_cursor_db = cursor_db;
    }

    public void OnDrop(SlotType slot_type, int offset, bool masking)
    {
        m_cursor_db.SetCursor(CursorMode.DEFAULT);

        var item = m_drag_slot_presenter.GetItem();
        if (item.Code == ItemCode.NONE)
        {
            return;
        }

        if (IsShopOrCraft)
        {
            return;
        }

        m_slot_type = slot_type;
        m_offset = offset;

        if (m_drag_slot_presenter.Mode == DragMode.SHIFT || m_drag_slot_presenter.Mode == DragMode.CTRL)
        {
            if (m_drag_slot_presenter.Type == SlotType.Shortcut)
            {
                return;
            }

            var draged_item_data = m_drag_slot_presenter.GetItem();
            var current_item_data = m_slot_context.Get(m_slot_type, m_offset);

            if (current_item_data.Code != ItemCode.NONE && current_item_data.Code != draged_item_data.Code)
            {
                return;
            }
        }

        if (m_drag_slot_presenter.Type == SlotType.Shortcut)
        {
            if (m_slot_type != SlotType.Shortcut)
            {
                return;
            }
        }

        if (!masking)
        {
            return;
        }

        ChangeSlot();

        if (m_slot_type != SlotType.Skill)
        {
            m_tooltip_presenter.OpenUI(item.Code);
        }
    }

    private void ChangeSlot()
    {
        var draged_item_data = m_drag_slot_presenter.GetItemData();
        var draged_item = m_drag_slot_presenter.GetItem();

        var current_item_data = m_slot_context.Get(m_slot_type, m_offset);

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
        if (m_slot_type == SlotType.Shortcut)
        {
            return false;
        }

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
                        var result = m_inventory_service.UpdateItem(m_offset, 1);
                        if (result == -1)
                        {
                            m_drag_slot_presenter.Clear();
                        }
                        else
                        {
                            m_drag_slot_presenter.Updates(-result);
                        }
                        m_inventory_service.InitializeSlot(m_offset);

                        return true;
                    }
                }
                else if (m_drag_slot_presenter.Mode == DragMode.CTRL)
                {
                    changed_slot_count = 1;
                    if (draged_item_data.Count == 1)
                    {
                        var result = m_inventory_service.UpdateItem(m_offset, 1);
                        if (result == -1)
                        {
                            m_drag_slot_presenter.Clear();
                        }
                        else
                        {
                            m_drag_slot_presenter.Updates(-result);
                        }
                        m_inventory_service.InitializeSlot(m_offset);

                        return true;
                    }
                }
                else
                {
                    changed_slot_count = draged_item_data.Count;

                    var result = m_inventory_service.UpdateItem(m_offset, changed_slot_count);
                    if (result == -1)
                    {
                        m_drag_slot_presenter.Clear();
                    }
                    else
                    {
                        m_drag_slot_presenter.Updates(-result);
                    }
                    m_inventory_service.InitializeSlot(m_offset);

                    return true;
                }

                var final_result = m_inventory_service.UpdateItem(m_offset, changed_slot_count);
                m_drag_slot_presenter.Updates(final_result != -1 ? -final_result : -changed_slot_count);
                m_inventory_service.InitializeSlot(m_offset);

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
                m_slot_context.Update(m_slot_type, m_offset, 1);
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
            m_slot_context.Update(m_slot_type, m_offset, changed_slot_count);
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
            m_slot_context.Update(m_slot_type, m_offset, 1);
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

        if (m_slot_type != SlotType.Shortcut)
        {
            m_slot_context.Set(m_slot_type, m_offset, draged_item_data.Code, draged_item_data.Count);

            if (temp_data.Code != ItemCode.NONE)
            {
                m_drag_slot_presenter.Set(temp_data.Code, temp_data.Count);
            }
            else
            {
                m_drag_slot_presenter.Clear();
            }

            m_inventory_service.InitializeSlot(m_offset);
        }
        else
        {
            m_slot_context.Set(m_slot_type, m_offset, draged_item_data.Code, m_inventory_service.GetItemCount(draged_item_data.Code));

            if (m_drag_slot_presenter.Type == SlotType.Shortcut)
            {
                m_drag_slot_presenter.Set(temp_data.Code, m_inventory_service.GetItemCount(temp_data.Code));
            }
        }
    }
}