using UnityEngine;

// TODO This script suggests we could do without a MonoBehaviour
// and instead just extend state-specific managed Update calls

public class MinimapRotator : MonoBehaviour
{
	[SerializeField] RectTransform targetTransform;

	private void Update()
	{
		targetTransform.rotation = Quaternion.Euler(0f, 0f, CameraCache.Main.transform.localRotation.eulerAngles.y);
	}
}
