public interface IEquipmentView : IPopupView
{
    void Inject(EquipmentPresenter presenter);

    void OpenUI();
    void CloseUI();

    void UpdateEffect(float max_hp, float max_mp, float atk, float spd);
}