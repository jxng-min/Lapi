using UnityEngine;

public class EarthquakeEffect : MonoBehaviour
{
    [SerializeField] private BossCtrl m_boss_ctrl;
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_animator.SetTrigger("Enable");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerStatus>().UpdateHP(-m_boss_ctrl.Attack.ATK * 0.8f);
        }
    }
}
