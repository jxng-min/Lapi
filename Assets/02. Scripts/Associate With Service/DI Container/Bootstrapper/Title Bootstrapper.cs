public class TitleBootstrapper : Bootstrapper
{
    protected override void Awake()
    {
        base.Awake();
        
        ServiceLocator.Initialize();
    }
}
