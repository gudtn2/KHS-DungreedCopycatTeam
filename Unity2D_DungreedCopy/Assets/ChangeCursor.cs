using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 승석아 이 시발려나
public class ChangeCursor : MonoBehaviour
{
    [SerializeField]
    private Texture2D       cursorImg;

    private void Start()
    {
        Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.Auto);
    }
}
