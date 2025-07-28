using UnityEngine;

namespace UserService
{
    public interface IUserService
    {
        public Vector3 Position { get; set; }
        public StatusData Status { get; set; }

        public void Inject(int offset);
        public bool Load();
        public void Save();
    }
}