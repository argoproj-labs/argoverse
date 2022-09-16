using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Atmosphere : MonoBehaviour
{
    public Material atmosphereMat;
    public Transform _player;
    public Transform _sun;
    [Header("Cloud setup, remove clouds material if not used to save processing")]
    public Material cloudsMat;
    public Color color1Initial;
    public Color color1Final;
    public Color color2Initial, color2Final;
    [SerializeField] private float dayThickness = 0.308f;
    [SerializeField] private float thicknessDelta = 0.073f;
    [SerializeField] private float dayCoreDarkness = -0.26f;
    [SerializeField] private float coreDarknessDelta = 0.26f;
    [SerializeField] private float dayFalloff = 0.11f;
    [SerializeField] private float falloffDelta = 9.89f;
    [SerializeField] private float dayAOstr = 0f;
    [SerializeField] private float AODelta = 0.5f;
    [SerializeField] private float dayFuzzy = 50f, fuzzyDelta = -48f;
    [SerializeField] private float dayEdgeBrigtness = 50f, edgeBrightnessDelta = -47.87f;
    [SerializeField] private Vector3 dayBounds = new Vector3(1f, 5f, 1f), boundsDelta = new Vector3(-0.97f, 0.44f, -1f);
    [SerializeField] private float dayNoiseTile = 0.03f, noiseTileDelta = 0.07f;
    [SerializeField] private float dayNoiseSpeed = 0.02f, noiseSpeedDelta = 0.08f;


    private void FixedUpdate()
    {
        float x = (1f - Vector3.Dot(-transform.up, (_player.position - transform.position).normalized)) / 2f; // 1 midnight 0 day? I think.
        float y = EasingTool.InOutSine(x);
        x = EasingTool.InOutQuad(x);
        atmosphereMat.SetTextureOffset("_BaseMap", new Vector2(0, x / 2));

        if (cloudsMat != null)
        {
            cloudsMat.SetFloat("_NoisePower", dayThickness + thicknessDelta * y);
            cloudsMat.SetFloat("_CoreDarkness", dayCoreDarkness + y * coreDarknessDelta);
            cloudsMat.SetFloat("_ColorFalloff", dayFalloff + y * falloffDelta);
            cloudsMat.SetColor("_Color", Color.Lerp(color1Initial, color1Final, y));
            cloudsMat.SetColor("_SecColor", Color.Lerp(color2Initial, color2Final, y));
            cloudsMat.SetFloat("_AO_Strength", dayAOstr + y * AODelta);
            cloudsMat.SetFloat("_FuzzyPower", dayFuzzy + fuzzyDelta * y);
            cloudsMat.SetFloat("_EdgeBrightness", dayEdgeBrigtness + edgeBrightnessDelta * y);
            cloudsMat.SetVector("Vector3_DF4CF5A5", dayBounds + boundsDelta * y);
            cloudsMat.SetFloat("Vector1_D652EA7E", dayNoiseTile + noiseTileDelta * y);
            cloudsMat.SetFloat("Vector1_A8FABF8F", dayNoiseSpeed + noiseSpeedDelta * y);
        }
    }
}
