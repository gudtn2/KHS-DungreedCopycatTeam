using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    private BoxCollider2D           bound;

    private MainCameraController    mainCamController;
    private void Awake()
    {
        bound             = GetComponent<BoxCollider2D>();

        mainCamController = FindObjectOfType<MainCameraController>();

        mainCamController.SetBound(bound);
    }
}
