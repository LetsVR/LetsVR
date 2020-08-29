using UnityEngine;

namespace LetsVR.XR
{
	static class AppState
	{
		public static bool IsServerMode { get; set; }
		public static string MultiuserName { get; internal set; } = "?";
	}
}
