using System;
using InventoryService;
using UserService;

public class WorkshopSlotPresenter : IDisposable
{
    private readonly IWorkshopSlotView m_view;
    private readonly IInventoryService m_inventory_service;
    private readonly IUserService m_user_service;
    private readonly ItemSlotFactory m_item_slot_factory;

    private ItemReceipe m_receipe;

    private bool m_is_crafting;

    public bool IsCrafting => m_is_crafting;

    public WorkshopSlotPresenter(IWorkshopSlotView view,
                                 IInventoryService inventory_service,
                                 IUserService user_service,
                                 ItemSlotFactory item_slot_factory)
    {
        m_view = view;

        m_inventory_service = inventory_service;
        m_user_service = user_service;

        m_item_slot_factory = item_slot_factory;

        m_inventory_service.OnUpdatedSlot += UpdateUI;

        m_view.Inject(this);
    }

    public void Inject(ItemReceipe receipe)
    {
        m_receipe = receipe;

        for (int i = 0; i < m_receipe.Ingredients.Length; i++)
        {
            var ingredient_item_slot_view = m_view.GetIngredientItemSlotView();

            var ingredient_item_slot_presenter = m_item_slot_factory.Instantiate(ingredient_item_slot_view,
                                                                                 (int)m_receipe.Ingredients[i].Item.Code,
                                                                                 SlotType.Craft,
                                                                                 m_receipe.Ingredients[i].Count);
        }

        var target_item_slot_view = m_view.GetTargetItemSlotView();

        var target_item_slot_presenter = m_item_slot_factory.Instantiate(target_item_slot_view,
                                                                         (int)m_receipe.Target.Code,
                                                                         SlotType.Craft,
                                                                         m_receipe.Count);
    }

    public void Initialize()
    {
        m_inventory_service.InitializeSlot(0);
    }

    public void UpdateUI(int offset, ItemData item_data)
    {
        if (m_is_crafting)
        {
            return;
        }
        
        m_view.UpdateUI(CheckCanCraft(), m_user_service.Status.Level < m_receipe.Constraint, m_receipe.Constraint);
    }

    public void OnClickedPurchase()
    {
        m_view.CraftItem(m_receipe.Time);
    }

    public void ConsumeIngredient()
    {
        m_is_crafting = true;

        foreach (var ingredient in m_receipe.Ingredients)
        {
            var target_code = ingredient.Item.Code;
            m_inventory_service.RemoveItem(target_code, ingredient.Count);
        }
    }

    public void GetTarget()
    {
        m_inventory_service.AddItem(m_receipe.Target.Code, m_receipe.Count);

        m_is_crafting = false;
    }

    private bool CheckCanCraft()
    {
        foreach (var ingredient in m_receipe.Ingredients)
        {
            var target_code = ingredient.Item.Code;
            if (m_inventory_service.GetItemCount(target_code) < ingredient.Count)
            {
                return false;
            }
        }

        return true;
    }

    public void ReturnSlots()
    {
        m_view.ReturnItemSlots();
    }

    public void OnChangedToggle(bool check)
    {
        if (check)
        {
            if (CheckCanCraft() && m_user_service.Status.Level >= m_receipe.Constraint)
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

    public void Dispose()
    {
        m_inventory_service.OnUpdatedSlot -= UpdateUI;
    }
}
