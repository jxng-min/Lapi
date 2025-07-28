using UnityEngine;

public class TitleBootstrapper : Bootstrapper
{
    protected override void Start()
    {
        ServiceLocator.Initialize();

        base.Start();
    }


}
