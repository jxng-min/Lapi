public class BossStatusPresenter
{
    private readonly IBossStatusView m_view;

    public BossStatusPresenter(IBossStatusView view)
    {
        m_view = view;
    }

    public void OpenUI(Enemy boss_so)
    {
        m_view.OpenUI(boss_so.Name);
    }

    public void UpdateUI(Enemy boss_so, BossCtrl boss_ctrl)
    {
        var hp_rate = boss_ctrl.Status.HP / boss_so.HP;

        m_view.UpdateUI(hp_rate);
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }
}