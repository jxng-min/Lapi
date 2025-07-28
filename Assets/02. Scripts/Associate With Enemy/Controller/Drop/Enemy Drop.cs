using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    private EnemyCtrl m_controller;

    private int m_exp_amount;
    private int m_exp_deviation;

    private int m_gold_amount;
    private int m_gold_deviation;

    private List<ItemTable> m_drop_item_list;
    private float m_drop_rate; 

    private void Awake()
    {
        m_controller = GetComponent<EnemyCtrl>();
    }

    public void Initialize(int exp_amount, int exp_dev, int gold_amount, int gold_dev, List<ItemTable> drop_item_list, float drop_rate)
    {
        m_exp_amount = exp_amount;
        m_exp_deviation = exp_dev;

        m_gold_amount = gold_amount;
        m_gold_deviation = gold_dev;

        m_drop_item_list = drop_item_list;
        m_drop_rate = drop_rate;
    }

    public void Drop()
    {
        GetEXP();
        InstantiateCoin();
        InstantiateItem();
    }

    private void GetEXP()
    {
        var final_amount = Random.Range(m_exp_amount - m_exp_deviation, m_exp_amount + m_exp_deviation);
    }

    private void InstantiateCoin()
    {
        var final_amount = Random.Range(m_gold_amount - m_gold_deviation, m_gold_amount + m_gold_deviation);

        var gold_count = final_amount / 25;
        final_amount %= 25;

        var sliver_count = final_amount / 5;
        var bronze_count = final_amount % 5;

        for (int i = 0; i < gold_count; i++)
        {
            var gold_coin_obj = ObjectManager.Instance.GetObject(ObjectType.GOLD_COIN);
            gold_coin_obj.transform.position = transform.position;
        }

        for (int i = 0; i < sliver_count; i++)
        {
            var silver_coin_obj = ObjectManager.Instance.GetObject(ObjectType.SILVER_COIN);
            silver_coin_obj.transform.position = transform.position;
        }

        for (int i = 0; i < bronze_count; i++)
        {
            var bronze_coin_obj = ObjectManager.Instance.GetObject(ObjectType.BRONZE_COIN);
            bronze_coin_obj.transform.position = transform.position;
        }        
    }

    private void InstantiateItem()
    {
        float rate = Random.Range(0f, 100f);
        if (rate > m_drop_rate)
        {
            return;
        }

        float total_weight = 0f;
        foreach (var item in m_drop_item_list)
        {
            total_weight += item.Weight;
        }

        float pick = Random.Range(0f, total_weight);
        float current = 0f;

        foreach (var item in m_drop_item_list)
        {
            current += item.Weight;
            if (pick <= current)
            {
                var item_obj = ObjectManager.Instance.GetObject(ObjectType.ITEM);
                item_obj.transform.position = transform.position;

                var field_item = item_obj.GetComponent<FieldItem>();
                field_item.Initialize(item.Item);
            }
        }
    }
}
