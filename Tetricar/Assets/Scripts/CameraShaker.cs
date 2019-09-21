using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Should be used with Main camera only
public class CameraShaker : MonoBehaviour
{
    [SerializeField]
    private Vector3 translationVector;
    private Vector3 angularVector;
    private Transform parentTransform;
    private float perlinSeed;
    private float trauma;
    public float shakeStrength;
    public float shakeTimer;
    public float recoverySpeed;
    private volatile bool shaking;
    
    

    void Awake() {
        perlinSeed = Random.value;
        trauma = 1f;
        translationVector = Vector3.one * 0.5f;
        angularVector = Vector3.one * 2f;
        parentTransform = this.transform.parent;
        shaking = false;
    }

    void Start() {
        Reset();
    }

    IEnumerator ShakeCamera() {
        this.shaking = true;
        float _x, _y, _z, __x, __y, __z;
        _x = this.transform.position.x;
        _y = this.transform.position.y;
        _z = this.transform.position.z;
        __x = this.transform.eulerAngles.x;
        __y = this.transform.eulerAngles.y;
        __z = this.transform.eulerAngles.z;
        for (int i = 0; i < 10; ++i) {
            float x, y, z;
            x = translationVector.x * (Mathf.PerlinNoise(perlinSeed, Time.time * shakeStrength) * 2 - 1) * trauma;
            y = translationVector.y * (Mathf.PerlinNoise(perlinSeed + Random.Range(0f, 1f), Time.time * shakeStrength) * 2 - 1) * trauma;
            z = translationVector.z * (Mathf.PerlinNoise(perlinSeed + Random.Range(1f, 2f), Time.time * shakeStrength) * 2 - 1) * trauma;
            float rotX, rotY, rotZ;
            rotX = angularVector.x * (Mathf.PerlinNoise(perlinSeed + Random.Range(2f,3f), Time.time * shakeStrength) * 2 - 1) * trauma;
            rotY = angularVector.y * (Mathf.PerlinNoise(perlinSeed + Random.Range(3f, 4f), Time.time * shakeStrength) * 2 - 1) * trauma;
            rotZ = angularVector.z * (Mathf.PerlinNoise(perlinSeed + Random.Range(4f, 5f), Time.time * shakeStrength) * 2 - 1) * trauma;
            
            transform.position = new Vector3(x + _x, y + _y, z + _z);
            transform.rotation = Quaternion.Euler(rotX + __x, rotY + __y, rotZ + __z);
            trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.02f);
        }
        Reset();
    }

    

    public void Reset() {
        this.shaking = false;
        this.transform.position = this.parentTransform.position;
        this.transform.rotation = this.parentTransform.rotation;
    }

    public void StartShaking() {
        if (this.shaking) return;
        this.trauma = 1f;
        StartCoroutine("ShakeCamera");
    }

    float ReverseClamp(float value, float min, float max) {
        if (value <= min) return max;
        if (value >= max) return min;
        return value;
    }
    
}
