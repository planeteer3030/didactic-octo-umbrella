using System;
using UnityEngine;

public static class AppStateManager
{
    private static AppState state;
    public static AppState CurrentState => state;

    private static bool initialized = false;
    public static bool IsInitialized => initialized;

    public static Action<AppState, AppState> OnAppStateChange;

    public static void Initialize(AppState initialState)
    {
        ChangeState(initialState);
        initialized = true;
    }

    public static void ChangeState(AppState newState)
    {
        if (newState == state)
            return;

#if UNITY_EDITOR
        Debug.Log($"Changing from {state?.GetType().Name} to {newState.GetType().Name}");
#endif

        var lastState = state;
        lastState?.OnExitState();

        OnAppStateChange?.Invoke(lastState, newState);

        state = newState;
        state.OnEnterState();
    }

    public static void UpdateState()
    {
        if (!IsInitialized)
            return;

        state.Update();
    }
}
