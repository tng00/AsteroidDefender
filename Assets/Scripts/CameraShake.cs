using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineCamera _vcam;
    private CinemachineBasicMultiChannelPerlin _noise;
    private float _timer;

    void Awake()
    {
        Instance = this;
        _vcam = GetComponent<CinemachineCamera>();
        _noise = _vcam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float intensity, float duration)
    {
        if (_noise != null) _noise.AmplitudeGain = intensity;
        _timer = duration;
    }

    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0 && _noise != null) _noise.AmplitudeGain = 0f;
        }
    }
}
