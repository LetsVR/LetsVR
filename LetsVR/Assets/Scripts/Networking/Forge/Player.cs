using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using LetsVR.XR.Utilities;
using UnityEngine;

namespace LetsVR.XR.Networking.Forge
{
	public sealed class Player : PlayerBehavior
	{
		[SerializeField] TMPro.TextMeshPro textMesh;

		[Header("Debug")]
		[SerializeField] ulong playerIdentifier;
		[SerializeField] internal bool isDesktop;

		internal GameObject CameraParent;

		public string Name { get; private set; }


		public static Player PlayerInLocalScene { get; set; }

		protected override void NetworkStart()
		{
			base.NetworkStart();

			Debug.Log($"[Player:{networkObject.Owner.NetworkId}] Network start");

			if (networkObject.IsOwner)
			{
				PlayerInLocalScene = this;

				// Assign the name when this object is setup on the network
				ChangeName();
			}
		}

		private void Update()
		{
			if (networkObject == null)
				return;

			isDesktop = networkObject.platformId == 0;

			if (networkObject.IsOwner)
			{
				// When our position changes the networkObject.position will detect the
				// change based on this assignment automatically, this data will then be
				// syndicated across the network on the next update pass for this networkObject
				networkObject.position = transform.position;
				networkObject.rotation = transform.rotation;
			}
			else
			{
				// If we are not the owner then we set the position to the
				// position that is syndicated across the network for this player
				transform.position = networkObject.position;
				transform.rotation = networkObject.rotation;
			}

			AlignWithCamera();
		}

		public ulong GetNetworkIdentifier()
		{
			return Name?.Hash() ?? 0L;
		}

		public override void UpdateName(RpcArgs args)
		{
			if (this == null)
				return;

			// Since there is only 1 argument and it is a string we can safely
			// cast the first argument to a string knowing that it is going to
			// be the name for this player
			Name = args.GetNext<string>();

			transform.name = Name;
			networkObject.Owner.Name = Name; // update the name of the NetworkingPlayer for when destroying the player & controllers
			SetPlayerIdentificators();

			playerIdentifier = GetNetworkIdentifier();
		}

		private void SetPlayerIdentificators()
		{
			textMesh.text = Name;
		}

		void ChangeName()
		{
			// Only the owning client of this object can assign the name
			if (!networkObject.IsOwner)
				return;

			Name = AppState.MultiuserName;
			networkObject.Owner.Name = Name; // update the name of the NetworkingPlayer for when destroying the player & controllers

			SetPlayerIdentificators();
			
			// Send an RPC to let everyone know what the name is for this player
			// We use "AllBuffered" so that if people come late they will get the
			// latest name for this object
			// We pass in "Name" for the args because we have 1 argument that is to
			// be a string as it is set in the NCW
			networkObject.SendRpc(RPC_UPDATE_NAME, Receivers.AllBuffered, Name);
		}

		void AlignWithCamera()
		{
			// make player object align with the camera object
			if (!CameraParent)
				return;

			var camera = CameraParent.GetComponent<Camera>();
			if (!camera)
				camera = CameraParent.GetComponentInChildren<Camera>(); 

			if (camera)
			{
				transform.SetParent(camera.gameObject.transform);
				transform.localPosition = Vector3.zero;
			}
		}
	}
}
