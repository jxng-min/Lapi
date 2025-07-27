using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CircleCollider2D))]
public class Hand : Weapon
{
    private CircleCollider2D m_collider;

    protected override void Awake()
    {
        base.Awake();
        m_collider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("ENEMY"))
        {
            var knockback_direction = (Vector2)(collider.transform.position - transform.parent.position).normalized;

            var enemy = collider.GetComponent<EnemyStatus>();
            enemy.UpdateHP(-ATK, knockback_direction);

            InstantiateIndicator(collider.transform, -ATK);
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
