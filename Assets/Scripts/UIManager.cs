using UnityEngine;

/// <summary>
/// Enables and disables UI root objects based on AppStateChange events.
/// </summary>
public class UIManager : MonoBehaviour
{
	[SerializeField] GameObject initializationUIRoot;
	[SerializeField] GameObject mainAppUIRoot;

	private void Awake()
	{
		initializationUIRoot.SetActive(false);
		mainAppUIRoot.SetActive(false);
	}

	void OnEnable()
	{
		if (TryGetRootForState(AppStateManager.CurrentState, out var root))
			root.SetActive(true);
		
		AppStateManager.OnAppStateChange += OnAppStateChange;
	}
	void OnDisable()
	{
		AppStateManager.OnAppStateChange -= OnAppStateChange;
	}

	private void OnAppStateChange(AppState oldstate, AppState newState)
	{
		if (TryGetRootForState(oldstate, out var oldStateRoot))
			oldStateRoot.SetActive(false);

		if (TryGetRootForState(newState, out var newStateRoot))
			newStateRoot.SetActive(true);
	}

	private bool TryGetRootForState(AppState state, out GameObject root)
	{
		switch(state)
		{
			case var _null when state is null:
				goto default;

			case var initialization when state is InitializationState:
				root = initializationUIRoot;
				return true;
			case var main when state is MainAppState:
				root = mainAppUIRoot;
				return true;

			default:
				root = null;
				return false;
		}
	}
}
