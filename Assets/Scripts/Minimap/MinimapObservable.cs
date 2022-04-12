using UnityEngine;

/// <summary>
/// When attached to a gameObject, creates a marker as a child object
/// visible only to the MinimapCamera.
/// </summary>
public class MinimapObservable : MonoBehaviour
{
	[SerializeField] GameObject prefab;
	GameObject marker;

	private void Awake()
	{
		marker = Instantiate(prefab, transform, false);
		marker.layer = 6;
	}

	private void OnEnable()
	{
		MinimapRegistry.RegisterObservable(this);
	}
	private void OnDisable()
	{
		MinimapRegistry.UnregisterObservable(this);
	}

	public void Orthografy(Transform transform)
	{
		marker.transform.LookAt(transform, transform.up);
	}
}
