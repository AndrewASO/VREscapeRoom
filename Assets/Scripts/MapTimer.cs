using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using TMPro;

public class MapTimer : MonoBehaviour {

    [Header("Map Timer")]
    public float mapDura = 300f;    //5 minutes
    public float finalAlarmStartsAt = 30f;  //Alarm starts in 30s (Keeps going without doing it only for a few seconds)

    [Header("Regular Alarm")]
    public float alarmEveryXSeconds = 30f;   //Alarm goes for a few second every 30s

    [Header("Timer Display")]
    public TMP_Text timerText;

    [Header("Events")]
    public UnityEvent onRegularAlarmPulse;
    public UnityEvent onFinalAlarmStarted;
    public UnityEvent onTimerEnded;

    private float elapsedTime;
    private float nextAlarmTime;
    private bool finalAlarmStarted;
    private bool timerRunning;

    public float TimeRemaining => Mathf.Max(0f, mapDura - elapsedTime);
    public float ElapsedTime => elapsedTime;

    private void Start() {
        //StartTimer();
        UpdateTimerText();
    }

    public void StartTimer() {
        StopAllCoroutines();

        elapsedTime = 0f;
        nextAlarmTime = alarmEveryXSeconds;
        finalAlarmStarted = false;
        timerRunning = true;

        UpdateTimerText();

        StartCoroutine(TimerRoutine()) ;
    }

    private IEnumerator TimerRoutine() {
        while (timerRunning) {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
            float timeRemaining = TimeRemaining;

            //If the final alarm bool hasn't been set to true and timeRemaining is finally less than when the final alarm begin
            if(!finalAlarmStarted && timeRemaining <= finalAlarmStartsAt) {
                finalAlarmStarted = true;
                onFinalAlarmStarted?.Invoke();
            }
            //Begin a regular alarm event
            if(!finalAlarmStarted && elapsedTime >= nextAlarmTime) {
                onRegularAlarmPulse?.Invoke();
                nextAlarmTime += alarmEveryXSeconds;
            }
            //Map ran out of time
            if(elapsedTime >= mapDura) {
                elapsedTime = mapDura;  //For getting time to display 0:00
                UpdateTimerText();
                timerRunning = false;
                onTimerEnded?.Invoke();
            }

            yield return null;
        }
    }

    private void UpdateTimerText() {
        if(timerText == null) return;

        float remaining = TimeRemaining;

        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);

        timerText.text = $"{minutes:0}:{seconds:00}";
    }

    public void StopTimer() {
        timerRunning = false;
    }

}
