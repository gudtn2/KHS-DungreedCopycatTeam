using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    [SerializeField]
    private Texture2D       cursorImg;

    private void Start()
    {
        Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.Auto);
    }
}
