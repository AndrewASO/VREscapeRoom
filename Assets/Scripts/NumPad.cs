using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class NumPad : MonoBehaviour {

    //Responsible for the code that the user needs to input
    [Header("Code Settings")]
    [SerializeField] private string correctCode = "1234";
    private string currentInput = "";

    [Header("Button Renderers")]
    [SerializeField] private Renderer[] buttonRenderers;

    //The different materials for when the code is incorrect / correct
    [Header("Flash Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material correctMaterial;
    [SerializeField] private Material incorrectMaterial;

    //This is responsible for how long the buttons will flash with different materials
    [Header("Flash Timing")]
    [SerializeField] private float correctFlashDura = 1.5f;
    [SerializeField] private float incorrectFlashDura = 1.5f;
    //May make both of these the same duration so might just change it to duration
    //in general rather than having one specialized for correct / incorrect
    [SerializeField] private float flashOnTime = 0.25f;
    [SerializeField] private float flashOffTime = 0.15f;

    //The audio that will play depending on the cues
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip inputSound;          //https://pixabay.com/sound-effects/film-special-effects-single-keypad-beep-433456/
    [SerializeField] private AudioClip incorrectSound;      //https://pixabay.com/sound-effects/film-special-effects-wrong-47985/
    [SerializeField] private AudioClip correctSound;        //https://pixabay.com/sound-effects/technology-correct-472358/

    //Might not even need both of these, should only need correctCode to open doors / do an event
    //Can't think of a use for incorrectCode besides maybe give a hint or speed up time ? 
    //But then why should I even keep unlocked if that's the case ? Eh, will look into this later and 
    //decide what needs to be trimmed or I might just have some redundant parts leftover
    [Header("Events")]
    public UnityEvent onCorrectCode;
    public UnityEvent onIncorrectCode;

    private Material[][] originalMaterials;
    //Do I even need ischecking ? Would be useful for a coroutine as I thought I Might need it for, but I'm not 
    //doing that anymore and just doing a call so don't think its needed ?
    private bool isChecking = false;
    private bool unlocked = false;

    private void Awake() {
        if(buttonRenderers == null || buttonRenderers.Length == 0) {
            buttonRenderers = GetComponentsInChildren<Renderer>();
        }
    }

    public void PressDigit(string digit) {
        PlaySound(inputSound);
        if(isChecking || unlocked) {
            return;
        }

        currentInput += digit;

        if(currentInput.Length == correctCode.Length) {
            CheckCode();
        }
    }

    private void CheckCode() {
        isChecking = true;
        if(currentInput == correctCode) {
            unlocked = true;

            PlaySound(correctSound);
            onCorrectCode?.Invoke();     //Turns out this is going to be used for opening the doors

            StartCoroutine(FlashButtons(correctMaterial, correctFlashDura, false) );
        }
        else {
            PlaySound(incorrectSound);
            //onIncorrectCode?.Invoke();

            StartCoroutine(FlashButtons(incorrectMaterial, incorrectFlashDura, true) );
        }
    }

    private IEnumerator FlashButtons(Material flashMaterial, float totalDura, bool resetAfter) {
        float timer = 0f;

        while(timer < totalDura) {
            SetAllButtons(flashMaterial);
            yield return new WaitForSeconds(flashOnTime);
            timer += flashOnTime;

            SetAllButtons(defaultMaterial);
            yield return new WaitForSeconds(flashOffTime);
            timer += flashOffTime;
        }

        SetAllButtons(defaultMaterial);

        if(resetAfter) {
            currentInput = "";
            isChecking = false;
        }
    }

    private void SetAllButtons(Material material) {
        if(material == null) {
            return;
        }
        foreach(Renderer buttonRenderer in buttonRenderers) {
            if(buttonRenderer != null) {
                buttonRenderer.sharedMaterial = material;
            }
        }
    }

    private void PlaySound(AudioClip clip) {
        if(audioSource != null && clip != null) {
            audioSource.PlayOneShot(clip);
        }
    }

    public void ResetPad() {
        currentInput = "";
        isChecking = false;
        unlocked = false;
        SetAllButtons(defaultMaterial);
    }
}
