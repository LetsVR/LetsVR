using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

namespace LetsVR.XR.Networking.Forge
{
	public class ControllerSync : ControllerSyncBehavior
	{
		private Transform actualHand;

		[Header("Forge synced values")]
		public bool isLeftController = true;
		public bool isHandsMode = true;

		[Header("Oculus hands and controllers")]
		[SerializeField] GameObject OVRLeftHand;
		[SerializeField] GameObject OVRRightHand;
		[SerializeField] GameObject OVRLeftHandController;
		[SerializeField] GameObject OVRRightHandController;

		[Header("Debug")]
		[SerializeField] ulong playerIdentifier;

		Player player;

		void Start()
		{
			GetComponentInChildren<VRTK_ControllerEvents>().enabled = true;
			GetComponentInChildren<StraightPointerRendererWithCursor>().enabled = true;
			GetComponentInChildren<VRTK_Pointer>().enabled = true;
		}

		private void Update()
		{
			if (networkObject == null)
				return;

			playerIdentifier = networkObject.playerIdentifier;

			GetComponentInChildren<VRTK_ControllerEvents>().enabled = true;
			GetComponentInChildren<StraightPointerRendererWithCursor>().enabled = true;
			GetComponentInChildren<VRTK_Pointer>().enabled = true;

			if (!networkObject.IsOwner)
			{
				// If we are not the owner then we set the position to the
				// position that is syndicated across the network for this player
				transform.position = networkObject.position;
                transform.rotation = networkObject.rotation;
				isLeftController = networkObject.isLeftController;
				isHandsMode = networkObject.isHandsMode;

				// show hands only if the player is a device, not desktop
				if (player != null)
				{
					if (player.isDesktop)
					{
						OVRLeftHand.SetActive(false);
						OVRRightHand.SetActive(false);
						OVRLeftHandController.SetActive(false);
						OVRRightHandController.SetActive(false);
					}
					else
					{
						if (isHandsMode)
						{
							OVRLeftHand.SetActive(isLeftController);
							OVRRightHand.SetActive(!isLeftController);
						}
						else
						{
							OVRLeftHandController.SetActive(isLeftController);
							OVRRightHandController.SetActive(!isLeftController);
						}
					}
				}
				return;
			}

			if (!actualHand)
			{
				VRTK_SDKSetup sdkType = VRTK_SDKManager.GetLoadedSDKSetup();
				if (sdkType != null)
				{
					if (isLeftController)
					{
						actualHand = VRTK_DeviceFinder.GetControllerLeftHand(true).transform;
					}
					else
					{
						actualHand = VRTK_DeviceFinder.GetControllerRightHand(true).transform;
					}
				}
			}

			// get position/rotation from Owner controllers
			if (actualHand != null)
            {
                transform.SetParent(actualHand.parent.transform);
                transform.position = actualHand.position;
                transform.rotation = actualHand.rotation;
            }

			// When our position changes the networkObject.position will detect the
			// change based on this assignment automatically, this data will then be
			// syndicated across the network on the next update pass for this networkObject
			networkObject.isLeftController = isLeftController;
			networkObject.isHandsMode = isHandsMode;
			networkObject.position = transform.position;
            networkObject.rotation = transform.rotation;

            if (transform.localScale == Vector3.zero)
            {
                transform.localScale = Vector3.one;
			}
		}
	}
}
