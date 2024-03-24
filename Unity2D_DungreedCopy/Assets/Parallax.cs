using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform   transformCam;
    private Vector3     camStartPos;
    private float       dis;

    private Material[]          mats;
    private GameObject[]        backgrounds;
    private float[]             backSpeed;

    private float farthestBack;
    [Range(0.01f,0.05f)]
    [SerializeField]
    private float parallaxSpeed;


    private void Awake()
    {
        transformCam    = Camera.main.transform;
        camStartPos     = transformCam.position;
    }
    private void Start()
    {
        int backCount = transform.childCount;
        mats = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; ++i)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mats[i] = backgrounds[i].GetComponent<Renderer>().material;
        }
        BackSpeedCalculate(backCount);
    }

    private void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; ++i)
        {
            if((backgrounds[i].transform.position.z - transformCam.position.z) > farthestBack)
            {
                farthestBack = backgrounds[i].transform.position.z - transformCam.position.z;
            }
        }

        for (int i = 0; i < backCount; ++i)
        {
            if ((backgrounds[i].transform.position.z - transformCam.position.z) > farthestBack)
            {
                backSpeed[i] =  1 - (backgrounds[i].transform.position.z - transformCam.position.z) / farthestBack;
            }
        }
    }

    private void LateUpdate()
    {
        dis = transformCam.position.x - camStartPos.x;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            mats[i].SetTextureOffset("_MainTex", new Vector2(dis, 0) * speed);
        }
    }
}
