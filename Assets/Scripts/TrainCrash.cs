using UnityEngine;
using System.Collections;

public class TrainCrash : MonoBehaviour {
    
    [Header("References")]
    public VRScreenUI ui;
    //[SerializeField] private CanvasGroup blackScreen;
    
    [Header("Audio")]
    [SerializeField] private AudioSource crashSound;
    //[SerializeField] private GameObject failMenu;

    [Header("Timing")]
    public float delayBeforeFade = 1f;
    public float fadeDura = 1.5f;

    [Header("Ending text")]
    public string failText = "The train barrels past the final warning,\n\nMetal screams.\n\nEverything goes black.";

    private bool hasCrashed;

    public void StartCrash(){
        if(hasCrashed) return;
        hasCrashed = true;
        StartCoroutine( CrashRoutine() );
    }

    private IEnumerator CrashRoutine(){
        if (crashSound != null) crashSound.Play();

        yield return new WaitForSeconds(delayBeforeFade);

        yield return StartCoroutine(ui.FadeBlack(1f, fadeDura) );

        ui.SetIntroText(failText);
    }
}
