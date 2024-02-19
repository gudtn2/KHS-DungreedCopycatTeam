using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalkManager : MonoBehaviour
{
    private static TalkManager instance;
    public static TalkManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TalkManager>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject(typeof(TalkManager).Name);
                    instance = singleton.AddComponent<TalkManager>();
                }
            }
            return instance;
        }
    }
    public Dictionary<int, string[]>    talkData;
    public string                       Name;

    private void Start()
    {
        talkData = new Dictionary<int, string[]>();
    }

    public void AddTalkData(int id, string[] sentences)
    {
        talkData.Add(id, sentences);
    }

    public string GetTalk(int id,int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

}
