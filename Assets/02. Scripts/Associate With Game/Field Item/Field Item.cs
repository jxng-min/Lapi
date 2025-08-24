using System.Collections;
using InventoryService;
using QuestService;
using UnityEngine;

public class FieldItem : FieldObject
{
    [Space(30f)]
    [Header("아이템 이미지")]
    [SerializeField] private SpriteRenderer m_item_image;
    private IInventoryService m_inventory_service;
    private IQuestService m_quest_service;

    public Item Item { get; set; }

    public void Initialize(Item item,
                           IInventoryService inventory_service,
                           IQuestService quest_service)
    {
        Item = item;
        m_item_image.sprite = item.Sprite;

        m_inventory_service = inventory_service;
        m_quest_service = quest_service;

        StartCoroutine(Co_Return());
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            m_quest_service.UpdateItemCount(Item.Code);
            m_inventory_service.AddItem(Item.Code, 1);
            ObjectManager.Instance.ReturnObject(gameObject, ObjectType.ITEM);
        }
    }

    private IEnumerator Co_Return()
    {
        float elapsed_time = 0f;
        float target_time = 20f;
        SetAlpha(1f);

        while (elapsed_time < target_time)
        {
            if (target_time - elapsed_time < 3f)
            {
                var delta = elapsed_time / target_time;
                SetAlpha(1f - delta);
            }

            elapsed_time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(0f);
        ObjectManager.Instance.ReturnObject(gameObject, ObjectType.ITEM);
    }

    private void SetAlpha(float alpha)
    {
        var color = m_item_image.color;
        color.a = alpha;
        m_item_image.color = color;
    }
}