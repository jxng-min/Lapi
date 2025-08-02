using System.Collections.Generic;
using UnityEngine;

public class ItemCooler : MonoBehaviour, IItemCooler
{
    private List<ItemCode> m_item_list;
    private Dictionary<ItemCode, float> m_cool_dict;
    private float m_cached_cooltime;

    private void Awake()
    {
        m_item_list = new();
        m_cool_dict = new();
    }

    private void Update()
    {
        for (int i = m_item_list.Count - 1; i >= 0; i--)
        {
            m_cached_cooltime = m_cool_dict[m_item_list[i]] -= Time.deltaTime;
            if (m_cached_cooltime <= 0f)
            {
                m_item_list.RemoveAt(i);
            }
        }
    }

    public void Push(ItemCode code, float cooltime)
    {
        if (!m_cool_dict.TryAdd(code, cooltime))
        {
            m_cool_dict[code] = cooltime;
        }

        m_item_list.Add(code);
    }

    public void Pop(ItemCode code)
    {
        for (int i = 0; i < m_item_list.Count; i++)
        {
            if (m_item_list[i] == code)
            {
                m_item_list.RemoveAt(i);
                break;
            }
        }
    }

    public float GetCool(ItemCode code)
    {
        return m_cool_dict.TryGetValue(code, out var cool) ? cool : 0f;
    }
}
