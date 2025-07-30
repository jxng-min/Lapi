public class GameBootstrapper : Bootstrapper
{
    protected override void Start()
    {
        ServiceLocator.Initialize();

        base.Start();
    }
}
