using UnityEngine;

[CreateAssetMenu(fileName = "New Item Receipe", menuName = "SO/Create Item Receipe")]
public class ItemReceipe : ScriptableObject
{
    [Header("재료 아이템 목록")]
    [SerializeField] private IngredientData[] m_ingredient_item_list;
    public IngredientData[] Ingredients => m_ingredient_item_list;

    [Header("완성 아이템")]
    [SerializeField] private Item m_target_item;
    public Item Target => m_target_item;

    [Header("제작할 아이템의 개수")]
    [SerializeField] private int m_item_count;
    public int Count => m_item_count;

    [Header("제작에 걸리는 시간")]
    [SerializeField] private float m_crafting_time;
    public float Time => m_crafting_time;

    [Header("해금 레벨")]
    [SerializeField] private int m_constraint_level;
    public int Constraint => m_constraint_level;
}
