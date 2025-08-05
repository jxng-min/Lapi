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
        public Vector3 Camera;
        public float PlayTime;
        public StatusData Status;

        public UserData()
        {
            Position = new Vector3(23f, -27f, 0f);
            Camera = new Vector3(5.5f, -19.5f, -10f);
            PlayTime = 0f;
            Status = new StatusData();
        }

        public UserData(Vector3 position, Vector3 camera, float playtime, StatusData status)
        {
            Position = position;
            Camera = camera;
            PlayTime = playtime;
            Status = status;
        }
    }
}