using UnityEngine;

public class Merchant : NPC
{
    [Space(50f)]
    [Header("상점 관련 컴포넌트")]
    [Header("상점에서 판매할 아이템의 목록")]
    [SerializeField] protected Sale[] m_sale_list;

    // private ShopPresenter m_shop_presenter;

    protected override void Start()
    {
        base.Start();

        // OnCompletedDialogue += m_shop_presenter.OpenUI;
    }
}
