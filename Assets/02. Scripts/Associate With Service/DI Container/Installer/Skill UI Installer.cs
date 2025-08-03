using System.Collections.Generic;
using EquipmentService;
using InventoryService;
using ShortcutService;
using SkillService;
using UnityEngine;
using UserService;

public class SkillUIInstaller : MonoBehaviour, IInstaller
{
    [Header("스킬 뷰")]
    [SerializeField] private SkillView m_skill_view;

    [Header("스킬 슬롯들의 루트")]
    [SerializeField] private Transform m_skill_root;

    public void Install()
    {
        var skill_slots = m_skill_root.GetComponentsInChildren<SkillSlotView>();

        List<IItemSlotView> item_slot_view_list = new();
        foreach (var skill_slot in skill_slots)
        {
            item_slot_view_list.Add(skill_slot.GetComponentInChildren<IItemSlotView>());
        }

        var item_slot_presenters = new ItemSlotPresenter[skill_slots.Length];
        for (int i = 0; i < item_slot_view_list.Count; i++)
        {
            item_slot_presenters[i] = new ItemSlotPresenter(item_slot_view_list[i],
                                                       ServiceLocator.Get<IInventoryService>(),
                                                       ServiceLocator.Get<IEquipmentService>(),
                                                       ServiceLocator.Get<ISkillService>(),
                                                       ServiceLocator.Get<IShortcutService>(),
                                                       DIContainer.Resolve<IItemDataBase>(),
                                                       DIContainer.Resolve<ToolTipPresenter>(),
                                                       DIContainer.Resolve<DragSlotPresenter>(),
                                                       DIContainer.Resolve<IItemActivator>(),
                                                       DIContainer.Resolve<IItemCooler>(),
                                                       i,
                                                       SlotType.Skill);
        }

        var skill_slot_presenters = new SkillSlotPresenter[skill_slots.Length];
        for (int i = 0; i < skill_slots.Length; i++)
        {
            skill_slot_presenters[i] = new SkillSlotPresenter(skill_slots[i],
                                                              ServiceLocator.Get<ISkillService>(),
                                                              ServiceLocator.Get<IUserService>(),
                                                              DIContainer.Resolve<IItemDataBase>(),
                                                              item_slot_presenters[i],
                                                              i);
        }

        var skill_presenter = new SkillPresenter(m_skill_view,
                                                 ServiceLocator.Get<ISkillService>(),
                                                 item_slot_presenters);
        DIContainer.Register<SkillPresenter>(skill_presenter);
    }
}
