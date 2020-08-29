using BeardedManStudios.Forge.Networking.Unity;
using LetsVR.XR.Utilities;
using UnityEngine;

namespace LetsVR.XR.Networking.Forge
{
	public class PlayerManager
	{ 
		public static void CreatePlayer(GameObject cameraParent)
		{
			if (NetworkManager.Instance == null)
				return;

			// create player
			var player = (Player)NetworkManager.Instance.InstantiatePlayer(position: Vector3.zero, rotation: Quaternion.identity);
			player.networkObject.platformId = Application.platform == RuntimePlatform.Android ? (short)1 : (short)0;
			player.CameraParent = cameraParent;
			ulong playerIdentifier = AppState.MultiuserName.Hash();

			// create controllers
			var leftControllerSync = NetworkManager.Instance.InstantiateControllerSync() as ControllerSync;
			leftControllerSync.isLeftController = true;
			leftControllerSync.networkObject.playerIdentifier = playerIdentifier; 

			var rightControllerSync = NetworkManager.Instance.InstantiateControllerSync() as ControllerSync;
			rightControllerSync.isLeftController = false;
			rightControllerSync.networkObject.playerIdentifier = playerIdentifier;
		}
	}
}
