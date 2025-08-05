using System.Numerics;

public interface INameTagView
{
    void OpenUI(string name, Vector2 position);
    void CloseUI();
}