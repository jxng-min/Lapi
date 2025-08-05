using EquipmentService;
using InventoryService;
using KeyService;
using ShortcutService;
using SkillService;
using UnityEngine;
using UserService;

public class LoaderUIInstaller : MonoBehaviour, IInstaller
{
    [Header("로더 뷰")]
    [SerializeField] private LoaderView m_loader_view;

    [Header("로더 슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_loader_slot_root;

    [Header("로더 = true, 세이버 = false")]
    [SerializeField] private bool m_is_loader;

    public void Install()
    {
        var loader_slot_views = m_loader_slot_root.GetComponentsInChildren<ILoaderSlotView>();

        var loader_slot_presenters = new LoaderSlotPresenter[loader_slot_views.Length + 1];
        for (int i = 0; i < loader_slot_presenters.Length; i++)
        {
            if (i == 4)
            {
                loader_slot_presenters[i] = new LoaderSlotPresenter(null,
                                                                    ServiceLocator.Get<IUserService>(),
                                                                    ServiceLocator.Get<IInventoryService>(),
                                                                    ServiceLocator.Get<IEquipmentService>(),
                                                                    ServiceLocator.Get<ISkillService>(),
                                                                    ServiceLocator.Get<IKeyService>(),
                                                                    ServiceLocator.Get<IShortcutService>(),
                                                                    i,
                                                                    m_is_loader);
            }
            else
            {
                loader_slot_presenters[i] = new LoaderSlotPresenter(loader_slot_views[i],
                                                                    ServiceLocator.Get<IUserService>(),
                                                                    ServiceLocator.Get<IInventoryService>(),
                                                                    ServiceLocator.Get<IEquipmentService>(),
                                                                    ServiceLocator.Get<ISkillService>(),
                                                                    ServiceLocator.Get<IKeyService>(),
                                                                    ServiceLocator.Get<IShortcutService>(),
                                                                    i,
                                                                    m_is_loader);
            }
        }

        var loader_presenter = new LoaderPresenter(m_loader_view, loader_slot_presenters);
    }
}
