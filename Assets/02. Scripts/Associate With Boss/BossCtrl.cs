using InventoryService;
using QuestService;
using UnityEngine;
using UserService;

public class BossCtrl : MonoBehaviour
{
    private BossStateContext m_state_context;

    private IState<BossCtrl> m_idle_state;
    private IState<BossCtrl> m_move_state;
    private IState<BossCtrl> m_trace_state;
    private IState<BossCtrl> m_attack_state;
    private IState<BossCtrl> m_dead_state;
    private IState<BossCtrl> m_recovery_state;

    public BossMovement Movement { get; protected set; }
    public BossStatus Status { get; protected set; }
    public BossAttack Attack { get; protected set; }
    public BossDrop Drop { get; protected set; }
    public Pathfinder Pathfinder { get; protected set; }
    public PlayerCtrl Player { get; protected set; }
    public BossStatusPresenter StatusPresenter { get; protected set; }

    public Enemy SO { get; protected set; }

    public IInventoryService InventoryService { get; protected set; }
    public IUserService UserService { get; protected set; }
    public IQuestService QuestService { get; protected set; }

    public Animator Animator { get; protected set; }
    public Rigidbody2D Rigidbody { get; protected set; }
    public CapsuleCollider2D Collider { get; protected set; }
    public SpriteRenderer Renderer { get; protected set; }

    public bool IsInit { get; protected set; }

    public Vector3 OriginPosition { get; protected set; }

    protected virtual void Awake()
    {
        m_state_context = new BossStateContext(this);

        m_idle_state = gameObject.AddComponent<BossIdleState>();
        m_move_state = gameObject.AddComponent<BossMoveState>();
        m_trace_state = gameObject.AddComponent<BossTraceState>();
        m_dead_state = gameObject.AddComponent<BossDeadState>();
        m_recovery_state = gameObject.AddComponent<BossRecoveryState>();

        Movement = GetComponent<BossMovement>();
        Status = GetComponent<BossStatus>();
        Attack = GetComponent<BossAttack>();
        Drop = GetComponent<BossDrop>();

        Pathfinder = GetComponent<Pathfinder>();

        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<CapsuleCollider2D>();
        Renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        IsInit = false;

        Animator.speed = 1f;
        Rigidbody.simulated = true;
        Collider.enabled = true;
        Renderer.color = Color.white;

        Status.IsDead = false;

        var grid = FindFirstObjectByType<GridMap>();
        Pathfinder.Inject(grid);
    }

    private void OnDisable()
    {
        IsInit = false;
    }

    public void Initialize(Enemy so,
                           IInventoryService inventory_service,
                           IUserService user_service,
                           IQuestService quest_service,
                           BossStatusPresenter status_presenter,
                           PlayerCtrl player_ctrl,
                           Vector3 origin_position)
    {
        SO = so;

        InventoryService = inventory_service;
        UserService = user_service;
        QuestService = quest_service;
        StatusPresenter = status_presenter;
        Player = player_ctrl;
        OriginPosition = origin_position;        

        Movement.Initialize(SO.SPD);
        Status.Initialize(SO.HP);
        Attack.Initialize(SO.ATK, 25f);
        Drop.Initialize(SO.EXP, SO.EXP_DEV, SO.GOLD, SO.GOLD_DEV, SO.DropList, SO.DropRate);

        IsInit = true;
        ChangeState(EnemyState.IDLE);
    }

    public void ChangeState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.IDLE:
                m_state_context.Transition(m_idle_state);
                break;

            case EnemyState.MOVE:
                m_state_context.Transition(m_move_state);
                break;

            case EnemyState.TRACE:
                m_state_context.Transition(m_trace_state);
                break;

            case EnemyState.ATTACK:
                m_state_context.Transition(m_attack_state);
                break;

            case EnemyState.DEAD:
                m_state_context.Transition(m_dead_state);
                break;

            case EnemyState.RECOVERY:
                m_state_context.Transition(m_recovery_state);
                break;
        }
    }    
}