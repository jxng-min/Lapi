using System;
using UnityEngine;

public class Merchant : NPC, IDisposable
{
    [Space(50f)]
    [Header("상점 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("상점에서 판매할 아이템의 목록")]
    [SerializeField] protected Sale[] m_sale_list;

    private ShopPresenter m_shop_presenter;

    private void OnDestroy()
    {
        Dispose();
    }

    public void Inject(ShopPresenter shop_presenter)
    {
        m_shop_presenter = shop_presenter;

        OnCompletedDialogue += InjectToShop;
        OnCompletedDialogue += OpenShop;
    }

    public override void Interaction()
    {
        if (m_dialogue_presenter.IsOpen)
        {
            return;
        }

        Rotation();
        OpenDialogue();
    }

    private void InjectToShop()
    {
        m_shop_presenter.Inject(m_sale_list);
    }

    private void OpenShop()
    {
        m_ui_manager.OpenUI(m_shop_presenter);
    }

    public void Dispose()
    {
        OnCompletedDialogue -= InjectToShop;
        OnCompletedDialogue -= OpenShop;
    }
}
