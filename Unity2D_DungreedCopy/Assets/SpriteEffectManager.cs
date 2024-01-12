using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffectManager : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve  activeCurve;
    [SerializeField]
    private float           activeTime;

    public void StartSpriteSetting(SpriteRenderer newSprite)
    {
        Color color = newSprite.color;

        color.a = 0;

        newSprite.color = color;
    }

    public IEnumerator ActiveSprite(SpriteRenderer newSprite)
    {
        float curTime = 0;
        float percent = 0;

        while (percent < 1)
        {
            curTime += Time.deltaTime;
            percent = curTime / activeTime;

            Color color = newSprite.color;
            color.a = Mathf.Lerp(0, 1, activeCurve.Evaluate(percent));
            newSprite.color = color;

            yield return null;
        }
    }

    public IEnumerator RoutainActiveSprite( SpriteRenderer[] newSprites)
    {
        int i = 0;

        while(i < newSprites.Length)
        {
            float curTime = 0;
            float percent = 0;

            if(percent < 1)
            {
                curTime += Time.deltaTime;
                percent = curTime / activeTime;

                Color color = newSprites[i].color;
                color.a = Mathf.Lerp(0, 1, activeCurve.Evaluate(percent));
                newSprites[i].color = color;
            }
            else if(percent >= 1)
            {
                yield return new WaitForSeconds(activeTime);
                ++i;
            }
        }
    }
}
