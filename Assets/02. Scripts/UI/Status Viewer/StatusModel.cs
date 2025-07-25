using System;

public class StatusModel
{
    private PlayerStatus m_player_status;

    public Action<float, float> OnUpdatedHP
    {
        get => m_player_status.OnUpdatedHP;
        set => m_player_status.OnUpdatedHP = value;
    }

    public Action<float, float> OnUpdateMP
    {
        get => m_player_status.OnUpdatedMP;
        set => m_player_status.OnUpdatedMP = value;
    }

    public StatusModel(PlayerStatus player_status)
    {
        m_player_status = player_status;
    }
}