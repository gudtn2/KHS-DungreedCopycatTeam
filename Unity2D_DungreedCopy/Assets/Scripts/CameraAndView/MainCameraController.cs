using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    static public MainCameraController instance;

    [SerializeField]
    private Transform       player;
    [SerializeField]
    private float           smooting = 0.2f;

    public BoxCollider2D    bound;

    // YS: 박스 콜라이더 영역의 최소/ 최대 x,y,z값을 지닐 변수
    private Vector3         minBound;
    private Vector3         maxBound;

    // YS: 카페라의 반너비, 반높이 값을 지닐 변수
    private float           halfWidth;
    private float           halfHeight;

    // YS: 카메라의 반높이 값의 속성을 이용하기 위한 변수
    private Camera          halfHeightCam;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        halfHeightCam = GetComponent<Camera>();
        
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;

        // YS: 반너비 구하는 공식 = 반높이 * Screen.width / Screen.height(Screen.식은 해상도를 나타냄)
        halfHeight = halfHeightCam.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }
    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smooting);

        float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        this.transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }
}
