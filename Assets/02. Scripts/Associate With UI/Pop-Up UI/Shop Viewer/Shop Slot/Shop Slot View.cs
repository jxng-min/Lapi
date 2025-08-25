using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotView : MonoBehaviour, IShopSlotView
{
    [Header("UI 관련 컴포넌트")]
    [Header("아이템 이름")]
    [SerializeField] private TMP_Text m_name_label;

    [Header("아이템 가격")]
    [SerializeField] private TMP_Text m_price_label;

    [Header("구매 버튼")]
    [SerializeField] private Button m_purchase_button;

    [Header("비활성화 패널")]
    [SerializeField] private GameObject m_disabled_panel;

    [Header("비활성화 패널 텍스트")]
    [SerializeField] private TMP_Text m_disabled_panel_text;

    private ShopSlotPresenter m_presenter;

    public void Inject(ShopSlotPresenter presenter)
    {
        m_presenter = presenter;
        m_purchase_button.onClick.AddListener(m_presenter.OnClickedPurchase);
        m_purchase_button.onClick.AddListener(() => SoundManager.Instance.PlaySFX("Shop Purchase"));
    }

    public void UpdateUI(string name, int price, bool purchase, bool is_constraint, int constraint_level = 1)
    {
        m_name_label.text = name;
        m_price_label.text = purchase ? $"가격: {NumberFormatter.FormatNumber(price)}" : $"가격: <color=red>{NumberFormatter.FormatNumber(price)}</color>";
        m_purchase_button.interactable = purchase;

        m_disabled_panel.SetActive(is_constraint);
        if (is_constraint)
        {
            m_disabled_panel_text.text = $"LV.{constraint_level}";
        }
    }

    public void DisableObject(bool disable)
    {
        gameObject.SetActive(!disable);
    }
}
