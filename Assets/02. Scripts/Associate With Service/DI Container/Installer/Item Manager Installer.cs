using EquipmentService;
using InventoryService;
using UnityEngine;

public class ItemManagerInstaller : MonoBehaviour, IInstaller
{
    [Header("아이템 활성자")]
    [SerializeField] private ItemActivator m_item_activator;

    [Header("아이템 쿨러")]
    [SerializeField] private ItemCooler m_item_cooler;

    public void Install()
    {
        DIContainer.Register<IItemActivator>(m_item_activator);
        DIContainer.Register<IItemCooler>(m_item_cooler);

        Inject();
    }

    public void Inject()
    {
        var player_ctrl = DIContainer.Resolve<PlayerCtrl>();

        m_item_activator.Inject(player_ctrl,
                                ServiceLocator.Get<IInventoryService>(),
                                ServiceLocator.Get<IEquipmentService>());
    }
}
