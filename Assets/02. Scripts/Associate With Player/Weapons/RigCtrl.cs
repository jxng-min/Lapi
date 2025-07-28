using UnityEngine;

public class RigCtrl : MonoBehaviour
{
    private void Update()
    {
        Rotation();
    }

    private void Rotation()
    {
        var mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (Vector2)(mouse_position - transform.position).normalized;
        var z_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, z_angle);
    }
}
