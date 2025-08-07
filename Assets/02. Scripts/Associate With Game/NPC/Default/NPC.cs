using System;
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

    [Header("NPC 기본 대화 ID")]
    [SerializeField] protected int m_dialogue_id = -1;

    public Action OnCompletedDialogue;
    private Animator m_animator;
    protected DialoguePresenter m_dialogue_presenter;

    public NPCCode Code => m_code;

    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();

        OnCompletedDialogue += ResetDirection;
    }

    protected virtual void Start()
    {
        ResetDirection();
    }

    public void Inject(DialoguePresenter dialogue_presenter)
    {
        m_dialogue_presenter = dialogue_presenter;
    }

    public virtual void Interaction()
    {
        Rotation();
        OpenDialogue();
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

    protected void OpenDialogue()
    {
        if (m_dialogue_id != -1)
        {
            if (m_dialogue_presenter.IsOpen)
            {
                return;
            }

            var target_position = (Vector2)transform.position + Vector2.up * 6f;
            m_dialogue_presenter.OpenUI(this, m_dialogue_id, new System.Numerics.Vector2(target_position.x, target_position.y));
        }
        else
        {
            OnCompletedDialogue?.Invoke();
        }
    }
}
