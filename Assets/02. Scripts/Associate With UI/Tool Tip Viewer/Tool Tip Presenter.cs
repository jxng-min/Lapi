using ItemDataService;

public class ToolTipPresenter
{
    private readonly IToolTipView m_view;
    private readonly IItemDataService m_item_data_service;
    private readonly ItemDataBase m_item_db;

    public ToolTipPresenter(IToolTipView view, IItemDataService item_data_service, ItemDataBase item_db)
    {
        m_view = view;
        m_item_data_service = item_data_service;
        m_item_db = item_db;

        m_view.Inject(this);
    }

    public void OpenUI(ItemCode code)
    {
        var item = m_item_db.GetItem(code);

        var name = m_item_data_service.GetName(code);
        var type = GetTypeName(item.Type);
        var desc = m_item_data_service.GetDescription(code);


        m_view.UpdateUI(item.Sprite, name, type, desc);
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }

    private string GetTypeName(ItemType type)
    {
        if ((int)(type & ItemType.Consumable) != 0)
        {
            return "소비 아이템";
        }
        else if ((int)(type & ItemType.Quest) != 0)
        {
            return "퀘스트 아이템";
        }
        else if ((int)(type & ItemType.ETC) != 0)
        {
            return "기타 아이템";
        }
        else
        {
            return "장비 아이템";
        }
    }
}