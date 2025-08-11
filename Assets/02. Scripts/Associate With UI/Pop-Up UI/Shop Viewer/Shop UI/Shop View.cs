using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ShopView : MonoBehaviour, IShopView
{
    [Header("상점 슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_root;

    [Header("상점 토글")]
    [SerializeField] private Toggle m_toggle;

    [Header("UI 닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private Animator m_animator;
    private ShopPresenter m_presenter;

    private List<ShopSlotView> m_shop_slot_list;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_shop_slot_list = new();
    }

    public void Inject(ShopPresenter presenter)
    {
        m_presenter = presenter;
        m_toggle.onValueChanged.AddListener((isOn) => m_presenter.OnChangedToggle(isOn));
        m_close_button.onClick.AddListener(m_presenter.CloseUI);
    }

    public void OpenUI()
    {
        m_animator.SetBool("Open", true);
    }

    public void CloseUI()
    {
        m_animator.SetBool("Open", false);
    }

    public IShopSlotView GetShopSlotView()
    {
        var shop_slot_obj = ObjectManager.Instance.GetObject(ObjectType.SHOP_SLOT);
        shop_slot_obj.transform.SetParent(m_slot_root);

        var shop_slot_view = shop_slot_obj.GetComponent<ShopSlotView>();
        m_shop_slot_list.Add(shop_slot_view);

        return shop_slot_view;
    }

    public IItemSlotView GetItemSlotView(int index)
    {
        return m_shop_slot_list[index].GetComponentInChildren<IItemSlotView>();
    }

    public void ReturnSlots()
    {
        var container = ObjectManager.Instance.GetPool(ObjectType.SHOP_SLOT).Container;

        foreach (var shop_slot in m_shop_slot_list)
        {
            shop_slot.transform.SetParent(container);
            ObjectManager.Instance.ReturnObject(shop_slot.gameObject, ObjectType.SHOP_SLOT);
        }

        m_shop_slot_list.Clear();
    }

    public void SetDepth()
    {
        (transform as RectTransform).SetAsFirstSibling();
    }
}