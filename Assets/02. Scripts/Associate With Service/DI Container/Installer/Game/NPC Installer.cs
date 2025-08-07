using InventoryService;
using NPCService;
using UnityEngine;
using UserService;

public class NPCInstaller : MonoBehaviour, IInstaller
{
    [Header("네임 태그 뷰")]
    [SerializeField] private NameTagView m_name_tag_view;

    [Header("상점 뷰")]
    [SerializeField] private ShopView m_shop_view;

    [Header("NPC 부모 트랜스폼")]
    [SerializeField] private Transform m_npc_root;

    public void Install()
    {
        var name_tag_presenter = new NameTagPresenter(m_name_tag_view,
                                                      ServiceLocator.Get<INPCService>());

        var npcs = m_npc_root.GetComponentsInChildren<NPCMouseDetector>();
        foreach (var npc in npcs)
        {
            npc.Inject(name_tag_presenter);
            npc.GetComponent<NPC>().Inject(DIContainer.Resolve<DialoguePresenter>());
        }

        InstallMerchant();
    }

    public void InstallMerchant()
    {
        var shop_presenter = new ShopPresenter(m_shop_view,
                                               ServiceLocator.Get<IInventoryService>(),
                                               ServiceLocator.Get<IUserService>(),
                                               DIContainer.Resolve<ItemSlotFactory>());

        var merchants = m_npc_root.GetComponentsInChildren<Merchant>();
        foreach (var merchant in merchants)
        {
            merchant.Inject(shop_presenter);
        }
    }
}
