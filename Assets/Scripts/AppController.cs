using UnityEngine;

// Primary entry point for the application.
// Drives the AppStateManager with a faux managed update.

public class AppController : MonoBehaviour
{
    [SerializeField] AppState initialState;

    void Start()
    {
        if (initialState != null)
            AppStateManager.Initialize(initialState);

        else
            Debug.LogError("App cannot initialize without an initial state!", gameObject);
    }

    void Update()
    {
        AppStateManager.UpdateState();
    }
}
