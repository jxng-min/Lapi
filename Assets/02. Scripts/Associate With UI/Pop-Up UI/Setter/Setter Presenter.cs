using SettingService;

public class SetterPresenter : IPopupPresenter
{
    private readonly ISetterView m_view;
    private readonly ISettingService m_setting_service;

    public SetterPresenter(ISetterView view,
                           ISettingService setting_service)
    {
        m_view = view;
        m_setting_service = setting_service;

        m_view.Inject(this);
    }

    public void OpenUI()
    {
        m_view.OpenUI(m_setting_service.Data.m_bgm_active,
                      m_setting_service.Data.m_sfx_active,
                      m_setting_service.Data.m_bgm_rate,
                      m_setting_service.Data.m_sfx_rate);
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }

    public void OnClickedBGM(bool isOn)
    {
        m_setting_service.Data.m_bgm_active = isOn;
        m_view.UpdateUI(m_setting_service.Data.m_bgm_active,
                        m_setting_service.Data.m_sfx_active,
                        m_setting_service.Data.m_bgm_rate,
                        m_setting_service.Data.m_sfx_rate);

        if (!isOn)
        {
            SoundManager.Instance.BGM.Pause();
        }
        else
        {
            SoundManager.Instance.BGM.Play();
        }
    }

    public void OnClickedSFX(bool isOn)
    {
        m_setting_service.Data.m_sfx_active = isOn;
        m_view.UpdateUI(m_setting_service.Data.m_bgm_active,
                        m_setting_service.Data.m_sfx_active,
                        m_setting_service.Data.m_bgm_rate,
                        m_setting_service.Data.m_sfx_rate);
    }

    public void OnValuedChangedBGM(float value)
    {
        m_setting_service.Data.m_bgm_rate = value;
        SoundManager.Instance.BGM.volume = value;
    }
    public void OnValuedChangedSFX(float value) => m_setting_service.Data.m_sfx_rate = value;
}
