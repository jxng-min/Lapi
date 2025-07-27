using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Bow : Weapon
{
    private Vector2 m_last_direction;

    #region Methods
    public override void Use()
    {
        if (CanUse)
        {
            CanUse = false;
            Animator.SetTrigger("Attack");
            Cool();

            m_last_direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }


    #endregion Methods
    public void Shoot()
    {
        // TODO: 화살 인스턴스 소환 및 발사
    }
}
