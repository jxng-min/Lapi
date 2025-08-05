using UnityEngine;

public class RigCtrl : MonoBehaviour
{
    [Header("회전 속도")]
    [SerializeField] private float m_rotate_speed = 10f;

    private void LateUpdate()
    {
        Rotation();
    }

    private void Rotation()
    {
        var mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (Vector2)(mouse_position - transform.position).normalized;

        var z_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        var target_rotation = Quaternion.Euler(0f, 0f, z_angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, target_rotation, Time.deltaTime * m_rotate_speed);
    }
}
