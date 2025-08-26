using SettingService;
using UnityEngine;

public class SetterInstaller : MonoBehaviour, IInstaller
{
    [Header("세터 뷰")]
    [SerializeField] private SetterView m_setter_view;

    public void Install()
    {
        var setter_presenter = new SetterPresenter(m_setter_view,
                                                   ServiceLocator.Get<ISettingService>());
    }
}
