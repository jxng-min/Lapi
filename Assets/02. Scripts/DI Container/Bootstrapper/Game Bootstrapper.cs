using UserService;

public class GameBootstrapper : Bootstrapper
{
    protected override void Start()
    {
        ServiceLocator.Initialize();

        base.Start();

        InjectPlayer();
    }

    private void InjectPlayer()
    {
        var player_ctrl = DIContainer.Resolve<PlayerCtrl>();

        var movement = DIContainer.Resolve<IMovement>();
        var attack = DIContainer.Resolve<IAttack>();

        var status = DIContainer.Resolve<IStatus>();
        (status as PlayerStatus).Inject(ServiceLocator.Get<IUserService>());

        var default_status = DIContainer.Resolve<DefaultStatus>();
        var growth_status = DIContainer.Resolve<GrowthStatus>();

        player_ctrl.Inject(movement, attack, status, default_status, growth_status);
    }
}
