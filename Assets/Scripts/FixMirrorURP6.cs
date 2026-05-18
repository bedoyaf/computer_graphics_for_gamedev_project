using UnityEngine;

[ExecuteInEditMode]
public class FixMirrorURP6 : MonoBehaviour
{
    [Header("Tvůj ručně vytvořený Render Texture z projektu")]
    public RenderTexture mojeVodniZrcadlo;

    private Camera _reflectionCamera;

    void OnEnable()
    {
        ApplyRenderTarget();
    }

    void Update()
    {
        if (_reflectionCamera != null && _reflectionCamera.targetTexture != mojeVodniZrcadlo)
        {
            ApplyRenderTarget();
        }
    }

    void ApplyRenderTarget()
    {
        _reflectionCamera = GetComponent<Camera>();

        if (_reflectionCamera != null && mojeVodniZrcadlo != null)
        {
            _reflectionCamera.targetTexture = mojeVodniZrcadlo;
            Debug.Log("URP 6 Fix: Textura byla úspěšně vnucena zrcadlové kameře!", this);
        }
    }
}