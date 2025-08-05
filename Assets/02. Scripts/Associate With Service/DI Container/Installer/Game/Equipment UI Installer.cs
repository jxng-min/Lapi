using EquipmentService;
using InventoryService;
using ShortcutService;
using SkillService;
using UnityEngine;

public class EquipmentUIInstaller : MonoBehaviour, IInstaller
{
    [Header("아이템 데이터베이스")]
    [SerializeField] private ItemDataBase m_item_db;

    [Header("장비 뷰")]
    [SerializeField] private EquipmentView m_equipment_view;

    [Header("아이템 슬롯의 부모")]
    [SerializeField] private Transform m_item_slot_root;

    [Header("플레이어 컨트롤러")]
    [SerializeField] private PlayerCtrl m_player_ctrl;

    public void Install()
    {
        DIContainer.Register<IEquipmentView>(m_equipment_view);

        ServiceLocator.Get<IEquipmentService>().Inject(m_item_db);

        var equipment_presenter = new EquipmentPresenter(m_equipment_view,
                                                         ServiceLocator.Get<IEquipmentService>(),
                                                         m_player_ctrl);
        DIContainer.Register<EquipmentPresenter>(equipment_presenter);

        DIContainer.Register<IEquipmentService>(ServiceLocator.Get<IEquipmentService>());

        var tooltip_presenter = DIContainer.Resolve<ToolTipPresenter>();
        var drag_slot_presenter = DIContainer.Resolve<DragSlotPresenter>();

        var slot_views = m_item_slot_root.GetComponentsInChildren<IItemSlotView>();

        var item_activator = DIContainer.Resolve<IItemActivator>();
        var item_cooler = DIContainer.Resolve<IItemCooler>();

        var slot_presenters = new ItemSlotPresenter[slot_views.Length];
        for (int i = 0; i < slot_presenters.Length; i++)
        {
            slot_presenters[i] = new ItemSlotPresenter(slot_views[i],
                                                       ServiceLocator.Get<IInventoryService>(),
                                                       ServiceLocator.Get<IEquipmentService>(),
                                                       ServiceLocator.Get<ISkillService>(),
                                                       ServiceLocator.Get<IShortcutService>(),
                                                       m_item_db,
                                                       tooltip_presenter,
                                                       drag_slot_presenter,
                                                       item_activator,
                                                       item_cooler,
                                                       i,
                                                       SlotType.Equipment);
        }
    }
}
