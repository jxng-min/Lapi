using EquipmentService;
using InventoryService;
using ItemDataService;
using SkillService;
using UnityEngine;

public class InventoryUIInstaller : MonoBehaviour, IInstaller
{
    [Header("아이템 데이터베이스")]
    [SerializeField] private ItemDataBase m_item_db;

    [Header("인벤토리 뷰")]
    [SerializeField] private InventoryView m_inventory_view;

    [Header("아이템 슬롯의 루트")]
    [SerializeField] private Transform m_item_slot_root;

    [Header("툴팁 뷰")]
    [SerializeField] private ToolTipView m_tooltip_view;

    [Header("드래그 슬롯 뷰")]
    [SerializeField] private DragSlotView m_drag_slot_view;

    public void Install()
    {
        DIContainer.Register<IItemDataBase>(m_item_db);
        DIContainer.Register<IInventoryView>(m_inventory_view);

        var m_inventory_presenter = new InventoryPresenter(m_inventory_view,
                                                           ServiceLocator.Get<IInventoryService>());
        DIContainer.Register<InventoryPresenter>(m_inventory_presenter);

        DIContainer.Register<IInventoryService>(ServiceLocator.Get<IInventoryService>());

        var tooltip_presenter = new ToolTipPresenter(m_tooltip_view,
                                                     ServiceLocator.Get<IItemDataService>(),
                                                     m_item_db);
        DIContainer.Register<ToolTipPresenter>(tooltip_presenter);

        var drag_slot_presenter = new DragSlotPresenter(m_drag_slot_view,
                                                        m_item_db,
                                                        ServiceLocator.Get<IInventoryService>(),
                                                        ServiceLocator.Get<IEquipmentService>());
        DIContainer.Register<DragSlotPresenter>(drag_slot_presenter);

        var item_activator = DIContainer.Resolve<IItemActivator>();
        var item_cooler = DIContainer.Resolve<IItemCooler>();

        var slot_views = m_item_slot_root.GetComponentsInChildren<IItemSlotView>();

        var slot_presenters = new ItemSlotPresenter[slot_views.Length];
        for (int i = 0; i < slot_presenters.Length; i++)
        {
            slot_presenters[i] = new ItemSlotPresenter(slot_views[i],
                                                       ServiceLocator.Get<IInventoryService>(),
                                                       ServiceLocator.Get<IEquipmentService>(),
                                                       ServiceLocator.Get<ISkillService>(),
                                                       m_item_db,
                                                       tooltip_presenter,
                                                       drag_slot_presenter,
                                                       item_activator,
                                                       item_cooler,
                                                       i);
        }

        Inject();
    }

    private void Inject()
    {
        var item_db = DIContainer.Resolve<IItemDataBase>();

        var inventory_model = DIContainer.Resolve<IInventoryService>();
        inventory_model.Inject(item_db);
    }
}
