using System.Collections;
using InventoryService;
using UnityEngine;

public class FieldCoin : FieldObject
{
    [Space(30)]
    [Header("코인 관련")]
    [Header("코인의 가치")]
    [SerializeField] private int m_amount;

    [Header("코인의 오브젝트 타입")]
    [SerializeField] private ObjectType m_type;

    [Header("코인의 이미지")]
    [SerializeField] private SpriteRenderer m_coin_image;

    [Header("그림자 이미지")]
    [SerializeField] private SpriteRenderer m_shadow_image;

    private IInventoryService m_inventory_service;

    public int Amount
    {
        get => m_amount;
        set => m_amount = value;
    }

    public ObjectType Type
    {
        get => m_type;
        set => m_type = value;
    }

    public void Initialize(IInventoryService inventory_service)
    {
        m_inventory_service = inventory_service;

        StartCoroutine(Co_Return());
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            m_inventory_service.UpdateGold(Amount);
            ObjectManager.Instance.ReturnObject(gameObject, Type);
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
        ObjectManager.Instance.ReturnObject(gameObject, Type);
    }

    private void SetAlpha(float alpha)
    {
        var color = m_coin_image.color;
        color.a = alpha;
        
        m_coin_image.color = color;
        m_shadow_image.color = color;
    }
}