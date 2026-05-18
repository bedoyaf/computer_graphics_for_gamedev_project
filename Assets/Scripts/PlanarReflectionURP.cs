using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class PlanarReflectionsURP : MonoBehaviour
{
    public LayerMask reflectionMask;
    public RenderTexture targetFolderTexture; // Tvoje ručně vytvořená textura z projektu!

    private Camera _reflectionCamera;
    private static bool _isRendering;

    private void OnEnable() => RenderPipelineManager.beginCameraRendering += ExecutePlanarReflections;
    private void OnDisable() => RenderPipelineManager.beginCameraRendering -= ExecutePlanarReflections;

    private void ExecutePlanarReflections(ScriptableRenderContext context, Camera camera)
    {
        if (camera.cameraType == CameraType.Preview || camera.cameraType == CameraType.SceneView && !Application.isPlaying) return;
        if (_isRendering || targetFolderTexture == null) return;
        _isRendering = true;

        CreateResources();
        UpdateCameraMatrices(camera);

        // Natvrdo v Unity 6 řekneme: Vykresli tuhle kameru přes StandardRequest
        RenderPipeline.StandardRequest request = new RenderPipeline.StandardRequest();
        RenderPipeline.SubmitRenderRequest(_reflectionCamera, request);

        _isRendering = false;
    }

    private void CreateResources()
    {
        if (_reflectionCamera == null)
        {
            GameObject go = new GameObject("Moje Planar Cam", typeof(Camera));
            _reflectionCamera = go.GetComponent<Camera>();
            _reflectionCamera.enabled = false; // Nechceme, aby běžela sama, budeme ji spouštět ručně
            _reflectionCamera.cullingMask = reflectionMask;
            _reflectionCamera.targetTexture = targetFolderTexture; // Nastavení tvé textury!
            go.hideFlags = HideFlags.DontSave;
        }
    }

    private void UpdateCameraMatrices(Camera realCamera)
    {
        _reflectionCamera.CopyFrom(realCamera);
        _reflectionCamera.targetTexture = targetFolderTexture;

        // Matematika zrcadlení pozice podle roviny vody
        Vector3 pos = transform.position;
        Vector3 normal = transform.up;
        float d = -Vector3.Dot(normal, pos);
        Vector4 reflectionPlane = new Vector4(normal.x, normal.y, normal.z, d);

        Matrix4x4 reflectionMatrix = Matrix4x4.zero;
        CalculateReflectionMatrix(ref reflectionMatrix, reflectionPlane);

        _reflectionCamera.worldToCameraMatrix = realCamera.worldToCameraMatrix * reflectionMatrix;
        _reflectionCamera.transform.position = reflectionMatrix.MultiplyPoint(realCamera.transform.position);
    }

    private void CalculateReflectionMatrix(ref Matrix4x4 reflectionMatrix, Vector4 plane)
    {
        reflectionMatrix.m00 = (1F - 2F * plane[0] * plane[0]); reflectionMatrix.m01 = (-2F * plane[0] * plane[1]); reflectionMatrix.m02 = (-2F * plane[0] * plane[2]); reflectionMatrix.m03 = (-2F * plane[3] * plane[0]);
        reflectionMatrix.m10 = (-2F * plane[1] * plane[0]); reflectionMatrix.m11 = (1F - 2F * plane[1] * plane[1]); reflectionMatrix.m12 = (-2F * plane[1] * plane[2]); reflectionMatrix.m13 = (-2F * plane[3] * plane[1]);
        reflectionMatrix.m20 = (-2F * plane[2] * plane[0]); reflectionMatrix.m21 = (-2F * plane[2] * plane[1]); reflectionMatrix.m22 = (1F - 2F * plane[2] * plane[2]); reflectionMatrix.m23 = (-2F * plane[3] * plane[2]);
        reflectionMatrix.m30 = 0F; reflectionMatrix.m31 = 0F; reflectionMatrix.m32 = 0F; reflectionMatrix.m33 = 1F;
    }
}