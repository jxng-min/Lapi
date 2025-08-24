using System.Collections;
using UnityEngine;

public class WendigoAttackState : MonoBehaviour, IState<BossCtrl>
{
    private WendigoCtrl m_controller;
    private Coroutine m_block_coroutine;

    public void ExecuteEnter(BossCtrl sender)
    {
        if (m_controller == null)
        {
            m_controller = sender as WendigoCtrl;
        }

        Attack();
    }

    public void ExecuteExit()
    {
        if (m_block_coroutine != null)
        {
            StopCoroutine(m_block_coroutine);
            m_block_coroutine = null;
        }
    }

    private void Attack()
    {
        if (!m_controller.Attack.CanAttack())
        {
            m_controller.ChangeState(EnemyState.TRACE);
        }

        if (m_block_coroutine != null)
        {
            m_controller.ChangeState(EnemyState.TRACE);
        }

        var magnitude = ((Vector2)(transform.position - m_controller.Player.transform.position)).magnitude;

        if (magnitude <= 8f)
        {
            Earthquake();
        }
        else if (magnitude <= 10f)
        {
            Swing();
        }
        else
        {
            m_controller.ChangeState(EnemyState.TRACE);
        }
    }

    private void Earthquake()
    {
        m_controller.Animator.SetTrigger("Earthquake");
        m_block_coroutine = StartCoroutine(Co_Blocking(2f));
    }

    private void Swing()
    {
        m_controller.Animator.SetTrigger("Swing");
        m_block_coroutine = StartCoroutine(Co_Blocking(4f));
    }

    private IEnumerator Co_Blocking(float target_time)
    {
        float elasped_time = 0f;

        m_controller.Animator.SetBool("Move", false);

        while (elasped_time <= target_time)
        {
            yield return new WaitUntil(() => GameManager.Instance.Event != GameEventType.SETTING);

            elasped_time += Time.deltaTime;
            yield return null;
        }

        m_controller.ChangeState(EnemyState.TRACE);
    }
}
