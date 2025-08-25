using SkillService;
using UnityEngine.EventSystems;

public class SlotPointerHandler
{
    private ISkillService m_skill_service;

    private IItemSlotContext m_slot_context;
    private ToolTipPresenter m_tooltip_presenter;
    private IItemActivator m_item_activator;
    private IItemCooler m_item_cooler;

    private IItemDataBase m_item_db;
    private ICursorDataBase m_cursor_db;

    private SlotType m_slot_type;
    private int m_offset;

    public SlotPointerHandler(ISkillService skill_service,
                              IItemSlotContext slot_context,
                              ToolTipPresenter tooltip_presenter,
                              IItemActivator item_activator,
                              IItemCooler item_cooler,
                              IItemDataBase item_db,
                              ICursorDataBase cursor_db)
    {
        m_skill_service = skill_service;
        m_slot_context = slot_context;
        m_tooltip_presenter = tooltip_presenter;
        m_item_activator = item_activator;
        m_item_cooler = item_cooler;
        m_item_db = item_db;
        m_cursor_db = cursor_db;
    }

    public void OnPointerEnter(SlotType slot_type, int offset)
    {
        var code = m_slot_context.Get(slot_type, offset).Code;
        if (code == ItemCode.NONE)
        {
            return;
        }

        m_slot_type = slot_type;
        m_offset = offset;

        m_tooltip_presenter.OpenUI(code);
        m_cursor_db.SetCursor(CursorMode.CAN_GRAB);
    }

    public void OnPointerExit()
    {
        m_tooltip_presenter.CloseUI();
        m_cursor_db.SetCursor(CursorMode.DEFAULT);
    }

    public void OnPointerClick(SlotType slot_type, int offset)
    {
        m_slot_type = slot_type;
        m_offset = offset;

        m_cursor_db.SetCursor(CursorMode.DEFAULT);

        var code = m_slot_context.Get(m_slot_type, m_offset).Code;
        if (code == ItemCode.NONE)
        {
            return;
        }

        if (m_item_cooler.GetCool(code) > 0f)
        {
            return;
        }

        SoundManager.Instance.PlaySFX("Default");

        if (m_slot_type == SlotType.Skill)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                m_cursor_db.SetCursor(CursorMode.CAN_GRAB);
            }
            else
            {
                m_cursor_db.SetCursor(CursorMode.DEFAULT);
            }

            var item_data = m_slot_context.Get(m_slot_type, m_offset);
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
            var count = m_slot_context.Get(m_slot_type, m_offset).Count;
            if (count > 1)
            {
                m_slot_context.Update(m_slot_type, m_offset, -1);
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    m_cursor_db.SetCursor(CursorMode.CAN_GRAB);
                }
                else
                {
                    m_cursor_db.SetCursor(CursorMode.DEFAULT);
                }
            }
            else
            {
                m_slot_context.Clear(m_slot_type, m_offset);
                m_cursor_db.SetCursor(CursorMode.DEFAULT);
            }
        }      
    }
}
