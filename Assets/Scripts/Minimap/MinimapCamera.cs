using UnityEngine;

/// <summary>
/// Drives a camera to follow as a birds eye view of the main camera.
/// Also signals MinimapObservables to orient and billboard.
/// </summary>
[RequireComponent(typeof(Camera))]
public class MinimapCamera : MonoBehaviour
{
	[SerializeField] float targetHeight = 6f;

	private void Awake()
	{
		CameraCache.SetMinimap(GetComponent<Camera>());
	}

	private void Update()
	{
		transform.position = CameraCache.Main.transform.position + (Vector3.up * targetHeight);
		transform.localRotation = Quaternion.Euler(90f, 0, -CameraCache.Main.transform.localRotation.eulerAngles.y);
	}

	private void LateUpdate()
	{
		RefreshObserved();
	}

	void RefreshObserved()
	{
		for (int i = 0; i < MinimapRegistry.Observables.Count; i++)
		{
			MinimapRegistry.Observables[i].Orthografy(transform);
		}
	}
}
