using UnityEngine;

[System.Serializable]
public class ItemQuest : BaseQuest
{
    [Header("아이템 코드")]
    [SerializeField] private ItemCode m_item_code;

    [Header("총 획득 횟수")]
    [SerializeField] private int m_total_item_count;

    public ItemCode Code => m_item_code;
    public int Total => m_total_item_count;

    public override string GetFormatText(int count)
    {
        return $"[{Mathf.Clamp(count, 0, Total)}/{Total}]";
    }
}
