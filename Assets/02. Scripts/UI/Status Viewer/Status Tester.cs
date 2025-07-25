using UnityEngine;

public class StatusTester : MonoBehaviour
{
    public PlayerStatus status;

    public void HP()
    {
        status.UpdateHP(-50);
    }

    public void MP()
    {
        status.UpdateMP(-30);
    }
}
