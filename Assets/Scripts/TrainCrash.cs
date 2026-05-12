using UnityEngine;
using System.Collections;

public class TrainCrash : MonoBehaviour {
    
    public VRScreenUI ui;
    //[SerializeField] private CanvasGroup blackScreen;
    [SerializeField] private AudioSource crashSound;
    //[SerializeField] private GameObject failMenu;

    public void StartCrash(){
        StartCoroutine( CrashRoutine() );
    }

    private IEnumerator CrashRoutine(){
        if (crashSound != null) crashSound.Play();

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(ui.FadeBlack(1f, 1.5f) );

        ui.ShowFailMenu();
    }
}
