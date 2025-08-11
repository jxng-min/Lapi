using System.Collections.Generic;
using KeyService;
using ShortcutService;
using UnityEngine;

public class ShortcutUIInstaller : MonoBehaviour, IInstaller
{
    [Header("단축키 뷰")]
    [SerializeField] private ShortcutView m_shortcut_view;

    [Header("단축키 슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_shortcut_slot_root;

    public void Install()
    {
        DIContainer.Register<IShortcutView>(m_shortcut_view);

        var shortcut_slot_view = m_shortcut_slot_root.GetComponentsInChildren<ShortcutSlotView>();

        var shortcut_slot_presenters = new ShortcutSlotPresenter[shortcut_slot_view.Length];
        for (int i = 0; i < shortcut_slot_presenters.Length; i++)
        {
            shortcut_slot_presenters[i] = new ShortcutSlotPresenter(shortcut_slot_view[i],
                                                                    DIContainer.Resolve<IItemDataBase>(),
                                                                    ServiceLocator.Get<IKeyService>(),
                                                                    ServiceLocator.Get<IShortcutService>(),
                                                                    DIContainer.Resolve<InventoryPresenter>(),
                                                                    DIContainer.Resolve<SkillPresenter>(),
                                                                    i);
        }

        var item_slot_views = new List<IItemSlotView>();
        for (int i = 0; i < shortcut_slot_view.Length; i++)
        {
            item_slot_views.Add(shortcut_slot_view[i].GetComponentInChildren<IItemSlotView>());
        }

        var item_slot_factory = DIContainer.Resolve<ItemSlotFactory>();

        var item_slot_presenters = new List<ItemSlotPresenter>();
        for (int i = 0; i < item_slot_views.Count; i++)
        {
            item_slot_presenters.Add(item_slot_factory.Instantiate(item_slot_views[i], i, SlotType.Shortcut));
        }

        var shortcut_presenter = new ShortcutPresenter(m_shortcut_view,
                                                       DIContainer.Resolve<InventoryPresenter>(),
                                                       DIContainer.Resolve<SkillPresenter>());
        DIContainer.Register<ShortcutPresenter>(shortcut_presenter);
    }
}
