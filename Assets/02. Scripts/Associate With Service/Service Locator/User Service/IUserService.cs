using System;
using UnityEngine;

namespace UserService
{
    public interface IUserService
    {
        Vector3 Position { get; set; }
        StatusData Status { get; set; }

        event Action<int, int> OnUpdatedLevel;

        void UpdateLevel(int exp);
    }
}