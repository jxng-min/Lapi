using System;
using System.IO;
using UnityEngine;
using UserService;

public class LocalUserService : ISaveable, IUserService
{
    private Vector3 m_position;
    private StatusData m_status;

    public event Action<int, int> OnUpdatedLevel;

    public Vector3 Position
    {
        get => m_position;
        set => m_position = value;
    }
    public StatusData Status
    {
        get => m_status;
        set => m_status = value;
    }

    public LocalUserService()
    {
        var user_data = new UserData();

        m_position = user_data.Position;
        m_status = user_data.Status;

        CreateDirectory();
    }

    private void CreateDirectory()
    {
        var local_directory = Path.Combine(Application.persistentDataPath, "User");

        if (!Directory.Exists(local_directory))
        {
            Directory.CreateDirectory(local_directory);
        }
    }

    public void UpdateLevel(int exp)
    {
        m_status.EXP += exp;

        OnUpdatedLevel?.Invoke(m_status.Level, m_status.EXP);
    }

    public bool Load(int offset)
    {
        var local_data_path = Path.Combine(Application.persistentDataPath, "User", $"UserData{offset}.json");

        if (File.Exists(local_data_path))
        {
            var json_data = File.ReadAllText(local_data_path);
            var user_data = JsonUtility.FromJson<UserData>(json_data);

            m_position = user_data.Position;
            m_status = user_data.Status;

            return true;
        }

        return false;
    }

    public void Save(int offset)
    {
        var local_data_path = Path.Combine(Application.persistentDataPath, "User", $"UserData{offset}.json");

        var user_data = new UserData(m_position, m_status);
        var json_data = JsonUtility.ToJson(user_data, true);

        File.WriteAllText(local_data_path, json_data);
    }
}
