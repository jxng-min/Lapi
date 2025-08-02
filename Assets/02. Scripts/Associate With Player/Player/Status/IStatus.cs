using System;

public interface IStatus
{
    float HP { get; }
    float MP { get; }
    float MaxHP { get; }
    float MaxMP { get; }

    void UpdateHP(float amount);
    void UpdateMP(float amount);

    void UpdateMaxStatus();
}