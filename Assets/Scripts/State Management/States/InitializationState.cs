using UnityEngine;

public class InitializationState : AppState
{
	[SerializeField] GameObject anchorPrefab;
	[SerializeField] AppState nextState;

	const string ROOT_ANCHOR_KEY = "ROOTANCHOR";

	public void OnStartButtonClicked()
	{		
		#if UNITY_EDITOR
		AppStateManager.ChangeState(nextState);
		#endif

		var rayStart = CameraCache.Main.transform.position + (CameraCache.Main.transform.forward * 1.5f);
		var rayDirection = Vector3.down;
		Ray ray = new Ray(rayStart, rayDirection);
		if(AnchorCreator.Instance.CreateAnchor(ray, anchorPrefab, ROOT_ANCHOR_KEY))
		{
			var anchor = AnchorCreator.Instance.AnchorPoints[ROOT_ANCHOR_KEY];
			AppStateManager.ChangeState(nextState);
		}
	}
}
