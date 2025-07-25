using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Weapon : MonoBehaviour
{
    private bool m_can_use = true;
    private float m_cooltime;
    private Animator m_animator;

    protected bool CanUse
    {
        get => m_can_use;
        set => m_can_use = value;
    }

    protected Animator Animator => m_animator;

    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    #region Methods
    public void Initialize(float cooltime)
    {
        m_cooltime = cooltime;
    }

    public abstract void Use();

    protected void Cool()
    {
        StartCoroutine(SetCooling());
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
    }

    protected void InstantiateIndicator(Vector3 position, float amount)
    {
        // TODO: 데이지 인디케이터를 position 위치에 amount 수치만큼
    }
    #endregion Methods
}
