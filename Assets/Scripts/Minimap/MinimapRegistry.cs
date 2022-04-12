using System.Collections.Generic;

/// <summary>
/// Tracks active observables for MinimapCamera.
/// </summary>
public static class MinimapRegistry 
{	
	private static List<MinimapObservable> knownObservables = new List<MinimapObservable>();
	public static List<MinimapObservable> Observables => knownObservables;

	public static void RegisterObservable(MinimapObservable newObservable)
	{
		if (!knownObservables.Contains(newObservable))
			knownObservables.Add(newObservable);

	}
	public static void UnregisterObservable(MinimapObservable existing)
	{
		if (knownObservables.Contains(existing))
			knownObservables.Remove(existing);
	}
}
