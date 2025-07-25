using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PolygonCollider2D))]
public class Sword : Weapon
{
    private PolygonCollider2D m_collider;

    protected override void Awake()
    {
        base.Awake();
        m_collider = GetComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            // TODO: 공격 처리 ㅠ
        }
    }

    #region Methods
    public override void Use()
    {
        if (CanUse)
        {
            CanUse = false;
            Animator.SetTrigger("Attack");
            Cool();
        }
    }

    public void EnableCollider()
    {
        m_collider.enabled = true;
    }

    public void DisableCollider()
    {
        m_collider.enabled = false;
    }
    #endregion Methods
}
