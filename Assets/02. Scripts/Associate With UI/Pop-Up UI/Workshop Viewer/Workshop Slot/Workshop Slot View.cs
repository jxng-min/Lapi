using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopSlotView : MonoBehaviour, IWorkshopSlotView
{
    [Header("재료 아이템 스크롤 바")]
    [SerializeField] private Scrollbar m_ingredient_scroll_bar;

    [Header("재료 아이템 슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_ingredient_slot_root;

    [Header("타겟 아이템 슬롯 뷰")]
    [SerializeField] private ItemSlotView m_target_item_slot_view;

    [Header("제작 진행도")]
    [SerializeField] private Image m_progress_image;

    [Header("제작 버튼")]
    [SerializeField] private Button m_craft_button;

    [Header("비활성화 패널")]
    [SerializeField] private GameObject m_disabled_panel;

    [Header("비활성화 패널 텍스트")]
    [SerializeField] private TMP_Text m_disabled_label;

    private WorkshopSlotPresenter m_presenter;
    private List<ItemSlotView> m_item_slot_list;

    private void Awake()
    {
        m_item_slot_list = new();
    }

    private void Update()
    {
        float speed = 2f;
        m_ingredient_scroll_bar.value = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
    }

    public void Inject(WorkshopSlotPresenter presenter)
    {
        m_presenter = presenter;

        m_craft_button.onClick.AddListener(m_presenter.OnClickedPurchase);
    }

    public void UpdateUI(bool can_craft, bool is_constraint, int constraint)
    {
        m_craft_button.interactable = can_craft;

        if (!is_constraint)
        {
            m_disabled_label.text = string.Empty;
            m_disabled_panel.SetActive(false);
        }
        else
        {
            m_disabled_panel.SetActive(true);
            m_disabled_label.text = $"LV.{constraint}";
        }
    }

    public void CraftItem(float craft_time)
    {
        StartCoroutine(Co_Crafting(craft_time));
    }

    private IEnumerator Co_Crafting(float craft_time)
    {
        m_presenter.ConsumeIngredient();
        m_craft_button.interactable = false;

        float elapsed_time = 0f;

        while (elapsed_time < craft_time)
        {
            float delta = elapsed_time / craft_time;

            m_progress_image.fillAmount = delta;

            elapsed_time += Time.deltaTime;
            yield return null;
        }

        m_craft_button.interactable = true;
        m_presenter.GetTarget();
    }

    public IItemSlotView GetIngredientItemSlotView()
    {
        var item_slot_obj = ObjectManager.Instance.GetObject(ObjectType.ITEM_SLOT);
        item_slot_obj.transform.SetParent(m_ingredient_slot_root);

        var item_slot_view = item_slot_obj.GetComponent<ItemSlotView>();
        m_item_slot_list.Add(item_slot_view);

        return item_slot_view;
    }

    public IItemSlotView GetTargetItemSlotView()
    {
        return m_target_item_slot_view;
    }

    public void ReturnItemSlots()
    {
        var container = ObjectManager.Instance.GetPool(ObjectType.ITEM_SLOT).Container;

        foreach (var item_slot in m_item_slot_list)
        {
            item_slot.transform.SetParent(container);
            ObjectManager.Instance.ReturnObject(item_slot.gameObject, ObjectType.ITEM_SLOT);
        }

        m_item_slot_list.Clear();
    }

    public void DisableObject(bool disable)
    {
        gameObject.SetActive(!disable);
    }
}
