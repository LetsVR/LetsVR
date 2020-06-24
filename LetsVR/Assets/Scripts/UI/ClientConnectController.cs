using LetsVR.XR.Networking.Forge;
using LetsVR.XR.Utilities;
using UnityEngine;

namespace LetsVR.XR.UI
{
	public class ClientConnectController : MonoBehaviour
	{
		[Header("Forge networking")]
		[SerializeField] GameObject NetworkManager;

		public void Connect()
		{
			ForgeNetwork.StartStopClient("127.0.0.1", 15010,
										NetworkManager,
										true,
										null,
										() => PlayerManager.CreatePlayer(Camera.main.gameObject));
		}
	}
}