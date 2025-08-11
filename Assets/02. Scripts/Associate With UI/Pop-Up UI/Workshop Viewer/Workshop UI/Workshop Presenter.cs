using System.Collections.Generic;
using InventoryService;
using UserService;

public class WorkshopPresenter : IPopupPresenter
{
    private readonly IWorkshopView m_view;
    private readonly IInventoryService m_inventory_service;
    private readonly IUserService m_user_service;
    private readonly ItemSlotFactory m_item_slot_factory;

    private ItemReceipe[] m_receipe_list;
    private List<WorkshopSlotPresenter> m_workshop_slot_presenters;

    public WorkshopPresenter(IWorkshopView view,
                             IInventoryService inventory_service,
                             IUserService user_service,
                             ItemSlotFactory item_slot_factory)
    {
        m_view = view;

        m_inventory_service = inventory_service;
        m_user_service = user_service;

        m_item_slot_factory = item_slot_factory;

        m_workshop_slot_presenters = new();

        m_view.Inject(this);
    }

    public void Inject(ItemReceipe[] receipe_list)
    {
        m_receipe_list = receipe_list;

        m_workshop_slot_presenters.Clear();

        for (int i = 0; i < m_receipe_list.Length; i++)
        {
            var workshop_slot_view = m_view.GetWorkshopSlotView();

            var workshop_slot_presenter = new WorkshopSlotPresenter(workshop_slot_view,
                                                                    m_inventory_service,
                                                                    m_user_service,
                                                                    m_item_slot_factory);
            workshop_slot_presenter.Inject(m_receipe_list[i]);
            workshop_slot_presenter.Initialize();
            m_workshop_slot_presenters.Add(workshop_slot_presenter);
        }
    }

    public void OpenUI()
    {
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        if (IsCrafting())
        {
            return;
        }

        m_view.CloseUI();
    }

    public void OnChangedToggle(bool isOn)
    {
        foreach (var shop_slot_presenter in m_workshop_slot_presenters)
        {
            shop_slot_presenter.OnChangedToggle(isOn);
        }
    }

    public bool IsCrafting()
    {
        foreach (var presenter in m_workshop_slot_presenters)
        {
            if (presenter.IsCrafting)
            {
                return true;
            }
        }

        return false;
    }

    public void ReturnItemSlots()
    {
        foreach (var presenter in m_workshop_slot_presenters)
        {
            presenter?.ReturnSlots();
            presenter.Dispose();
        }
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }
}