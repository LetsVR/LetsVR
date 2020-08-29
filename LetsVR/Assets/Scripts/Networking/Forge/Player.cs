﻿using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using LetsVR.XR.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LetsVR.XR.Networking.Forge
{
	public sealed class Player : PlayerBehavior
	{
		Transform avatar;

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

			isDesktop = networkObject.platformId == 0;

			if (isDesktop)
				avatar = gameObject.transform.Find("DesktopAvatar");
			else
				avatar = gameObject.transform.Find("OculusAvatar");

			if (networkObject.IsOwner)
			{
				PlayerInLocalScene = this;

				// Assign the name when this object is setup on the network
				ChangeName();

				//if (networkObject.Networker is IClient)
				//	StartCoroutine(SetupOwnerVRRig());
			}

			avatar.gameObject.SetActive(true);
		}

		private void Update()
		{
			if (networkObject == null)
				return;

			if (networkObject.IsOwner)
			{
				// When our position changes the networkObject.position will detect the
				// change based on this assignment automatically, this data will then be
				// syndicated across the network on the next update pass for this networkObject
				networkObject.position = transform.position;
				networkObject.rotation = transform.rotation;
			}
			// Check to see if we are the owner of this player
			else
			{
				// If we are not the owner then we set the position to the
				// position that is syndicated across the network for this player
				transform.position = networkObject.position;
				transform.rotation = networkObject.rotation;

				//StartCoroutine(SetupGuestVRRig());
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
