using EquipmentService;

public class EquipmentPresenter : IPopupPresenter
{
    private IEquipmentView m_view;
    private IEquipmentService m_model;
    private PlayerCtrl m_player_ctrl;

    public EquipmentPresenter(IEquipmentView view, IEquipmentService model, PlayerCtrl player_ctrl)
    {
        m_view = view;
        m_model = model;
        m_player_ctrl = player_ctrl;

        m_model.OnUpdatedEffect += UpdateEffect;

        m_view.Inject(this);
        
        Initialize();
    }

    public void OpenUI()
    {
        Initialize();
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }

    public void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            m_model.InitializeSlot(i);
        }
    }

    public void UpdateEffect(EquipmentEffect equipment_effect)
    {
        m_player_ctrl.EquipmentEffect = equipment_effect;

        var max_hp = m_player_ctrl.Status.MaxHP;
        var max_mp = m_player_ctrl.Status.MaxMP;
        var atk = m_player_ctrl.Attack.ATK;
        var spd = m_player_ctrl.Movement.SPD;

        m_player_ctrl.Status.UpdateMaxStatus();

        m_view.UpdateEffect(max_hp, max_mp, atk, spd);
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }
}
