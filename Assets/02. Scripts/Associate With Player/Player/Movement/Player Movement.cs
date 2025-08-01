using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovement
{
    private PlayerCtrl m_controller;

    public float SPD => m_controller.DefaultStatus.SPD
                        + m_controller.DefaultStatus.SPD * (m_controller.EquipmentEffect.SPD / 100f);
    public Vector2 Direction { get; set; }
    public bool Controll { get; set; } = true;

    private void Awake()
    {
        m_controller = GetComponent<PlayerCtrl>();
    }

    private void Update()
    {
        InputMove();
    }

    private void FixedUpdate()
    {
        if (!Controll)
        {
            return;
        }

        Move();
    }

    #region Methods
    private void InputMove()
    {
        Direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    public void Move()
    {
        m_controller.Rigidbody.linearVelocity = Direction * SPD;
        m_controller.Animator.SetBool("Move", IsMove());
    }

    public bool IsMove()
    {
        return Direction.magnitude > 0f;
    }
    #endregion Methods
}
