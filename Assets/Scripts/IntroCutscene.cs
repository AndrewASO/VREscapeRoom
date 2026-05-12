using UnityEngine;
using System.Collections;
using TMPro;

public class IntroCutscene : MonoBehaviour {

    [Header("Train Intro Connections")]
    public MapTimer mapTimer;
    public VRScreenUI ui;
    //public TMP_Text introText;
    //public CanvasGroup blackScreen;

    [Header("Train Intro Audio")]
    public AudioSource trainRumble;
    public AudioSource alarm;
    public AudioSource metalScreech;

    [Header("Timing")]
    public float firstLinedelay = 2f;
    public float lineDelay = 3f;
    public float fadeOutTime = 1.5f;

    void Start(){
        StartCoroutine(IntroRoutine() );
    }

    IEnumerator IntroRoutine(){
        if(ui != null){
            ui.SetBlackInstant(1f);
            ui.SetIntroText("");
        }

        if(trainRumble != null) trainRumble.Play();

        yield return new WaitForSeconds(firstLinedelay);
        ui.SetIntroText("You wake to the sound of the train shaking beneath you.");

        //Alarm Starts
        yield return new WaitForSeconds(lineDelay);
        if(alarm != null) alarm.Play();
        ui.SetIntroText("An alarm blares through the car.");

        yield return new WaitForSeconds(lineDelay);
        ui.SetIntroText("You wake to the sound of the train shaking beneath you.");

        yield return new WaitForSeconds(lineDelay);
        if(metalScreech != null) metalScreech.Play();
        ui.SetIntroText("The train is out of control.");

        yield return new WaitForSeconds(lineDelay);
        ui.SetIntroText("Stop the train before it reaches the end of the line.");

        yield return new WaitForSeconds(lineDelay);

        if(ui != null) ui.HideIntroText();

        if(ui != null){
            yield return StartCoroutine(ui.FadeBlack(0f, fadeOutTime) );
        }

        if(metalScreech != null & metalScreech.loop){
            metalScreech.Stop();
        }

        if(mapTimer != null){
            mapTimer.StartTimer();
        }
    }

}
