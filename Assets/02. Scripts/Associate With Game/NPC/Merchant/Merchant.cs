using UnityEngine;

public class Merchant : NPC
{
    [Space(50f)]
    [Header("상점 관련 컴포넌트")]
    [Header("상점에서 판매할 아이템의 목록")]
    [SerializeField] protected Sale[] m_sale_list;

    private ShopPresenter m_shop_presenter;

    public void Inject(ShopPresenter shop_presenter)
    {
        m_shop_presenter = shop_presenter;
        OnCompletedDialogue += m_shop_presenter.OpenUI;
    }

    public override void Interaction()
    {
        m_shop_presenter.Inject(m_sale_list);

        base.Interaction();
    }
}
