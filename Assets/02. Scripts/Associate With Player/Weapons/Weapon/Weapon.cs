using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Weapon : MonoBehaviour
{
    private bool m_can_use = true;
    private float m_cooltime;
    private Animator m_animator;
    private PlayerAttack m_player_attack;
    private Coroutine m_cool_coroutine;

    protected bool CanUse
    {
        get => m_can_use;
        set => m_can_use = value;
    }

    protected Animator Animator => m_animator;
    protected float ATK => m_player_attack.ATK;

    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_player_attack = transform.parent.parent.GetComponent<PlayerAttack>();
    }

    private void OnDisable()
    {
        m_can_use = true;
        
        if (m_cool_coroutine != null)
        {
            StopCoroutine(m_cool_coroutine);
            m_cool_coroutine = null;
        }
    }

    #region Methods
    public void Initialize(float cooltime)
    {
        m_cooltime = cooltime;
    }

    public abstract void Use();

    protected void Cool()
    {
        m_cool_coroutine = StartCoroutine(SetCooling());
    }

    private IEnumerator SetCooling()
    {
        var elapsed_time = 0f;

        while (elapsed_time <= m_cooltime)
        {
            elapsed_time += Time.deltaTime;

            yield return null;
        }

        m_can_use = true;
        m_cool_coroutine = null;
    }

    protected void InstantiateIndicator(Transform target, float amount)
    {
        var di_obj = ObjectManager.Instance.GetObject(ObjectType.DAMAGE_INDICATOR);
        di_obj.transform.SetParent(target);
        di_obj.transform.localPosition = Vector3.zero;

        var di = di_obj.GetComponent<DamageIndicator>();
        di.Initialize(amount);

        var damage = ObjectManager.Instance.GetObject(ObjectType.DAMAGE);
        damage.transform.SetParent(target);
        damage.transform.localPosition = Vector3.zero;
    }
    #endregion Methods
}
