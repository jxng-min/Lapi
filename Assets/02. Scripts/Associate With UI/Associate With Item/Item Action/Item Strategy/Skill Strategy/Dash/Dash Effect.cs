using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DashEffect : MonoBehaviour
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
        ObjectManager.Instance.ReturnObject(gameObject, ObjectType.DASH);
    }
}
