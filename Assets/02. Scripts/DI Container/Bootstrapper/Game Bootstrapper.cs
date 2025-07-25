using UnityEngine;

public class GameBootstrapper : Bootstrapper
{
    protected override void Start()
    {
        base.Start();

        InjectPlayer();
    }

    private void InjectPlayer()
    {
        var player_ctrl = DIContainer.Resolve<PlayerCtrl>();

        var movement = DIContainer.Resolve<IMovement>();
        var attack = DIContainer.Resolve<IAttack>();
        var status = DIContainer.Resolve<IStatus>();

        player_ctrl.Inject(movement, attack, status);
    }
}
