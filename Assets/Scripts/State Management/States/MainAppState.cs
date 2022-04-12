using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MainAppState : AppState
{
    [SerializeField] GameObject treePrefab;
	[SerializeField] GameObject warningPrefab;
    [SerializeField] AppState resetState;
    ARSessionOrigin origin;
	Renderer warningRenderer;

    public override void OnEnterState()
    {
        if (origin == null)
            origin = FindObjectOfType<ARSessionOrigin>();

		if (warningRenderer == null)
		{
			warningRenderer = Instantiate(warningPrefab).GetComponent<Renderer>();
			warningRenderer.material.color = Color.clear;
		}
    }

    public override void Update()
    {
        var playerPosition = CameraCache.Main.transform.position;
        var originPosition = origin.transform.position;

        var sqrDist = Vector3.SqrMagnitude(playerPosition - originPosition);

        if (sqrDist >= WARNING_DIST * WARNING_DIST)
            DrawWarning();

        // if (sqrDist >= MAX_DIST * MAX_DIST)
        //     FadeCameraToBlack(sqrDist);
    }

    #region UI
    public void OnResetButtonClicked()
    {
        FindObjectOfType<ARSession>()?.Reset();
        AppStateManager.ChangeState(resetState);
    }

    public void OnAddTreeClicked()
    {
        var rayStart = CameraCache.Main.transform.position + (CameraCache.Main.transform.forward * 1.5f);
        var rayDirection = Vector3.down;
        Ray ray = new Ray(rayStart, rayDirection);

        var index = AnchorCreator.Instance.AnchorPoints.Count;
        AnchorCreator.Instance.CreateAnchor(ray, treePrefab, index.ToString());
    }
    #endregion

    #region Boundary

    const float WARNING_DIST = 5;
    const float MAX_DIST = 10;
    const float BLACKOUT_DIST = MAX_DIST + 0.3f;

    ARCameraBackground arCameraBackground;
    ARCameraBackground ARCameraBackground
    {
        get
        {
            if (arCameraBackground == null)
                arCameraBackground = CameraCache.Main.GetComponent<ARCameraBackground>();

            return arCameraBackground;
        }
    }

    void DrawWarning()
    {
		var playerVector = CameraCache.Main.transform.position - origin.transform.position;
		var rayLength = MAX_DIST - playerVector.magnitude;
		var combinedRay = playerVector + (CameraCache.Main.transform.forward.normalized * rayLength);
		var clampedRay = origin.transform.position + (combinedRay.normalized * MAX_DIST);
		warningRenderer.transform.position = origin.transform.position + clampedRay;
		
		var lookRotation = Quaternion.LookRotation(playerVector);
		warningRenderer.transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);

		var lerpFactor = Mathf.InverseLerp(0, MAX_DIST * (.5f * MAX_DIST), playerVector.sqrMagnitude);
		warningRenderer.material.color = Color.Lerp(Color.clear, Color.white, Mathf.Clamp01(lerpFactor));
    }

    void FadeCameraToBlack(float sqrDist)
    {
        var lerpFactor = Mathf.InverseLerp(MAX_DIST * MAX_DIST, BLACKOUT_DIST * BLACKOUT_DIST, sqrDist);
        lerpFactor = Mathf.Clamp01(lerpFactor);
        var color = ARCameraBackground.material.color;

        color = Color.Lerp(Color.white, Color.black, lerpFactor);
        ARCameraBackground.material.color = color;
    }
    #endregion
}
