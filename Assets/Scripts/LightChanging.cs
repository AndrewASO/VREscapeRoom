using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightChanging : MonoBehaviour {
    [Header("Lamp parent objects")]
    public GameObject[] lampParents;

    [Header("Normal Light Settings")]
    public Color normaLightColor = Color.white;
    public float normalLightIntensity = 0.4f;


    [Header("Alarm Light Settings")]
    public Color alarmColor = Color.red;
    public float alarmLightIntensity = 1.0f;    //Testing 1.0f first may reduce it to 0.7

    [Header("Emission Settings")]
    public Color normalEmissionColor = Color.white;
    public float normalEmissionStr = 2f;

    public Color alarmEmissionColor = Color.red;
    public float alarmEmissionStr = 3f;

    [Header("Alarm Timing")]
    public float alarmDura = 4f;
    public float flickerInterval = 0.15f;

    [Header("Emission Material Detection")]
    public string emissionMaterialNameContains = "emissive";
    private readonly List<Light> allLights = new List<Light>();
    private readonly List<EmissionSlot> emissionSlots = new List<EmissionSlot>();
    private MaterialPropertyBlock propertyBlock;

    private Coroutine shortAlarmCoroutine;
    private Coroutine finalAlarmCoroutine;

    private bool finalAlarmActive;

    private class EmissionSlot {
        public Renderer renderer;
        public int materialIndex;
        public string colorPropertyName;
    }

    private void Awake() {
        propertyBlock = new MaterialPropertyBlock();

        FindAllLampLights();
        FindAllEmissionMaterials();
    }

    private void Start() {
        SetNormalMode();
    }

    private void FindAllLampLights() {
        allLights.Clear();

        foreach(GameObject lampParent in lampParents) {
            if(lampParent == null) continue;

            Light[] lights = lampParent.GetComponentsInChildren<Light>(true);

            foreach(Light light in lights) {
                allLights.Add(light);
            }
        }
        Debug.Log($"RoomLight found {allLights.Count} child lights");
    }

    private void FindAllEmissionMaterials() {
        emissionSlots.Clear();

        foreach(GameObject lampParent in lampParents) {
            if(lampParent == null) continue;

            Renderer[] renderers = lampParent.GetComponentsInChildren<Renderer>(true);

            foreach(Renderer renderer in renderers) {
                Material[] materials = renderer.sharedMaterials;

                for(int i = 0; i < materials.Length; i++) {
                    Material material = materials[i];

                    if(material == null) continue;

                    bool nameMatches = material.name.ToLower().Contains(emissionMaterialNameContains.ToLower() );
                    //bool hasEmissionColor = material.HasProperty("_EmissionColor");

                    if(!nameMatches) continue;

                    string colorProperty = GetBestColorProperty(material);

                    
                    //if(nameMatches && hasEmissionColor) {
                    //    emissionSlots.Add(new EmissionSlot {
                    //        renderer = renderer,
                    //        materialIndex = i
                    //    });
                    //    break;
                    //}
                    

                    if (string.IsNullOrEmpty(colorProperty)) {
                        Debug.LogWarning($"Material '{material.name}' was found, but it has no usable color property.");
                        continue;
                    }

                    emissionSlots.Add(new EmissionSlot {
                        renderer = renderer,
                        materialIndex = i,
                        colorPropertyName = colorProperty,
                    });
                    break;
                }
            }
        }
        Debug.Log($"RoomLight found {emissionSlots.Count} emissive material slots");
    }

    private string GetBestColorProperty(Material material) {
        if(material.HasProperty("_EmissionColor") ) return "_EmissionColor";
        if(material.HasProperty("_BaseColor") ) return "_BaseColor";
        if(material.HasProperty("_Color") ) return "_Color";
        return null;
    }

    public void TriggerShortAlarmFlash() {
        if(finalAlarmActive) return;

        if(shortAlarmCoroutine != null) {
            StopCoroutine(shortAlarmCoroutine);
        }

        shortAlarmCoroutine = StartCoroutine(ShortAlarmRoutine() );
    }

    public void BeginFinalAlarmLoop() {
        finalAlarmActive = true;

        if(shortAlarmCoroutine != null) {
            StopCoroutine(shortAlarmCoroutine);
            shortAlarmCoroutine = null;
        }

        if(finalAlarmCoroutine != null) {
            StopCoroutine(finalAlarmCoroutine);
        }

        finalAlarmCoroutine = StartCoroutine(FinalAlarmRoutine() );
    }

    public void StopFinalAlarmLoop() {
       finalAlarmActive = false;

       if(finalAlarmCoroutine != null) {
            StopCoroutine(finalAlarmCoroutine);
            finalAlarmCoroutine = null;
        } 

        SetNormalMode();
    }

    private IEnumerator ShortAlarmRoutine() {
        float timer = 0f;

        while(timer < alarmDura) {
            bool flashOn = Mathf.FloorToInt(timer / flickerInterval) % 2 == 0;

            if (flashOn) {
                SetAllLights(alarmColor, alarmLightIntensity, true);
                SetAllEmission(alarmEmissionColor, alarmEmissionStr);
            }
            else {
                SetAllLights(Color.black, 0f, false);
                SetAllEmission(Color.black, 0f);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        SetNormalMode();
        shortAlarmCoroutine = null;
    }

    private IEnumerator FinalAlarmRoutine() {
        while (true) {
            SetAllLights(alarmColor, alarmLightIntensity, true);
            SetAllEmission(alarmEmissionColor, alarmEmissionStr);

            yield return new WaitForSeconds(flickerInterval);

            SetAllLights(Color.black, 0f, false);
            SetAllEmission(Color.black, 0f);

            yield return new WaitForSeconds(flickerInterval);
        }
    }

    public void SetNormalMode() {
        if(finalAlarmActive) return;

        SetAllLights(normaLightColor, normalLightIntensity, true);
        SetAllEmission(normalEmissionColor, normalEmissionStr);
    }

    private void SetAllLights(Color color, float intensity, bool enabled) {
        foreach(Light light in allLights) {
            if(light == null) continue;

            light.enabled = enabled;
            light.color = color;
            light.intensity = intensity;
        }
    }

    private void SetAllEmission(Color color, float strength) {
        foreach(EmissionSlot slot in emissionSlots) {
            if(slot.renderer == null) continue;

            slot.renderer.GetPropertyBlock(propertyBlock, slot.materialIndex);
            Color finalColor = color * strength;
            //propertyBlock.SetColor("_EmissionColor", color * strength);
            propertyBlock.SetColor(slot.colorPropertyName, finalColor);
            slot.renderer.SetPropertyBlock(propertyBlock, slot.materialIndex);
        }
    }


}
