using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDungeonData : MonoBehaviour
{
    static public PlayerDungeonData instance;
    
    public int   countKill = 0;

    public float enterTime; // ���� ���۽ð� ���
    public float deathTime; // �÷��̾� ���� �ð� ���
    public float totalTime; // �÷��̾ ��Ƴ��� �ð�

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void TimeChangeToText(TextMeshProUGUI textUI)
    {
        textUI.text = FormatTime(totalTime);
    }

    private string FormatTime(float seconds)
    {
        int h = (int)(seconds / 3600);
        int m = (int)(seconds % 3600) / 60;
        int s = (int)(seconds % 60);

        return string.Format("{0:D2}h {1:D2}m {2:D2}s", h, m, s);
    }
}
