using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private Transform camTransform;

    private Vector3 camStartPos;
    private float   dis;
    
    private Material[]  materials;
    private float[]     layerSpeed;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float parallaxSpeed;

    private void Awake()
    {
        camStartPos = camTransform.position;

        int backgroundCount = transform.childCount;
        GameObject[] backgrounds = new GameObject[backgroundCount];

        materials = new Material[backgroundCount];
        layerSpeed = new float[backgroundCount];

        for (int i = 0; i < backgroundCount; ++i)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            materials[i] = backgrounds[i].GetComponent<Renderer>().material;
        }
    }
}

