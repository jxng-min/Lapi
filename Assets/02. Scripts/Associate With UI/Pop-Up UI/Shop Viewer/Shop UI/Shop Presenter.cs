using System.Collections.Generic;
using InventoryService;
using UserService;

public class ShopPresenter : IPopupPresenter
{
    private readonly IShopView m_view;
    private readonly IInventoryService m_inventory_service;
    private readonly IUserService m_user_service;
    private readonly ItemSlotFactory m_item_slot_factory;

    private Sale[] m_sale_list;
    private List<ShopSlotPresenter> m_shop_slot_presenters;

    public ShopPresenter(IShopView view,
                         IInventoryService inventory_service,
                         IUserService user_service,
                         ItemSlotFactory item_slot_factory)
    {
        m_view = view;

        m_inventory_service = inventory_service;
        m_user_service = user_service;

        m_item_slot_factory = item_slot_factory;

        m_shop_slot_presenters = new();

        m_view.Inject(this);
    }

    public void Inject(Sale[] sale_list)
    {
        m_sale_list = sale_list;

        m_shop_slot_presenters.Clear();

        for (int i = 0; i < sale_list.Length; i++)
        {
            var shop_slot_view = m_view.GetShopSlotView();

            var shop_slot_presenter = new ShopSlotPresenter(shop_slot_view,
                                                            m_inventory_service,
                                                            m_user_service,
                                                            m_sale_list[i]);
            shop_slot_presenter.Initialize();
            m_shop_slot_presenters.Add(shop_slot_presenter);

            var item_slot_view = m_view.GetItemSlotView(i);

            var item_slot_presenter = m_item_slot_factory.Instantiate(item_slot_view, (int)m_sale_list[i].Item.Code, SlotType.Shop);
        }
    }

    public void OpenUI()
    {
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_view.CloseUI();
        Dispose();
    }

    public void OnChangedToggle(bool isOn)
    {
        foreach (var shop_slot_presenter in m_shop_slot_presenters)
        {
            shop_slot_presenter.OnChangedToggle(isOn);
        }
    }

    public void Dispose()
    {
        foreach (var shop_slot_presenter in m_shop_slot_presenters)
        {
            shop_slot_presenter.Dispose();
        }
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }
}
