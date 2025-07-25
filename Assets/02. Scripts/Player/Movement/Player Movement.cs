using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovement
{
    private PlayerCtrl m_controller;
    private float m_speed = 10f;

    public float SPD => m_speed;
    public Vector2 Direction { get; set; }

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
