public interface IStatusView
{
    void Inject(StatusPresenter presenter);
    void UpdateLV(int level, float exp_rate);
    void UpdateHP(float hp_rate);
    void UpdateMP(float mp_rate);
}