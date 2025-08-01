using SkillService;
using UnityEngine;

public class FireBallStrategy : ItemStrategy, ISkillStrategy
{
    public override bool Activate(Item item)
    {
        return false;
    }

    public void Inject(ISkillService skill_service)
    {

    }
}
