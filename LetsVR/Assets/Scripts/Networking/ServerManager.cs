using LetsVR.XR.Networking.Forge;
using LetsVR.XR.Utilities;
using UnityEngine;

namespace LetsVR.XR.Networking
{
	public class ServerManager : MonoBehaviour
	{
		[Header("Forge networking")]
		[SerializeField] GameObject NetworkManager;

		void Start()
		{
			AppState.IsServerMode = true;
			Application.targetFrameRate = 30;

			ForgeNetwork.MaxConnections = 64;
			ForgeNetwork.StartStopServer("127.0.0.1", 15010, NetworkManager, true);
		}
	}
}