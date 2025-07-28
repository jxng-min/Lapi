using InventoryService;
using UnityEngine;

public class InventoryUIInstaller : MonoBehaviour, IInstaller
{
    [Header("아이템 데이터베이스")]
    [SerializeField] private ItemDataBase m_item_db;

    [Header("인벤토리 뷰")]
    [SerializeField] private InventoryView m_inventory_view;

    [Header("아이템 슬롯의 루트")]
    [SerializeField] private Transform m_item_slot_root;

    private IItemSlotView[] m_slot_views;

    public void Install()
    {
        DIContainer.Register<ItemDataBase>(m_item_db);
        DIContainer.Register<IInventoryView>(m_inventory_view);

        var m_inventory_presenter = new InventoryPresenter(m_inventory_view, ServiceLocator.Get<IInventoryService>());
        DIContainer.Register<InventoryPresenter>(m_inventory_presenter);

        DIContainer.Register<IInventoryService>(ServiceLocator.Get<IInventoryService>());

        m_slot_views = m_item_slot_root.GetComponentsInChildren<IItemSlotView>();

        var slot_presenters = new ItemSlotPresenter[m_slot_views.Length];
        for (int i = 0; i < slot_presenters.Length; i++)
        {
            slot_presenters[i] = new ItemSlotPresenter(m_slot_views[i], ServiceLocator.Get<IInventoryService>(), m_item_db);
        }
    }
}
