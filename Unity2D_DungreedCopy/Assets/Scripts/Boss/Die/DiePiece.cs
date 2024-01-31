using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePiece : MonoBehaviour
{
    private float   originTimeScale = 1.0f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ¶¥¿¡ ´êÀº °æ¿ì
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9)
        {
            Time.timeScale = originTimeScale;
        }
    }
}
