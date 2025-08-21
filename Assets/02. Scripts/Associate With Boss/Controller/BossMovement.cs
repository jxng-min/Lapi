using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    private BossCtrl m_controller;

    private List<Node> m_current_path;
    private Node m_current_node;
    private Coroutine m_move_coroutine;

    public float SPD { get; private set; }
    public Vector2 Direction { get; private set; }
    public bool IsMove { get; set; }

    private void Awake()
    {
        m_controller = GetComponent<BossCtrl>();
    }

    private void OnDisable()
    {
        Reset();
    }

    public void Initialize(float spd)
    {
        SPD = spd;
    }

    public void MoveAlongPath(List<Node> path)
    {
        if (path == null || path.Count == 0)
        {
            return;
        }
        m_current_path = path;

        if (m_move_coroutine != null)
        {
            StopCoroutine(m_move_coroutine);
            m_move_coroutine = null;
        }

        m_move_coroutine = StartCoroutine(Co_MoveAlongPath());
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

    public Vector3 GetRandomDestination()
    {
        List<Node> current_path;
        Vector2 offset;
        Vector3 destination;

        while (true)
        {
            offset = Random.insideUnitCircle * 4f;
            destination = transform.position + (Vector3)offset;

            current_path = m_controller.Pathfinder.Pathfind(transform.position, destination);
            if (current_path != null)
            {
                break;
            }
        }

        return destination;
    }

    private IEnumerator Co_MoveAlongPath()
    {
        IsMove = true;
        m_controller.Animator.SetBool("Move", true);

        var index = 0;
        while (index < m_current_path.Count)
        {
            yield return new WaitUntil(() => GameManager.Instance.Event != GameEventType.SETTING);

            var node = m_current_path[index];
            while (Vector2.Distance(transform.position, node.World) > 0.1f)
            {
                yield return new WaitUntil(() => GameManager.Instance.Event != GameEventType.SETTING);
                
                Direction = (node.World - (Vector2)transform.position).normalized;
                m_controller.Animator.SetFloat("DirX", Direction.x);
                m_controller.Animator.SetFloat("DirY", Direction.y);
                
                transform.position = Vector2.MoveTowards(transform.position, node.World, Time.deltaTime * SPD);

                yield return null;
            }

            index++;
        }

        IsMove = false;
        m_controller.Animator.SetBool("Move", false);
        m_move_coroutine = null;
    }    
}
