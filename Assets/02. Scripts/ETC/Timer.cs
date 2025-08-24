using UnityEngine;
using UserService;

public class Timer : MonoBehaviour
{
    private IUserService m_user_service;

    public void Inject(IUserService user_service)
    {
        m_user_service = user_service;
    }

    public void Update()
    {
        if (GameManager.Instance.Event != GameEventType.SETTING)
        {
            m_user_service.PlayTime += Time.deltaTime;
        }
    }
}