using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class AnchorCreator : MonoBehaviour
{
	// Removes all the anchors that have been created.
	public void RemoveAllAnchors()
	{
		foreach (var kvp in m_AnchorPoints)
		{
			Destroy(kvp.Value);
		}
		m_AnchorPoints.Clear();
	}

	static AnchorCreator instance;
	public static AnchorCreator Instance => instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(this);

		m_RaycastManager = GetComponent<ARRaycastManager>();
		m_AnchorManager = GetComponent<ARAnchorManager>();
		m_PlaneManager = GetComponent<ARPlaneManager>();
		m_AnchorPoints = new Dictionary<string, ARAnchor>();

		//TODO Listen for state changes and reset on reset.
		//ARSession.stateChanged += (x) => m_AnchorPoints.Clear();
	}

	public bool CreateAnchor(Ray ray, GameObject prefab = null, string label = null)
	{
		if (m_RaycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
		{
			// Raycast hits are sorted by distance, so the first one
			// will be the closest hit.
			var hitPose = s_Hits[0].pose;
			var hitTrackableId = s_Hits[0].trackableId;
			var hitPlane = m_PlaneManager.GetPlane(hitTrackableId);

			// This attaches an anchor to the area on the plane corresponding to the raycast hit,
			// and afterwards instantiates an instance of your chosen prefab at that point.
			// This prefab instance is parented to the anchor to make sure the position of the prefab is consistent
			// with the anchor, since an anchor attached to an ARPlane will be updated automatically by the ARAnchorManager as the ARPlane's exact position is refined.
			var anchor = m_AnchorManager.AttachAnchor(hitPlane, hitPose);

			if (anchor == null)
			{
				Debug.Log("Error creating anchor.");
				return false;
			}

			// Stores the anchor so that it may be removed later.
			var key = string.IsNullOrEmpty(label) ? anchor.trackableId.ToString() : label;

			if (m_AnchorPoints.ContainsKey(label))
					m_AnchorPoints[label] = anchor;
			else
					m_AnchorPoints.Add(key, anchor);

			if (prefab != null)
					Instantiate(prefab, anchor.transform);
			
			OnAnchorPointsChanged?.Invoke();
			return true;
		}

		return false;
	}

	static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

	Dictionary<string, ARAnchor> m_AnchorPoints;
	
	public Dictionary<string, ARAnchor> AnchorPoints => m_AnchorPoints;
	public UnityEvent OnAnchorPointsChanged = new UnityEvent();

	ARRaycastManager m_RaycastManager;

	ARAnchorManager m_AnchorManager;

	ARPlaneManager m_PlaneManager;
}
