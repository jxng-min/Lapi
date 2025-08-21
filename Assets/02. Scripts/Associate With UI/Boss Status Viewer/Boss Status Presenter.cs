public class BossStatusPresenter
{
    private readonly IBossStatusView m_view;

    public BossStatusPresenter(IBossStatusView view)
    {
        m_view = view;
    }

    public void OpenUI(Enemy boss_so, BossCtrl boss_ctrl)
    {
        m_view.OpenUI(boss_so.Name);
        UpdateUI(boss_so, boss_ctrl);
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