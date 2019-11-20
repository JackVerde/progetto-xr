using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Light))]
public class LightEstimation : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The ARCameraManager which will produce frame events containing light estimation information.")]
    private ARCameraManager arCameraManager;

    [SerializeField] private ReflectionProbe reflectionProbe;

    public float lightIntensityMultiplier;
    public float probeIntensityMultiplier;
    public float globalIntensityMultiplier;

    public float min, max;

    public ARCameraManager CameraManager
    {
        get => arCameraManager;
        set
        {
            if (arCameraManager == value)
                return;

            if (arCameraManager != null)
                arCameraManager.frameReceived -= FrameChanged;

            arCameraManager = value;

            if (arCameraManager != null & enabled)
                arCameraManager.frameReceived += FrameChanged;
        }
    }

    private float? Brightness { get; set; }

    private float? ColorTemperature { get; set; }

    private Color? ColorCorrection { get; set; }

    private void Awake()
    {
        _mLight = GetComponent<Light>();
    }

    private void OnEnable()
    {
        if (arCameraManager != null)
            arCameraManager.frameReceived += FrameChanged;
    }

    private void OnDisable()
    {
        if (arCameraManager != null)
            arCameraManager.frameReceived -= FrameChanged;
    }

    private void FrameChanged(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            Brightness = args.lightEstimation.averageBrightness.Value;
            var intensityValue = Brightness.Value;
            _mLight.intensity = Mathf.Clamp(intensityValue * lightIntensityMultiplier, min, max);
            reflectionProbe.intensity = intensityValue * probeIntensityMultiplier; 
            RenderSettings.ambientIntensity = intensityValue * globalIntensityMultiplier;
        }

        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            ColorTemperature = args.lightEstimation.averageColorTemperature.Value;
            _mLight.colorTemperature = ColorTemperature.Value;
        }

        if (!args.lightEstimation.colorCorrection.HasValue) return;
        ColorCorrection = args.lightEstimation.colorCorrection.Value;
        _mLight.color = ColorCorrection.Value;
    }
    
    private Light _mLight;
}