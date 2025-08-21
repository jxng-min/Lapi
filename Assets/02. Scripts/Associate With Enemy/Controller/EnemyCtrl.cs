using InventoryService;
using QuestService;
using UnityEngine;
using UserService;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class EnemyCtrl : MonoBehaviour
{
    private EnemyStateContext m_state_context;

    #region States
    private IState<EnemyCtrl> m_idle_state;
    private IState<EnemyCtrl> m_move_state;
    private IState<EnemyCtrl> m_trace_state;
    private IState<EnemyCtrl> m_attack_state;
    private IState<EnemyCtrl> m_dead_state;
    #endregion States

    public EnemyMovement Movement { get; protected set; }
    public EnemyStatus Status { get; protected set; }
    public EnemyAttack Attack { get; protected set; }
    public EnemyDrop Drop { get; protected set; }
    public Pathfinder Pathfinder { get; protected set; }
    public PlayerCtrl Player { get; protected set; }

    public Enemy SO { get; protected set; }

    public IInventoryService InventoryService { get; protected set; }
    public IUserService UserService { get; protected set; }
    public IQuestService QuestService { get; protected set; }

    public Animator Animator { get; protected set; }
    public Rigidbody2D Rigidbody { get; protected set; }
    public CircleCollider2D Collider { get; protected set; }
    public SpriteRenderer Renderer { get; protected set; }

    public bool IsInit { get; protected set; }

    protected virtual void Awake()
    {
        m_state_context = new EnemyStateContext(this);

        m_idle_state = gameObject.AddComponent<EnemyIdleState>();
        m_move_state = gameObject.AddComponent<EnemyMoveState>();
        m_trace_state = gameObject.AddComponent<EnemyTraceState>();
        m_dead_state = gameObject.AddComponent<EnemyDeadState>();

        Movement = GetComponent<EnemyMovement>();
        Status = GetComponent<EnemyStatus>();
        Attack = GetComponent<EnemyAttack>();
        Drop = GetComponent<EnemyDrop>();
        
        Pathfinder = GetComponent<Pathfinder>();

        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<CircleCollider2D>();
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
                           PlayerCtrl player_ctrl)
    {
        SO = so;

        InventoryService = inventory_service;
        UserService = user_service;
        QuestService = quest_service;
        Player = player_ctrl;

        Animator.runtimeAnimatorController = SO.Animator;

        Movement.Initialize(SO.SPD);
        Status.Initialize(SO.HP);
        Attack.Initialize(SO.ATK, 8f);
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
        }
    }
}