using UnityEngine;
using System.Collections;
using TMPro;

public class IntroCutscene : MonoBehaviour {

    [Header("Train Intro Connections")]
    public MapTimer mapTimer;
    public VRScreenUI ui;
    public MusicPlaying musicPlaying;
    //public TMP_Text introText;
    //public CanvasGroup blackScreen;

    [Header("Train Intro Audio")]
    public AudioSource trainRumble;
    public AudioSource alarm;
    public AudioSource metalScreech;

    [Header("Audio Volume")]
    [Range(0f, 1f)] public float rumbleVolume = 1f;
    [Range(0f, 1f)] public float alarmVolume = 1f;
    [Range(0f, 1f)] public float metalVolume = 1f;

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

        if(trainRumble != null) trainRumble.volume = rumbleVolume; trainRumble.Play();

        yield return new WaitForSeconds(firstLinedelay);
        ui.SetIntroText("You wake to the sound of the train shaking beneath you.");

        //Alarm Starts
        yield return new WaitForSeconds(lineDelay);
        if(alarm != null) alarm.volume = alarmVolume; alarm.Play();
        ui.SetIntroText("An alarm blares through the car.");

        yield return new WaitForSeconds(lineDelay);
        ui.SetIntroText("You wake to the sound of the train shaking beneath you.");

        yield return new WaitForSeconds(lineDelay);
        if(metalScreech != null) metalScreech.volume = metalVolume; metalScreech.Play();
        ui.SetIntroText("The train is out of control.");

        yield return new WaitForSeconds(lineDelay);
        ui.SetIntroText("Stop the train before it reaches the end of the line.");

        yield return new WaitForSeconds(lineDelay);
        ui.SetIntroText("In the first level you'll have to solve a puzzle using your chair seats");

        yield return new WaitForSeconds(lineDelay);
        ui.SetIntroText("In the 2nd level you'll need to solve a puzzle relating to the food menu");

        yield return new WaitForSeconds(lineDelay);

        if(ui != null) ui.HideIntroText();

        if(ui != null){
            yield return StartCoroutine(ui.FadeBlack(0f, fadeOutTime) );
        }

        if(metalScreech != null) metalScreech.Stop();

        if(mapTimer != null) mapTimer.StartTimer();
        
        if(musicPlaying != null) musicPlaying.StartMusic();
    }

}
