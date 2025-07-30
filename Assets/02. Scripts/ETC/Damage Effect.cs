using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DamageEffect : MonoBehaviour
{
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_animator.SetTrigger("Play");
    }

    public void Return()
    {
        var container = ObjectManager.Instance.GetPool(ObjectType.DAMAGE).Container;
        transform.SetParent(container);
        transform.localPosition = Vector3.zero;

        ObjectManager.Instance.ReturnObject(gameObject, ObjectType.DAMAGE);
    }
}
