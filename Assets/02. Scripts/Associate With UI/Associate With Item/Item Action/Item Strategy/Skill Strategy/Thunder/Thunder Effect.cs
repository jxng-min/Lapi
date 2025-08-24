using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(BoxCollider2D), typeof(CircleCollider2D))]
public class ThunderEffect : MonoBehaviour
{
    private Animator m_animator;
    private BoxCollider2D m_box_collider;
    private CircleCollider2D m_circle_collider;

    private float m_main_atk;
    private float m_sub_atk;

    private bool m_is_circle_collider_active;
    private HashSet<EnemyCtrl> m_enemy_set = new();
    private HashSet<BossCtrl> m_boss_set = new();
    private Coroutine m_damage_coroutine;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_box_collider = GetComponent<BoxCollider2D>();
        m_circle_collider = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        m_is_circle_collider_active = false;

        m_animator.SetTrigger("Play");

        m_damage_coroutine = StartCoroutine(Co_Damage());
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_is_circle_collider_active)
        {
            return;
        }

        if (collider.CompareTag("ENEMY"))
        {
            var enemy_ctrl = collider.GetComponent<EnemyCtrl>();
            enemy_ctrl.Status.UpdateHP(-m_main_atk, Vector2.zero);
        }
        else if (collider.CompareTag("BOSS"))
        {
            var boss_ctrl = collider.GetComponent<BossCtrl>();
            boss_ctrl.Status.UpdateHP(-m_main_atk);
        }

        InstantiateIndicator(collider.transform, -m_main_atk);
    }

    public void Inject(float main_atk, float sub_atk)
    {
        m_main_atk = main_atk;
        m_sub_atk = sub_atk;
    }

    public void EnableBoxCollider()
    {
        m_box_collider.enabled = true;
    }

    public void ToggleCollider()
    {
        m_box_collider.enabled = false;

        m_circle_collider.enabled = true;
        m_is_circle_collider_active = true;
    }

    public void DisableCircleCollider()
    {
        m_circle_collider.enabled = false;
        m_is_circle_collider_active = false;

        if (m_damage_coroutine != null)
        {
            StopCoroutine(m_damage_coroutine);
            m_damage_coroutine = null;
        }
    }

    public void Return()
    {
        ObjectManager.Instance.ReturnObject(gameObject, ObjectType.THUNDER);
    }

    private IEnumerator Co_Damage()
    {
        yield return new WaitUntil(() => m_is_circle_collider_active == true);

        while (m_is_circle_collider_active)
        {
            Vector2 center = m_circle_collider.transform.TransformPoint(m_circle_collider.offset);

            float radius = m_circle_collider.radius * Mathf.Max(
                m_circle_collider.transform.lossyScale.x,
                m_circle_collider.transform.lossyScale.y
            );

            m_enemy_set.Clear();
            m_boss_set.Clear();
            var enemy_objects = Physics2D.OverlapCircleAll(center, radius);
            for (int i = 0; i < enemy_objects.Length; i++)
            {
                if (enemy_objects[i].CompareTag("ENEMY"))
                {
                    var enemy_ctrl = enemy_objects[i].GetComponent<EnemyCtrl>();
                    if (enemy_ctrl != null)
                    {
                        m_enemy_set.Add(enemy_ctrl);
                    }
                }
                else if (enemy_objects[i].CompareTag("BOSS"))
                {
                    var boss_ctrl = enemy_objects[i].GetComponent<BossCtrl>();
                    if (boss_ctrl != null)
                    {
                        m_boss_set.Add(boss_ctrl);
                    }
                }
            }

            foreach (var enemy_ctrl in m_enemy_set)
            {
                enemy_ctrl.Status.UpdateHP(-m_sub_atk, Vector2.zero);
                InstantiateIndicator(enemy_ctrl.transform, -m_sub_atk);
            }

            foreach (var boss_ctrl in m_boss_set)
            {
                boss_ctrl.Status.UpdateHP(-m_sub_atk);
                InstantiateIndicator(boss_ctrl.transform, -m_sub_atk);                
            }

            yield return new WaitForSeconds(0.5f);
        }

        m_damage_coroutine = null;
    }

    private void InstantiateIndicator(Transform target, float amount)
    {
        var di_obj = ObjectManager.Instance.GetObject(ObjectType.DAMAGE_INDICATOR);
        di_obj.transform.SetParent(target);
        di_obj.transform.localPosition = Vector3.zero;

        var di = di_obj.GetComponent<DamageIndicator>();
        di.Initialize(amount);

        var damage = ObjectManager.Instance.GetObject(ObjectType.DAMAGE);
        damage.transform.SetParent(target);
        damage.transform.localPosition = Vector3.zero;
    }
}
