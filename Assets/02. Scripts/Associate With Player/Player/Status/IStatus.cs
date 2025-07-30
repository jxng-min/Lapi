using System;

public interface IStatus
{
    float MaxHP { get; }
    float MaxMP { get; }

    void UpdateHP(float amount);
    void UpdateMP(float amount);

    void UpdateMaxStatus();
}