public interface IState<T>
{
    void ExecuteEnter(T sender);
    void Execute();
    void ExecuteExit();
}