using InventoryService;
using SkillService;
using UnityEngine;
using UserService;

public class SkillSlotPresenter
{
    private readonly ISkillSlotView m_view;
    private readonly ISkillService m_skill_service;
    private readonly IUserService m_user_service;
    private readonly IItemDataBase m_item_db;

    private ItemSlotPresenter m_item_slot_presenter;
    private int m_offset;

    public SkillSlotPresenter(ISkillSlotView view,
                              ISkillService skill_service,
                              IUserService user_service,
                              IItemDataBase item_db,
                              ItemSlotPresenter item_slot_presenter,
                              int offset)
    {
        m_view = view;
        m_skill_service = skill_service;
        m_user_service = user_service;
        m_item_db = item_db;

        m_item_slot_presenter = item_slot_presenter;

        m_offset = offset;

        m_skill_service.OnUpdatedSlot += UpdateSlot;

        m_view.Inject(this);
    }

    public void UpdateSlot(int offset, ItemData item_data)
    {
        if (m_offset == offset)
        {
            var item = m_item_db.GetItem(item_data.Code) as SkillItem;

            var player_level = m_user_service.Status.Level;

            m_view.UpdateUI(item.Name,
                            item_data.Count,
                            m_skill_service.SkillPoint > 0,
                            item.Constraint <= player_level,
                            item.Constraint);
        }
    }

    public void UpgradeSkill()
    {
        for (int offset = 0; offset < 3; offset++)
        {
            if (m_offset == offset)
            {
                m_skill_service.UpdateSkill(m_offset, 1);
            }
            else
            {
                m_skill_service.UpdateSkill(offset, 0, 0);
            }
        }
    }
}
