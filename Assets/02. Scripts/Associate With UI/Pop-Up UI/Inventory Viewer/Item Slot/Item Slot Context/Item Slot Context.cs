using System;
using EquipmentService;
using InventoryService;
using ShortcutService;
using SkillService;

public class ItemSlotContext : IItemSlotContext
{
    private readonly IItemDataBase m_item_db;

    private readonly IInventoryService m_inventory_service;
    private readonly IEquipmentService m_equipment_service;
    private readonly ISkillService m_skill_service;
    private readonly IShortcutService m_shortcut_service;

    public ItemSlotContext(IItemDataBase item_db,
                           IInventoryService inventory_service,
                           IEquipmentService equipment_service,
                           ISkillService skill_service,
                           IShortcutService shortcut_service)
    {
        m_item_db = item_db;
        m_inventory_service = inventory_service;
        m_equipment_service = equipment_service;
        m_skill_service = skill_service;
        m_shortcut_service = shortcut_service;
    }

    public void Register(SlotType slot_type, Action<int, ItemData> update_action, int offset = 0, int count = 0)
    {
        switch (slot_type)
        {
            case SlotType.Inventory:
                m_inventory_service.OnUpdatedSlot += update_action;
                break;

            case SlotType.Equipment:
                m_equipment_service.OnUpdatedSlot += update_action;
                break;

            case SlotType.Skill:
                m_skill_service.OnUpdatedSlot += update_action;
                break;

            case SlotType.Shortcut:
                m_shortcut_service.OnUpdatedSlot += update_action;
                break;

            case SlotType.Shop:
            case SlotType.Craft:
                update_action?.Invoke(offset, Get(slot_type, offset, count));
                break;
        }       
    }             
    
    public void Discard(SlotType slot_type, Action<int, ItemData> update_action)
    {
        switch (slot_type)
        {
            case SlotType.Inventory:
                m_inventory_service.OnUpdatedSlot -= update_action;
                break;

            case SlotType.Equipment:
                m_equipment_service.OnUpdatedSlot -= update_action;
                break;

            case SlotType.Skill:
                m_skill_service.OnUpdatedSlot -= update_action;
                break;

            case SlotType.Shortcut:
                m_shortcut_service.OnUpdatedSlot -= update_action;
                break;
        }        
    }    

    public ItemData Get(SlotType slot_type, int offset, int count = 1)
    {
        return slot_type switch
        {
            SlotType.Inventory              => m_inventory_service.GetItem(offset),
            SlotType.Equipment              => m_equipment_service.GetItem(offset),
            SlotType.Skill                  => m_skill_service.GetSkill(offset),
            SlotType.Shortcut               => m_shortcut_service.GetItem(offset),
            SlotType.Shop or SlotType.Craft => new ItemData(m_item_db.GetItem((ItemCode)offset).Code, count),
            _                               => null
        };
    }

    public void Set(SlotType slot_type, int offset, ItemCode code, int count = 1)
    {
        var action = slot_type switch
        {
            SlotType.Inventory              => () => m_inventory_service.SetItem(offset, code, count),
            SlotType.Equipment              => () => m_equipment_service.SetItem(offset, code),
            SlotType.Shortcut               => () => m_shortcut_service.SetItem(offset, code),
            _                               => (Action)(() => {})
        };

        action();
    }

    public void Update(SlotType slot_type, int offset, int count)
    {
        var action = slot_type switch
        {
            SlotType.Inventory              => () => m_inventory_service.UpdateItem(offset, count),
            _                               => (Action)(() => {})
        };

        action();
    }

    public void Clear(SlotType slot_type, int offset)
    {
        var action = slot_type switch
        {
            SlotType.Inventory              => () => m_inventory_service.Clear(offset),
            SlotType.Equipment              => () => m_equipment_service.Clear(offset),
            SlotType.Shortcut               => () => m_shortcut_service.Clear(offset),
            _                               => (Action)(() => {})
        };

        action();     
    }    
}