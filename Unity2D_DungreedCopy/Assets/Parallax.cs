using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform       cam;            // MainCamera
    private Vector3         camStartPos;
    private float           distance;       // camStartPos와 현재위치간의 거리

    private GameObject[]    backgrounds;                                        
    private Material[]      mat;                                        
    private float[]         backSpeed;

    private float           farthestBack;

    [Range(0.01f,0.1f)]
    [SerializeField]
    private float           parallaxSpeed;

    private void Awake()
    {
        cam             = Camera.main.transform;
        camStartPos     = cam.position;

        int backCount = transform.childCount;

        mat             = new Material[backCount];
        backgrounds     = new GameObject[backCount];
        backSpeed       = new float[backCount];

        for (int i = 0; i < backCount; ++i)
        {
            backgrounds[i]  = transform.GetChild(i).gameObject;
            mat[i]          = backgrounds[i].GetComponent<Renderer>().material;
        }

        BackSpeedCalculate(backCount);
    }

    private void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++) // 가장 멀리있는 백그라운드 찾기
        {
            if((backgrounds[i].transform.position.z - cam.position.z) > farthestBack)
            {
                farthestBack = backgrounds[i].transform.position.z - cam.position.z;
            }
        }

        for (int i = 0; i < backCount; i++) // background의 스피드 설정
        {
            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
        }
    }

    private void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x, transform.position.y, 0);

        for (int i = 0; i < backgrounds.Length; ++i)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            mat[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }


}

