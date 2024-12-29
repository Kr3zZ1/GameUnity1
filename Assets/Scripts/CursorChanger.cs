using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D customCursorTexture; // Текстура курсора
    public Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        EnableCursor();
    }

    public void EnableCursor()
    {
        Cursor.SetCursor(customCursorTexture, hotSpot, CursorMode.Auto);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisableCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
