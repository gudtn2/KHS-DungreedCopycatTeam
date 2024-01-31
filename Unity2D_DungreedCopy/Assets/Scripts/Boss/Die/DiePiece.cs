using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePiece : MonoBehaviour
{
    private float       originTimeScale = 1.0f;

    [SerializeField]
    private GameObject  fairyXLPrefab;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ¶¥¿¡ ´êÀº °æ¿ì
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 9)
        {
            Time.timeScale = originTimeScale;
            playerController.isBossDie = false;
            StartCoroutine("CreateFairyXL");
        }
    }

    private IEnumerator CreateFairyXL()
    {
        yield return new WaitForSeconds(2f);

        GameObject temp = Instantiate(fairyXLPrefab);
        temp.transform.position = new Vector2(transform.position.x, transform.position.y + 3);
        temp.transform.rotation = temp.transform.rotation;
    }
}
