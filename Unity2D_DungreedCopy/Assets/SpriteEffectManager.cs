using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffectManager : MonoBehaviour
{
    [SerializeField]
    private float           limitTime;
    [SerializeField]
    private AnimationCurve  activeCurve;

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
            percent = curTime / limitTime;

            Color color = newSprite.color;
            color.a = Mathf.Lerp(0, 1, activeCurve.Evaluate(percent));
            newSprite.color = color;

            yield return null;
        }
    }
}
