using UnityEngine;

public static class CameraCache
{
	private static Camera mainCamera;
	public static Camera Main
	{
		get
		{
			if (mainCamera == null)
				SetMain(Camera.main);

			return mainCamera;
		}
	}

	private static Camera minimapCamera;
	public static Camera Minimap => minimapCamera;

	public static void SetMain(Camera camera)
	{
		if (camera != null)
			mainCamera = camera;
	}

	public static void SetMinimap(Camera camera)
	{
		if (camera != null)
			minimapCamera = camera;
	}

}
