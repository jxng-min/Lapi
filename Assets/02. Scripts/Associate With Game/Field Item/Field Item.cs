using InventoryService;
using UnityEngine;

public class FieldItem : FieldObject
{
    [Space(30f)]
    [Header("아이템 이미지")]
    [SerializeField] private SpriteRenderer m_item_image;
    private IInventoryService m_inventory_service;

    public Item Item { get; set; }

    public void Initialize(Item item, IInventoryService inventory_service)
    {
        Item = item;
        m_item_image.sprite = item.Sprite;

        m_inventory_service = inventory_service;
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            m_inventory_service.AddItem(Item.Code, 1);
            ObjectManager.Instance.ReturnObject(gameObject, ObjectType.ITEM);
        }
    }
}