using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DamageIndicator : MonoBehaviour
{
    [Header("데미지")]
    [SerializeField] private TMP_Text m_damage_label;

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_animator.SetTrigger("Up");
    }

    #region Methods
    public void Initialize(float damage)
    {
        if (damage < 0f)
        {
            m_damage_label.text = $"<color=red>{NumberFormatter.FormatNumber(damage)}</color>";
        }
        else if (damage > 0f)
        {
            m_damage_label.text = $"<color=green>{NumberFormatter.FormatNumber(damage)}</color>";
        }
        else
        {
            m_damage_label.text = $"<color=blue>MISS</color>";
        }
    }

    public void Return()
    {
        var container = ObjectManager.Instance.GetPool(ObjectType.DAMAGE_INDICATOR).Container;
        gameObject.transform.SetParent(container);

        ObjectManager.Instance.ReturnObject(gameObject, ObjectType.DAMAGE_INDICATOR);
    }
    #endregion Methods
}
