using InventoryService;
using UserService;

public class ShopSlotPresenter
{
    private readonly IShopSlotView m_view;
    private readonly IInventoryService m_inventory_service;
    private readonly IUserService m_user_service;
    private readonly Sale m_sale;

    public ShopSlotPresenter(IShopSlotView view,
                             IInventoryService inventory_service,
                             IUserService user_service,
                             Sale sale)
    {
        m_view = view;
        m_inventory_service = inventory_service;
        m_user_service = user_service;

        m_sale = sale;

        m_inventory_service.OnUpdatedGold += UpdateUI;

        m_view.Inject(this);
    }

    public void Initialize()
    {
        m_inventory_service.InitializeGold();
    }

    public void UpdateUI(int money)
    {
        m_view.UpdateUI(m_sale.Item.Name,
                        m_sale.Cost,
                        money >= m_sale.Cost,
                        m_user_service.Status.Level < m_sale.Constraint,
                        m_sale.Constraint);
    }

    public void OnClickedPurchase()
    {
        m_inventory_service.UpdateGold(-m_sale.Cost);
        m_inventory_service.AddItem(m_sale.Item.Code, 1);
    }

    public void OnChangedToggle(bool check)
    {
        if (check)
        {
            if (m_inventory_service.Gold >= m_sale.Cost && m_user_service.Status.Level >= m_sale.Constraint)
            {
                m_view.DisableObject(false);
            }
            else
            {
                m_view.DisableObject(true);
            }
        }
        else
        {
            m_view.DisableObject(false);
        }
    }
}
