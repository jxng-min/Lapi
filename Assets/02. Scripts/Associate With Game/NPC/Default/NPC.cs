using System;
using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NPC : MonoBehaviour
{
    [Header("의존성 관련 컴포넌트")]
    [Header("플레이어 컨트롤러")]
    [SerializeField] private PlayerCtrl m_player_ctrl;

    [Header("NPC 관련 컴포넌트")]
    [Header("NPC 코드")]
    [SerializeField] private NPCCode m_code;

    [Header("NPC 기본 방향")]
    [SerializeField] private Vector2 m_origin_direction;

    public event Action OnCompletedDialogue;
    private Animator m_animator;

    public NPCCode Code => m_code;

    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        ResetDirection();
    }

    public virtual void Interaction()
    {
        Rotation();
        OnCompletedDialogue?.Invoke();
    }

    protected void ResetDirection()
    {
        m_animator.SetFloat("DirX", m_origin_direction.x);
        m_animator.SetFloat("DirY", m_origin_direction.y);
    }

    protected void SetDirection(Vector2 direction)
    {
        m_animator.SetFloat("DirX", direction.x);
        m_animator.SetFloat("DirY", direction.y);        
    }

    protected void Rotation()
    {
        var target_direction = (Vector2)(m_player_ctrl.transform.position - transform.position).normalized;
        SetDirection(target_direction);
    }
}
