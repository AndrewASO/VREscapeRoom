using UnityEngine;
using System.Collections;

public class TrainStopped : MonoBehaviour {
    
    [Header("References")]
    public VRScreenUI ui;
    public MapTimer mapTimer;

    [Header("Audio")]
    public AudioSource alarm;
    public AudioSource trainRumble;
    public AudioSource brakeSound;

    [Header("Timing")]
    public float brakeDura = 4f;
    public float fadeDura = 1.5f;

    private bool brakesHandled;


    public void HandleBrakes(){
        if(brakesHandled) return;

        brakesHandled = true;
        StartCoroutine( BrakeRoutine() );
    }

    IEnumerator BrakeRoutine(){
        mapTimer.StopTimer();

        if(alarm != null) alarm.Stop();
        if(brakeSound != null) brakeSound.Play();

        float t = 0f;

        float startPitch = trainRumble != null ? trainRumble.pitch : 1f;
        float startVolume = trainRumble != null ? trainRumble.volume : 1f;

        while(t < brakeDura){
            t += Time.deltaTime;
            float p = t / brakeDura;

            if(trainRumble != null){
                trainRumble.pitch = Mathf.Lerp(startPitch, 0.5f, p);
                trainRumble.volume = Mathf.Lerp(startVolume, 0.5f, p);
            }
            yield return null;
        }
        if(trainRumble != null) trainRumble.Stop();
        //if(brakeSuccess != null) brakeSuccess.Play();

        yield return new WaitForSeconds(1f);

        if(ui != null){
            yield return StartCoroutine(ui.FadeBlack(1f, fadeDura) );
            ui.ShowSuccessMenu();
        }
    }
}
