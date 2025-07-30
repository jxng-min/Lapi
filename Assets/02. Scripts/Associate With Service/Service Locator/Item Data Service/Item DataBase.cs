using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item DataBase", menuName = "SO/DB/Create Item DataBase")]
public class ItemDataBase : ScriptableObject, IItemDataBase
{
    [Header("아이템 목록")]
    [SerializeField] private Item[] m_item_list;

    private Dictionary<ItemCode, Item> m_item_dict;

    private void OnEnable()
    {
        m_item_dict = new();

        if (m_item_list == null)
        {
            return;
        }

        foreach (var item in m_item_list)
        {
            m_item_dict.TryAdd(item.Code, item);
        }
    }

    public Item GetItem(ItemCode code)
    {
        return m_item_dict.TryGetValue(code, out var item) ? item : null;
    }
}
