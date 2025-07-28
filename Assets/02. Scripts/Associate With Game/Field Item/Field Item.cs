using UnityEngine;

public class FieldItem : FieldObject
{
    [Space(30f)]
    [Header("아이템 이미지")]
    [SerializeField] private SpriteRenderer m_item_image;

    public Item Item { get; set; }

    public void Initialize(Item item)
    {
        Item = item;
        m_item_image.sprite = item.Sprite;
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            ObjectManager.Instance.ReturnObject(gameObject, ObjectType.ITEM);
        }
    }
}