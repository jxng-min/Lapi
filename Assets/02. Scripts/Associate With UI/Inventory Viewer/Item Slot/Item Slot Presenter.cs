using System.Numerics;
using EquipmentService;
using InventoryService;
using ShortcutService;
using SkillService;
using UnityEngine.EventSystems;

public class ItemSlotPresenter
{
    private readonly IItemSlotView m_view;
    private readonly IInventoryService m_inventory_service;
    private readonly IEquipmentService m_equipment_service;
    private readonly ISkillService m_skill_service;
    private readonly IShortcutService m_shortcut_service;
    private IItemDataBase m_item_db;

    private ToolTipPresenter m_tooltip_presenter;
    private DragSlotPresenter m_drag_slot_presenter;

    private IItemActivator m_item_activator;
    private IItemCooler m_item_cooler;

    private int m_offset;
    private SlotType m_slot_type;

    public ItemSlotPresenter(IItemSlotView view,
                             IInventoryService inventory_service,
                             IEquipmentService equipment_service,
                             ISkillService skill_service,
                             IShortcutService shortcut_service,
                             IItemDataBase item_db,
                             ToolTipPresenter tooltip_presenter,
                             DragSlotPresenter drag_slot_presenter,
                             IItemActivator item_activator,
                             IItemCooler item_cooler,
                             int offset,
                             SlotType slot_type = SlotType.Inventory)
    {
        m_view = view;
        m_inventory_service = inventory_service;
        m_equipment_service = equipment_service;
        m_skill_service = skill_service;
        m_shortcut_service = shortcut_service;
        m_item_db = item_db;

        m_offset = offset;

        m_tooltip_presenter = tooltip_presenter;
        m_drag_slot_presenter = drag_slot_presenter;

        m_item_activator = item_activator;
        m_item_cooler = item_cooler;

        m_slot_type = slot_type;

        if (m_slot_type == SlotType.Inventory)
        {
            m_inventory_service.OnUpdatedSlot += UpdateSlot;
        }
        else if (m_slot_type == SlotType.Equipment)
        {
            m_equipment_service.OnUpdatedSlot += UpdateSlot;
        }
        else if (m_slot_type == SlotType.Skill)
        {
            m_skill_service.OnUpdatedSlot += UpdateSlot;
        }
        else
        {
            m_shortcut_service.OnUpdatedSlot += UpdateSlot;
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

            case SlotType.Equipment:
                return m_equipment_service.GetItem(offset);

            case SlotType.Skill:
                return m_skill_service.GetSkill(offset);

            case SlotType.Shortcut:
                return m_shortcut_service.GetItem(offset);

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

        if (m_slot_type != SlotType.Shortcut)
        {
            SetItem(m_offset, draged_item_data.Code, draged_item_data.Count);

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
            SetItem(m_offset, draged_item_data.Code, m_inventory_service.GetItemCount(draged_item_data.Code));

            if (m_drag_slot_presenter.Type == SlotType.Shortcut)
            {
                m_drag_slot_presenter.Set(temp_data.Code, m_inventory_service.GetItemCount(temp_data.Code));
            }
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

    private void SetItem(int offset, ItemCode code, int count)
    {
        switch (m_slot_type)
        {
            case SlotType.Inventory:
                m_inventory_service.SetItem(offset, code, count);
                break;

            case SlotType.Equipment:
                m_equipment_service.SetItem(offset, code);
                break;

            case SlotType.Shortcut:
                m_shortcut_service.SetItem(offset, code);
                break;
        }
    }

    private ItemData GetItem(int offset)
    {
        switch (m_slot_type)
        {
            case SlotType.Inventory:
                return m_inventory_service.GetItem(offset);

            case SlotType.Equipment:
                return m_equipment_service.GetItem(offset);

            case SlotType.Shortcut:
                return m_shortcut_service.GetItem(offset);

            case SlotType.Skill:
                return m_skill_service.GetSkill(offset);
        }

        return null;
    }

    private void Clear(int offset)
    {
        switch (m_slot_type)
        {
            case SlotType.Inventory:
                m_inventory_service.Clear(offset);
                break;

            case SlotType.Equipment:
                m_equipment_service.Clear(offset);
                break;
        }        
    }

    public void UseItem()
    {
        var code = GetItem(m_offset).Code;
        if (code == ItemCode.NONE)
        {
            return;
        }

        if (m_item_cooler.GetCool(code) > 0f)
        {
            return;
        }

        if (m_slot_type == SlotType.Skill)
        {
            var item_data = GetItemData(m_slot_type, m_offset);
            if (item_data.Count <= 0)
            {
                return;
            }
        }

        if (m_slot_type == SlotType.Shortcut)
        {
            return;
        }

        m_tooltip_presenter.CloseUI();

        var item = m_item_db.GetItem(code);
        if (!m_item_activator.UseItem(item, m_offset, m_slot_type))
        {
            return;
        }

        if (item.Cool > 0f)
        {
            if (m_slot_type == SlotType.Skill)
            {
                var skill_level = m_skill_service.GetSkillLevel(code);

                var final_cool = item.Cool + ((skill_level - 1) * (item as SkillItem).GrowthCool);

                 m_item_cooler.Push(code, final_cool);
            }
            else
            {
                m_item_cooler.Push(code, item.Cool);
            }
        }

        if (item.Type == ItemType.Consumable)
        {
            var count = GetItem(m_offset).Count;
            if (count > 1)
            {
                UpdateItem(m_offset, -1);
            }
            else
            {
                Clear(m_offset);
            }
        }
    }

    public bool IsEmpty()
    {
        return GetItem(m_offset).Code == ItemCode.NONE;
    }

    public float GetCool()
    {
        var code = GetItem(m_offset).Code;
        var item = m_item_db.GetItem(code);

        return m_item_cooler.GetCool(code) / item.Cool;
    }

    public void OnPointerEnter()
    {
        var code = GetItem(m_offset).Code;
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

        if (m_slot_type == SlotType.Skill)
        {
            var count = GetItemData(m_slot_type, m_offset).Count;
            if (count <= 0)
            {
                return;
            }
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

        if (m_slot_type == SlotType.Skill)
        {
            var count = GetItemData(m_slot_type, m_offset).Count;
            if (count <= 0)
            {
                return;
            }
        }

        m_drag_slot_presenter.SetPosition(mouse_position);
    }

    public void OnEndDrag()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (m_drag_slot_presenter.Type == SlotType.Shortcut)
            {
                m_drag_slot_presenter.Clear();
            }
            
        }

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
            if (m_drag_slot_presenter.Type == SlotType.Shortcut)
            {
                return;
            }

            var draged_item_data = m_drag_slot_presenter.GetItemData();
            var current_item_data = GetItemData(m_slot_type, m_offset);

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

        if (!m_view.IsMask(item.Type))
        {
            return;
        }

        ChangeSlot();

        if (m_slot_type != SlotType.Skill)
        {
            m_tooltip_presenter.OpenUI(item.Code);
        }
    }
}