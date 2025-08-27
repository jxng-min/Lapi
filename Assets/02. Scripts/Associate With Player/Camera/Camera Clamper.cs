using System.Collections;
using UnityEngine;
using UserService;

public class CameraClamper : MonoBehaviour
{
    private CameraClamperHolder m_holder;
    private PlayerCtrl m_player_ctrl;

    [Header("고정 타겟 카메라")]
    [SerializeField] private Transform m_camera;

    [Header("카메라가 고정될 위치")]
    [SerializeField] private Transform m_clamped_transform;

    [Header("다음 스폰 위치")]
    [SerializeField] private Transform m_spawn_transform;

    [Header("맵 이름")]
    [SerializeField] private string m_map_name;

    private IUserService m_user_service;

    private void Awake()
    {
        m_holder = transform.parent.parent.GetComponent<CameraClamperHolder>();
        m_player_ctrl = m_holder.Player;

        m_user_service = ServiceLocator.Get<IUserService>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_player_ctrl.transform.position = m_spawn_transform.position;

            m_user_service.Position = m_spawn_transform.position;
            m_user_service.Camera = m_clamped_transform.position;
            m_user_service.Map = m_map_name;

            if (m_holder.WarpCoroutine != null)
            {
                m_holder.LastClamper.StopCoroutine(m_holder.WarpCoroutine);
                m_holder.WarpCoroutine = null;
            }

            m_holder.WarpCoroutine = StartCoroutine(TranslateCamera());
            m_holder.LastClamper = this;
        }
    }

    private IEnumerator TranslateCamera()
    {
        float elapsed_time = 0f;
        float target_time = 2f;

        while (elapsed_time <= target_time)
        {
            elapsed_time += Time.deltaTime;

            float delta = elapsed_time / target_time;

            float delta_x = Mathf.Lerp(m_camera.position.x, m_clamped_transform.position.x, Time.deltaTime * delta * 15f);
            float delta_y = Mathf.Lerp(m_camera.position.y, m_clamped_transform.position.y, Time.deltaTime * delta * 15f);

            m_camera.transform.position = new Vector3(delta_x, delta_y, m_camera.transform.position.z);

            yield return null;
        }

        m_camera.transform.position = m_clamped_transform.position;

        m_holder.WarpCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Portal.png", true);
        Gizmos.DrawIcon(m_clamped_transform.position, "Camera.png", true);
        Gizmos.DrawIcon(m_spawn_transform.position, "Portal.png", true);
    }
}