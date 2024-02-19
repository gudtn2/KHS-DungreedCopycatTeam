using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypingEffect : MonoBehaviour
{
    public int          charPerSec;
    public GameObject   endCursor;
    string              targetMsg;
    TextMeshProUGUI     msgText;
    int                 index;
    float               interval;

    private void Awake()
    {
        msgText = GetComponent<TextMeshProUGUI>();
    }

    public void SetMSG(string msg)
    {
        targetMsg = msg;
        EffectStart();
    }

    private void EffectStart()
    {
        msgText.text = "";
        index = 0;
        endCursor.SetActive(false);
        interval = 1.0f / charPerSec;

        Invoke("Effecting", interval);
    }
    private void Effecting()
    {
        if (msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }

        msgText.text += targetMsg[index];
        index++;

        Invoke("Effecting", interval);
    }
    private void EffectEnd()
    {
        endCursor.SetActive(true);
    }
}
