using System;
using QuestService;
using UnityEngine;
using UserService;

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

    [Header("NPC가 가지는 퀘스트 목록")]
    [SerializeField] private Quest[] m_quests;

    public Action OnCompletedDialogue;
    private Animator m_animator;
    protected DialoguePresenter m_dialogue_presenter;
    protected IUserService m_user_service;
    protected IQuestService m_quest_service;

    public NPCCode Code => m_code;

    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();

        OnCompletedDialogue += ResetDirection;
        OnCompletedDialogue += UpdateQuestProgress;
    }

    protected virtual void Start()
    {
        ResetDirection();
    }

    protected virtual void OnDestroy()
    {
        OnCompletedDialogue -= ResetDirection;
        OnCompletedDialogue -= UpdateQuestProgress;        
    }

    public void Inject(DialoguePresenter dialogue_presenter,
                       IUserService user_service,
                       IQuestService quest_service)
    {
        m_dialogue_presenter = dialogue_presenter;
        m_user_service = user_service;
        m_quest_service = quest_service;
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

    protected virtual void OpenDialogue()
    {
        if (IsExistQuest(out var quest))
        {
            DialogueAboutQuest(quest);
        }
        else
        {
            Dialogue(m_dialogue_id);
        }
    }

    protected void Dialogue(int dialogue_id)
    {
        if (dialogue_id != -1)
        {
            if (m_dialogue_presenter.IsOpen)
            {
                return;
            }

            SetDialogueUI(dialogue_id);
        }
        else
        {
            OnCompletedDialogue?.Invoke();
        }
    }

    protected void DialogueAboutQuest(Quest quest)
    {
        var dialogue_id = -1;
        switch (m_quest_service.GetQuestState(quest.ID))
        {
            case QuestState.NONE:
                dialogue_id = quest.StartDialogue;
                break;

            case QuestState.IN_PROGRESS:
                dialogue_id = quest.ProgressDialogue;
                break;

            case QuestState.CAN_CLEAR:
                dialogue_id = quest.CanClearDialogue;
                break;
        }

        Debug.Log($"{quest.ID}, {m_quest_service.GetQuestState(quest.ID)}");

        Dialogue(dialogue_id);
    }

    protected bool IsExistQuest(out Quest quest)
    {
        quest = null;

        for (int i = 0; i < m_quests.Length; i++)
        {
            if (m_quests[i].Level <= m_user_service.Status.Level)
            {
                if (m_quest_service.GetQuestState(m_quests[i].ID) == QuestState.CLEARED)
                {
                    continue;
                }
                
                for (int j = 0; j < m_quests[i].Previous.Length; j++)
                {
                    if (m_quest_service.GetQuestState(m_quests[i].Previous[j].ID) != QuestState.CLEARED)
                    {
                        return false;
                    }
                }

                quest = m_quests[i];
                return true;
            }
        }

        return false;
    }

    protected void SetDialogueUI(int dialogue_id)
    {
        var target_position = (Vector2)transform.position + Vector2.up * 6f;
        m_dialogue_presenter.OpenUI(this, dialogue_id, new System.Numerics.Vector2(target_position.x, target_position.y));
    }

    private void UpdateQuestProgress()
    {
        if (IsExistQuest(out var quest))
        {
            var state = m_quest_service.GetQuestState(quest.ID);

            switch (state)
            {
                case QuestState.NONE:
                    m_quest_service.AddQuest(quest);
                    break;

                case QuestState.CAN_CLEAR:
                    m_quest_service.UpdateQuestState(quest.ID, QuestState.CLEARED);
                    m_quest_service.ClaimReward(quest);
                    m_quest_service.ClaimSubmit(quest);
                    break;
            }
        }
    }
}
