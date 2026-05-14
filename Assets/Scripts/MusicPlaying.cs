using System.Collections;
using UnityEngine;

public class MusicPlaying : MonoBehaviour {

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioClip music1;
    public AudioClip music2;

    [Header("Music Volume")]
    [Range(0f, 1f)] public float music1Volume = 1f;
    [Range(0f, 1f)] public float music2Volume = 0.6f;
    

    [Header("Timing")]
    public float music1Duration = 180f;
    public float music2Duration = 120f;

    public Coroutine musicRoutine;

    public void StartMusic() {
        if(musicRoutine != null) StopCoroutine(musicRoutine);
        musicRoutine = StartCoroutine(MusicRoutine() );
    }

    private void StopMusic() {
        if(musicRoutine != null) {
            StopCoroutine(musicRoutine);
            musicRoutine = null;
        }
        if(musicSource != null) musicSource.Stop();
    }

    private IEnumerator MusicRoutine() {
        if(musicSource == null) yield break;
        if(music1 != null) {
            musicSource.clip = music1;
            musicSource.volume = music1Volume;
            musicSource.loop = true;
            musicSource.Play();

            yield return new WaitForSeconds(music1Duration);
        }
        if(music2 != null) {
            musicSource.clip = music2;
            musicSource.volume = music2Volume;
            musicSource.loop = true;
            musicSource.Play();

            yield return new WaitForSeconds(music2Duration);
        }

        musicSource.Stop();
        musicRoutine = null;
    }
}
