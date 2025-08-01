public interface IMovement
{
    float SPD { get; }
    bool Controll { get; set; }
    void Move();
    bool IsMove();
}