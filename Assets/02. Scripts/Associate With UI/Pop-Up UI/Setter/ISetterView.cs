public interface ISetterView : IPopupView
{
    void Inject(SetterPresenter presenter);
    void OpenUI(bool bgm_active, bool sfx_active, float bgm_rate, float sfx_rate);
    void UpdateUI(bool bgm_active, bool sfx_active, float bgm_rate, float sfx_rate);
    void CloseUI();
}