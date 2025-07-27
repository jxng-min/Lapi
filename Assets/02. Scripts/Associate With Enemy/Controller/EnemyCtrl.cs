using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class EnemyCtrl : MonoBehaviour
{
    private EnemyStateContext m_state_context;

    #region States
    private IState<EnemyCtrl> m_idle_state;
    private IState<EnemyCtrl> m_move_state;
    private IState<EnemyCtrl> m_trace_state;
    private IState<EnemyCtrl> m_attack_state;
    #endregion States

    public EnemyMovement Movement { get; private set; }
    public EnemyStatus Status { get; private set; }
    public EnemyAttack Attack { get; private set; }
    public Pathfinder Pathfinder { get; private set; }
    [field: SerializeField] public MeleeEnemy SO { get; private set; }

    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public CircleCollider2D Collider { get; private set; }
    public SpriteRenderer Renderer { get; private set; }

    protected virtual void Awake()
    {
        m_state_context = new EnemyStateContext(this);

        m_idle_state = gameObject.AddComponent<EnemyIdleState>();
        m_move_state = gameObject.AddComponent<EnemyMoveState>();
        m_trace_state = gameObject.AddComponent<EnemyTraceState>();

        Movement = GetComponent<EnemyMovement>();
        Status = GetComponent<EnemyStatus>();
        Attack = GetComponent<EnemyAttack>();
        Pathfinder = GetComponent<Pathfinder>();

        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<CircleCollider2D>();
        Renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Animator.speed = 1f;
        Rigidbody.simulated = true;
        Collider.enabled = true;
        Renderer.color = Color.white;

        var grid = FindFirstObjectByType<GridMap>();
        Pathfinder.Inject(grid);

        Initialize(SO);

        ChangeState(EnemyState.IDLE);
    }

    private void Update()
    {
        m_state_context?.ExecuteUpdate();
    }

    public void Initialize(MeleeEnemy so)
    {
        SO = so;

        Animator.runtimeAnimatorController = SO.Animator;

        Movement.Initialize(SO.SPD);
        Status.Initialize(SO.HP);
        Attack.Initialize(SO.ATK);
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
        }
    }
}