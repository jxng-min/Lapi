using UnityEngine;

[System.Serializable]
public class CursorData
{
    [Header("커서 모드")]
    public CursorMode Mode;

    [Header("커서 텍스처")]
    public Texture2D Cursor;

    [Header("커서 핫스팟")]
    public Vector2 Hotspot;
}