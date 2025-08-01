using System.Collections.Generic;
using EquipmentService;
using InventoryService;
using SkillService;
using UnityEngine;

public class ItemActivator : MonoBehaviour, IItemActivator
{
    private PlayerCtrl m_player_ctrl;
    private IInventoryService m_inventory_service;
    private IEquipmentService m_equipment_service;
    private ISkillService m_skill_service;

    private Dictionary<ItemCode, ItemStrategy> m_activate_dict;
    private Dictionary<ItemCode, ISkillStrategy> m_skill_dict;

    private void Awake()
    {
        RegisterActivateStrategy();
    }

    public void Inject(PlayerCtrl player_ctrl, IInventoryService inventory_service, IEquipmentService equipment_service, ISkillService skill_service)
    {
        m_player_ctrl = player_ctrl;

        m_inventory_service = inventory_service;
        m_equipment_service = equipment_service;
        m_skill_service = skill_service;

        foreach (var item in m_activate_dict)
        {
            item.Value.Inject(m_player_ctrl);
        }

        foreach (var skill in m_skill_dict)
        {
            skill.Value.Inject(m_skill_service);
        }
    }

    public bool UseItem(Item item, int offset, SlotType slot_type = SlotType.Inventory)
    {
        switch (item.Type)
        {
            case ItemType.Consumable:
            case ItemType.Skill:
                return ActivateItem(item);

            case ItemType.Quest:
            case ItemType.ETC:
                return false;

            case ItemType.Equipment_Helmet:
            case ItemType.Equipment_Armor:
            case ItemType.Equipment_Weapon:
            case ItemType.Equipment_Shield:
                ToggleEquipment(item, offset, slot_type);
                return false;
        }

        return true;
    }

    private void RegisterActivateStrategy()
    {
        var dash_strategy = new DashStrategy();
        var fire_ball_strategy = new FireBallStrategy();
        var thunder_strategy = new ThunderStrategy();

        m_activate_dict = new()
        {
            { ItemCode.SMALL_HP_POTION, new SmallHPPotionStrategy() },
            { ItemCode.SMALL_MP_POTION, new SmallMPPotionStrategy() },
            { ItemCode.SMALL_POTION, new SmallPotionStrategy() },
            { ItemCode.MIDDLE_HP_POTION, new MiddleHPPotionStrategy() },
            { ItemCode.MIDDLE_MP_POTION, new MiddleMPPotionStrategy() },
            { ItemCode.MIDDLE_POTION, new MiddlePotionStrategy() },
            { ItemCode.DASH, dash_strategy },
            { ItemCode.FIRE_BALL, fire_ball_strategy },
            { ItemCode.THUNDER, thunder_strategy },
        };
        
        m_skill_dict = new()
        {
            { ItemCode.DASH, dash_strategy },
            { ItemCode.FIRE_BALL, fire_ball_strategy },
            { ItemCode.THUNDER, thunder_strategy },
        };
    }

    private bool ActivateItem(Item item)
    {
        if (m_activate_dict.TryGetValue(item.Code, out var strategy))
        {
            return strategy.Activate(item);
        }

        return false;
    }

    private void ToggleEquipment(Item item, int offset, SlotType slot_type)
    {
        if (slot_type == SlotType.Equipment)
        {
            var target_offset = m_inventory_service.GetValidOffset(item.Code);

            if (target_offset != -1)
            {
                m_equipment_service.Clear(offset);
                m_inventory_service.SetItem(target_offset, item.Code, 1);
            }
        }
        else
        {
            var target_offset = m_equipment_service.GetOffset(item.Type);

            var temp_item_data = new ItemData();
            temp_item_data.Code = m_equipment_service.GetItem(target_offset).Code;
            temp_item_data.Count = m_equipment_service.GetItem(target_offset).Count;

            m_equipment_service.AddItem(item.Code);

            if (temp_item_data.Code == ItemCode.NONE)
            {
                m_inventory_service.Clear(offset);
            }
            else
            {
                m_inventory_service.AddItem(temp_item_data.Code, temp_item_data.Count);
            }
        }
    }
}
