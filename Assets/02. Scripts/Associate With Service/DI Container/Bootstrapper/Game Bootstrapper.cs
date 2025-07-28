using InventoryService;
using UserService;

public class GameBootstrapper : Bootstrapper
{
    protected override void Start()
    {
        ServiceLocator.Initialize();

        base.Start();

        InjectPlayer();
        InjectInventory();
    }

    private void InjectPlayer()
    {
        var player_ctrl = DIContainer.Resolve<PlayerCtrl>();

        var movement = DIContainer.Resolve<IMovement>();

        var attack = DIContainer.Resolve<IAttack>();
        var weapon = DIContainer.Resolve<Weapon>();
        (attack as PlayerAttack).Inject(ServiceLocator.Get<IUserService>(), weapon);

        var status = DIContainer.Resolve<IStatus>();
        (status as PlayerStatus).Inject(ServiceLocator.Get<IUserService>());

        var default_status = DIContainer.Resolve<DefaultStatus>();
        var growth_status = DIContainer.Resolve<GrowthStatus>();

        player_ctrl.Inject(movement, attack, status, default_status, growth_status);

        (status as PlayerStatus).Initialize();
    }

    private void InjectInventory()
    {
        var item_db = DIContainer.Resolve<ItemDataBase>();

        var inventory_model = DIContainer.Resolve<IInventoryService>();
        inventory_model.Inject(item_db);

        var inventory_presenter = DIContainer.Resolve<InventoryPresenter>();

        var inventory_view = DIContainer.Resolve<IInventoryView>();
        inventory_view.Inject(inventory_presenter);
    }
}
