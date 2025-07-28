using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyCtrl m_controller;

    private List<Node> m_current_path;
    private Node m_current_node;
    private Coroutine m_move_coroutine;

    public float SPD { get; private set; }
    public Vector2 Direction { get; private set; }
    public bool IsMove { get; set; }

    private void Awake()
    {
        m_controller = GetComponent<EnemyCtrl>();
    }

    private void OnDisable()
    {
        Reset();
    }

    public void Initialize(float spd)
    {
        SPD = spd;
    }

    public void Move()
    {
        m_current_path = m_controller.Pathfinder.Pathfind(transform.position, GetRandomDestination());
        if (m_current_path == null)
        {
            return;
        }

        MoveToDestination();
    }

    public void Trace(Vector3 target_position)
    {
        m_current_path = m_controller.Pathfinder.Pathfind(transform.position, target_position);
        if (m_current_path == null)
        {
            return;
        }

        MoveToDestination();
    }

    private void MoveToDestination()
    {
        IsMove = true;
        m_controller.Animator.SetBool("Move", true);

        if (m_move_coroutine != null)
        {
            StopCoroutine(m_move_coroutine);
            m_move_coroutine = null;
        }
        m_move_coroutine = StartCoroutine(Co_Move());
    }

    private Vector3 GetRandomDestination()
    {
        var offset = Random.insideUnitCircle * 4f;

        return transform.position + (Vector3)offset;
    }

    public void Reset()
    {
        IsMove = false;

        if (m_move_coroutine != null)
        {
            StopCoroutine(m_move_coroutine);
            m_move_coroutine = null;
        }

        m_controller.Rigidbody.linearVelocity = Vector2.zero;
        m_controller.Rigidbody.angularVelocity = 0f;
    }

    private IEnumerator Co_Move()
    {
        var index = 0;

        while (true)
        {
            if (m_current_path == null || m_current_path.Count == 0)
            {
                IsMove = false;
                m_move_coroutine = null;

                yield break;
            }

            if (index >= m_current_path.Count)
            {
                IsMove = false;
                m_move_coroutine = null;

                m_controller.ChangeState(EnemyState.IDLE);

                yield break;                
            }

            m_current_node = m_current_path[index];

            if (Vector2.Distance(transform.position, m_current_node.World) <= 0.1f)
            {
                index++;
                continue;
            }

            Direction = (m_current_node.World - (Vector2)transform.position).normalized;
            m_controller.Animator.SetFloat("DirX", Direction.x);
            m_controller.Animator.SetFloat("DirY", Direction.y);

            if (!m_controller.Status.IsKnockback)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_current_node.World, Time.deltaTime * SPD);
            }

            yield return null;
        }
    }
}
