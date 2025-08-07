using EquipmentService;
using InventoryService;
using ShortcutService;
using SkillService;
using UnityEngine;

public class ItemSlotFactoryInstaller : MonoBehaviour, IInstaller
{
    [Header("아이템 데이터베이스")]
    [SerializeField] private ItemDataBase m_item_db;

    public void Install()
    {
        var item_slot_factory = new ItemSlotFactory(ServiceLocator.Get<IInventoryService>(),
                                                    ServiceLocator.Get<IEquipmentService>(),
                                                    ServiceLocator.Get<ISkillService>(),
                                                    ServiceLocator.Get<IShortcutService>(),
                                                    m_item_db,
                                                    DIContainer.Resolve<ToolTipPresenter>(),
                                                    DIContainer.Resolve<DragSlotPresenter>(),
                                                    DIContainer.Resolve<IItemActivator>(),
                                                    DIContainer.Resolve<IItemCooler>());
        DIContainer.Register<ItemSlotFactory>(item_slot_factory);
    }
}
