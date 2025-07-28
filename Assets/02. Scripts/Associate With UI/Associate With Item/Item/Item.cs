using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "SO/Create Generic Item")]
public class Item : ScriptableObject
{
    [Header("아이템 기본 정보")]
    [Header("아이템 코드")]
    [SerializeField] private ItemCode m_code;
    public ItemCode Code => m_code;

    [Header("아이템 타입")]
    [SerializeField] private ItemType m_type;
    public ItemType Type => m_type;

    [Header("아이템 명")]
    [SerializeField] private string m_name;
    public string Name => m_name;

    [Header("슬롯 중첩 여부")]
    [SerializeField] private bool m_stackable;
    public bool Stackable => m_stackable;

    [Header("아이템 쿨타임")]
    [SerializeField] private float m_cool = -1f;
    public float Cool => m_cool;

    [Header("아이템 이미지")]
    [SerializeField] private Sprite m_sprite;
    public Sprite Sprite => m_sprite;
}