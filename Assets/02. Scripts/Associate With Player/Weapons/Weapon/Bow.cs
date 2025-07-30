using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Bow : Weapon
{
    [Header("플레이어 컨트롤러")]
    [SerializeField] private PlayerCtrl m_player_ctrl;

    private float m_arrow_speed = 100f;

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


    #endregion Methods
    public void Shoot()
    {
        var ATK = m_player_ctrl.Attack.ATK;

        var arrow_obj = ObjectManager.Instance.GetObject(ObjectType.ARROW);
        arrow_obj.transform.position = transform.position;

        var last_point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)(last_point - transform.parent.position).normalized;

        var arrow = arrow_obj.GetComponent<Arrow>();
        arrow.Initialize(ATK, m_arrow_speed, direction);
    }
}
