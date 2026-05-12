using UnityEngine;
using System.Collections;
using TMPro;

public class VRScreenUI : MonoBehaviour{
    [Header("Black Screen")]
    public CanvasGroup blackPanel;

    [Header("Intro")]
    public TMP_Text introText;

    [Header("Menus")]
    public GameObject failMenu;
    public GameObject successMenu;

    public void Awake(){
        SetBlackInstant(1f);

        if(introText != null) introText.text = "";
        if(failMenu != null) failMenu.SetActive(false);
        if(successMenu != null) successMenu.SetActive(false);
    }

    public void SetBlackInstant(float alpha){
        if(blackPanel == null) return;

        blackPanel.alpha = alpha;
        blackPanel.interactable = false;
        blackPanel.blocksRaycasts = false;
    }

    public IEnumerator FadeBlack(float targetAlpha, float duration){
        if(blackPanel == null) yield break;

        float startAlpha = blackPanel.alpha;
        float t = 0f;

        while(t < duration){
            t += Time.deltaTime;
            blackPanel.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
            yield return null;
        }

        blackPanel.alpha = targetAlpha;
    }

    public void SetIntroText(string text){
        if(introText != null) introText.text = text;
    }

    public void HideIntroText(){
        if(introText != null) introText.text = "";
    }

    public void ShowFailMenu(){
        HideIntroText();
        if(failMenu != null) failMenu.SetActive(true);
        if(successMenu != null) successMenu.SetActive(false);
    }

    public void ShowSuccessMenu(){
        HideIntroText();
        if(failMenu != null) failMenu.SetActive(false);
        if(successMenu != null) successMenu.SetActive(true);
    }
}
