using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBossIntroduce : MonoBehaviour
{
    [Header("보스 소개 UI 컨트롤")]
    [SerializeField]
    private TextMeshProUGUI TextBossNameUI;
    [SerializeField]
    private TextMeshProUGUI TextBossNicknameUI;
    [SerializeField]
    private string          stringBossName;
    [SerializeField]
    private string          stringBossNickname;
    [SerializeField]
    private Image           BossIntroduceImageTop;
    [SerializeField]
    private Image           BossIntroduceImageBottom;
    [SerializeField]
    GameObject              bossLifeObj;
    
    public bool             isAliveTheBoss = false;

    private PlayerController        player;
    private UIEffectManager         uiEffectManager;

    private void Awake()
    {
        player              = FindObjectOfType<PlayerController>();
        uiEffectManager     = FindObjectOfType<UIEffectManager>();
    }

    private void Start()
    {
        TextBossNameUI.text = stringBossName;
        TextBossNicknameUI.text = stringBossNickname;
    }
    public IEnumerator OnIntroduceBoss(float start,float end)
    {
        StartCoroutine(uiEffectManager.UIFade(BossIntroduceImageTop, start, end));
        StartCoroutine(uiEffectManager.UIFade(BossIntroduceImageBottom, start, end));

        yield return new WaitForSeconds(1f);
        StartCoroutine(uiEffectManager.UIFade(TextBossNameUI, start, end));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(uiEffectManager.UIFade(TextBossNicknameUI, start, end));
    }
    public IEnumerator OffIntroduceBoss(float start, float end)
    {
        isAliveTheBoss = true;
        bossLifeObj.SetActive(isAliveTheBoss);

        StartCoroutine(uiEffectManager.UIFade(BossIntroduceImageTop, start, end));
        StartCoroutine(uiEffectManager.UIFade(BossIntroduceImageBottom, start, end));

        StartCoroutine(uiEffectManager.UIFade(TextBossNameUI, start, end));
        StartCoroutine(uiEffectManager.UIFade(TextBossNicknameUI, start, end));
        yield return null;
    }
}
