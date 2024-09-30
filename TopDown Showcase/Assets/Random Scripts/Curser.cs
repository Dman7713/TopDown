using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    private Vector2 cursorHotspot;

    // Start is called before the first frame update
    void Start()
    {
        // Correct way to create a Vector2
        cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        // Correct spelling of Cursor
        //Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }
}
