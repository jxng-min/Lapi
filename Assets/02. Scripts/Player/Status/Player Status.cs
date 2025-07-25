using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IStatus
{
    public Action<float, float> OnUpdatedHP;
    public Action<float, float> OnUpdatedMP;

    public void UpdateHP(float amount)
    {
        throw new NotImplementedException();
    }

    public void UpdateMP(float amount)
    {
        throw new NotImplementedException();
    }
}
