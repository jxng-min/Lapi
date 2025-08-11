using UnityEngine;

[DisallowMultipleComponent]
public class PlayerCtrl : MonoBehaviour
{
    public IMovement Movement { get; private set; }
    public IAttack Attack { get; private set; }
    public IStatus Status { get; private set; }

    public DefaultStatus DefaultStatus { get; private set; }
    public GrowthStatus GrowthStatus { get; private set; }
    public EquipmentEffect EquipmentEffect { get; set; }

    public Rigidbody2D Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public SpriteRenderer Renderer { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.PLAYING, GameManager.Instance.Playing);
        GameEventBus.Subscribe(GameEventType.INTERACTING, GameManager.Instance.Interacting);
        GameEventBus.Subscribe(GameEventType.EVENT, GameManager.Instance.Events);
        GameEventBus.Subscribe(GameEventType.SETTING, GameManager.Instance.Setting);

        GameEventBus.Publish(GameEventType.PLAYING);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.PLAYING, GameManager.Instance.Playing);
        GameEventBus.Unsubscribe(GameEventType.INTERACTING, GameManager.Instance.Interacting);
        GameEventBus.Unsubscribe(GameEventType.EVENT, GameManager.Instance.Events);
        GameEventBus.Unsubscribe(GameEventType.SETTING, GameManager.Instance.Setting);        
    }

    private void Update()
    {
        if (GameManager.Instance.Event != GameEventType.PLAYING)
        {
            return;
        }

        SetAnime();
    }

    public void Inject(IMovement movement, IAttack attack, IStatus status, DefaultStatus default_status, GrowthStatus growth_status)
    {
        Movement = movement;
        Attack = attack;
        Status = status;

        DefaultStatus = default_status;
        GrowthStatus = growth_status;
    }

    public void SetAnime()
    {
        var mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse_position.z = 0f;

        var player_direction = (Vector2)(mouse_position - transform.position).normalized;
        Animator.SetFloat("DirX", player_direction.x);
        Animator.SetFloat("DirY", player_direction.y);
    }
}
