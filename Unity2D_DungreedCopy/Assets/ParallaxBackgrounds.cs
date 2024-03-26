using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgrounds : MonoBehaviour
{
    [SerializeField]
    private Vector2 parallaxEffectMultiplier;

    private Transform   camTransform;
    private Vector3     lastCamPos;
    private float       textureUnitSizeX;
    private void Awake()
    {
        camTransform = Camera.main.transform;
        lastCamPos = camTransform.position;

        Sprite      sprite  = GetComponent<SpriteRenderer>().sprite;
        Texture2D   texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }
    private void Update()
    {
        Vector3 deltaMovement = camTransform.position - lastCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y) ;
        lastCamPos = camTransform.position;

        if(Mathf.Abs(camTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPosX = (camTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(camTransform.position.x + offsetPosX, transform.position.y);
        }
    }
}
