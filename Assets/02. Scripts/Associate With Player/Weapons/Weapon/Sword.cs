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
        if (collider.CompareTag("ENEMY"))
        {
            var knockback_direction = (Vector2)(collider.transform.position - transform.parent.position).normalized;

            var enemy = collider.GetComponent<EnemyStatus>();
            enemy.UpdateHP(-ATK, knockback_direction);

            InstantiateIndicator(collider.transform, -ATK);
        }
        else if (collider.CompareTag("BOSS"))
        {
            var boss = collider.GetComponent<BossStatus>();
            boss.UpdateHP(-ATK);

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
            SoundManager.Instance.PlaySFX("Sword");
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
