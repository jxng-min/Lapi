using EquipmentService;
using InventoryService;
using KeyService;
using QuestService;
using SettingService;
using ShortcutService;
using SkillService;
using UserService;

public class LoaderSlotPresenter
{
    private readonly ILoaderSlotView m_view;
    private readonly IUserService m_user_service;
    private readonly IInventoryService m_inventory_service;
    private readonly IEquipmentService m_equipment_service;
    private readonly ISkillService m_skill_service;
    private readonly IKeyService m_key_service;
    private readonly IShortcutService m_shortcut_service;
    private readonly IQuestService m_quest_service;
    private readonly ISettingService m_setting_service;

    private readonly int m_offset;
    private readonly bool m_is_loader;

    public LoaderSlotPresenter(ILoaderSlotView view,
                               IUserService user_service,
                               IInventoryService inventory_service,
                               IEquipmentService equipment_service,
                               ISkillService skill_service,
                               IKeyService key_service,
                               IShortcutService shortcut_service,
                               IQuestService quest_service,
                               ISettingService setting_service,
                               int offset,
                               bool is_loader)
    {
        m_view = view;
        m_user_service = user_service;
        m_inventory_service = inventory_service;
        m_equipment_service = equipment_service;
        m_skill_service = skill_service;
        m_key_service = key_service;
        m_shortcut_service = shortcut_service;
        m_quest_service = quest_service;
        m_setting_service = setting_service;

        m_offset = offset;
        m_is_loader = is_loader;

        m_view?.Inject(this);
    }

    public void UpdateUI()
    {
        var seperated_time = Calculate();
        var hour = seperated_time.Item1;
        var minute = seperated_time.Item2;
        var second = seperated_time.Item3;

        m_view?.UpdateUI(true, m_is_loader, m_user_service.Status.Level, hour, minute, second);        
    }

    public void Load()
    {
        if (!(m_user_service as ISaveable).Load(m_offset) ||
           !(m_inventory_service as ISaveable).Load(m_offset) ||
           !(m_equipment_service as ISaveable).Load(m_offset) ||
           !(m_skill_service as ISaveable).Load(m_offset) ||
           !(m_key_service as ISaveable).Load(m_offset) ||
           !(m_shortcut_service as ISaveable).Load(m_offset) ||
           !(m_quest_service as ISaveable).Load(m_offset) ||
           !(m_setting_service as ISaveable).Load(m_offset))
        {
            m_view?.UpdateUI(false, m_is_loader);
        }
        else
        {
            UpdateUI();
        }
    }

    public void Save()
    {
        (m_user_service as ISaveable).Save(m_offset);
        (m_inventory_service as ISaveable).Save(m_offset);
        (m_equipment_service as ISaveable).Save(m_offset);
        (m_skill_service as ISaveable).Save(m_offset);
        (m_key_service as ISaveable).Save(m_offset);
        (m_shortcut_service as ISaveable).Save(m_offset);
        (m_quest_service as ISaveable).Save(m_offset);
        (m_setting_service as ISaveable).Save(m_offset);

        UpdateUI();
    }

    public void OnClickedButton()
    {
        if (m_is_loader)
        {
            Load();
            m_view.LoadScene(m_user_service.Map);
            m_view.PlaySFX("Load");
        }
        else
        {
            Save();
            m_view.PlaySFX("Save");
        }
    }

    private (int, int, int) Calculate()
    {
        var playtime = m_user_service.PlayTime;

        int hr = (int)playtime / 3600;
        playtime %= 3600;

        int min = (int)playtime / 60;
        int sec = (int)playtime % 60;

        return (hr, min, sec);
    }
}
