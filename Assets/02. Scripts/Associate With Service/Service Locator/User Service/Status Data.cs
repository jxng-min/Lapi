using UnityEngine;

namespace UserService
{
    [System.Serializable]
    public class StatusData
    {
        public int Level;
        public int EXP;
        public float HP;
        public float MP;

        public StatusData()
        {
            Level = 1;
            EXP = 0;
            HP = 500f;
            MP = 300f;
        }
    }

    [System.Serializable]
    public class UserData
    {
        public Vector3 Position;
        public StatusData Status;

        public UserData()
        {
            Position = new Vector3();
            Status = new StatusData();
        }

        public UserData(Vector3 position, StatusData status)
        {
            Position = position;
            Status = status;
        }
    }
}