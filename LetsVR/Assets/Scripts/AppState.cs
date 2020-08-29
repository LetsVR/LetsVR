using UnityEngine;

namespace LetsVR.XR
{
	static class AppState
	{
		static bool cachedDesktopMode;

		static GameObject vrSim;

		public static bool IsDesktopMode 
		{ 
			get
			{
				if (!vrSim) 
					vrSim = GameObject.Find("[VRTK_SDKManager]/[VRTK_SDKSetups]/VRSimulator");

				cachedDesktopMode = cachedDesktopMode || vrSim.activeInHierarchy;

				return cachedDesktopMode;
			}
		}

		public static bool IsServerMode { get; set; }
		public static string MultiuserName { get; internal set; } = "?";
	}
}
