using System;
using UnityEngine;

namespace UserService
{
    public interface IUserService
    {
        Vector3 Position { get; set; }
        Vector3 Camera { get; set; }
        float PlayTime { get; set; }
        StatusData Status { get; set; }

        event Action<int, int> OnUpdatedLevel;

        void InitializeLevel();
        void UpdateLevel(int exp);
    }
}