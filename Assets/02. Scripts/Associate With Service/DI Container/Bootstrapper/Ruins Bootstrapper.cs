using UnityEngine;

public class RuinsBootstrapper : Bootstrapper
{
    protected override void Start()
    {
        base.Start();
        SoundManager.Instance.PlayBGM("Ruins");
    }
}