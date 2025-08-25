public class GameBootstrapper : Bootstrapper
{
    protected override void Start()
    {
        base.Start();
        SoundManager.Instance.PlayBGM("Sprout Island");
    }
}