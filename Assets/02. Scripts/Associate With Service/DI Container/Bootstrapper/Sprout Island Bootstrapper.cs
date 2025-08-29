public class SproutIslandBootstrapper : Bootstrapper
{
    protected override void Start()
    {
        base.Start();
        SoundManager.Instance.PlayBGM("Sprout Island");
    }
}