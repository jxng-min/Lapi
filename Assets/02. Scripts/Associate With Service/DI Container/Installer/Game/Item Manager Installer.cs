using EquipmentService;
using InventoryService;
using ItemDataService;
using ShortcutService;
using SkillService;
using UnityEngine;

public class ItemManagerInstaller : MonoBehaviour, IInstaller
{
    [Header("아이템 데이터베이스")]
    [SerializeField] private ItemDataBase m_item_db;
    
    [Header("아이템 활성자")]
    [SerializeField] private ItemActivator m_item_activator;

    [Header("아이템 쿨러")]
    [SerializeField] private ItemCooler m_item_cooler;

    [Header("툴팁 뷰")]
    [SerializeField] private ToolTipView m_tooltip_view;

    [Header("드래그 슬롯 뷰")]
    [SerializeField] private DragSlotView m_drag_slot_view;

    public void Install()
    {
        DIContainer.Register<IItemDataBase>(m_item_db);

        DIContainer.Register<IItemActivator>(m_item_activator);
        DIContainer.Register<IItemCooler>(m_item_cooler);

        var tooltip_presenter = new ToolTipPresenter(m_tooltip_view,
                                                     ServiceLocator.Get<IItemDataService>(),
                                                     DIContainer.Resolve<IItemDataBase>());
        DIContainer.Register<ToolTipPresenter>(tooltip_presenter);

        var drag_slot_presenter = new DragSlotPresenter(m_drag_slot_view,
                                                        DIContainer.Resolve<IItemDataBase>(),
                                                        ServiceLocator.Get<IInventoryService>(),
                                                        ServiceLocator.Get<IEquipmentService>(),
                                                        ServiceLocator.Get<ISkillService>(),
                                                        ServiceLocator.Get<IShortcutService>());
        DIContainer.Register<DragSlotPresenter>(drag_slot_presenter);

        Inject();
    }

    public void Inject()
    {
        var player_ctrl = DIContainer.Resolve<PlayerCtrl>();

        m_item_activator.Inject(player_ctrl,
                                ServiceLocator.Get<IInventoryService>(),
                                ServiceLocator.Get<IEquipmentService>(),
                                ServiceLocator.Get<ISkillService>());
    }
}
