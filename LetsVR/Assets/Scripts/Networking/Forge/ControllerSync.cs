using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using LetsVR.XR.Utilities;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using prvncher.MixedReality.Toolkit.OculusQuestInput;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LetsVR.XR.Networking.Forge
{
	class DummyInputSource : IMixedRealityInputSource
	{
		public IMixedRealityPointer[] Pointers => Array.Empty<IMixedRealityPointer>();

		public InputSourceType SourceType => InputSourceType.Other;

		public uint SourceId => 100000;

		public string SourceName => "Manual";

		public new bool Equals(object x, object y)
		{
			return ReferenceEquals(x, y);
		}

		public int GetHashCode(object obj)
		{
			return obj.GetHashCode();
		}
	}
	class DummyController : IMixedRealityController
	{
		public bool Enabled { get; set; }

		public TrackingState TrackingState => TrackingState.Tracked;

		public Handedness ControllerHandedness => Handedness.Left;

		public IMixedRealityInputSource InputSource => new DummyInputSource();

		public IMixedRealityControllerVisualizer Visualizer => null;

		public bool IsPositionAvailable => true;

		public bool IsPositionApproximate => false;

		public bool IsRotationAvailable => true;

		public MixedRealityInteractionMapping[] Interactions => Array.Empty<MixedRealityInteractionMapping>();

		public Vector3 AngularVelocity => Vector3.zero;

		public Vector3 Velocity => Vector3.zero;

		public bool IsInPointingPose => true;
	}

	public class ControllerSync : ControllerSyncBehavior
	{
		[Header("Forge synced values")]
		public bool isLeftController = true;
		public bool isHandsMode = true;

		[Header("Oculus hands and controllers")]
		[SerializeField] GameObject OVRLeftHand;
		[SerializeField] GameObject OVRRightHand;
		[SerializeField] GameObject OVRLeftController;
		[SerializeField] GameObject OVRRightController;

		[Header("Pointer")]
		[SerializeField] GameObject Pointer;

		[Header("Debug")]
		[SerializeField] ulong playerIdentifier;
		[SerializeField] bool isFocusedLocked;
		[SerializeField] Vector3 pointerPosition;
		[SerializeField] Quaternion pointerRotation;

		// class
		Transform anchor;

		Player player;

		private void Awake()
		{
			Pointer.GetComponent<ShellHandRayPointer>().Controller = new DummyController { Enabled = true };
		}

		private void Update()
		{
			if (networkObject == null)
				return;

			playerIdentifier = networkObject.playerIdentifier;

			if (player == null)
			{
				GetPlayer();
			}

			if (!networkObject.IsOwner)
			{
				// If we are not the owner then we set the position to the
				// position that is syndicated across the network for this player
				transform.position = networkObject.position;
				transform.rotation = networkObject.rotation;
				isLeftController = networkObject.isLeftController;
				isHandsMode = networkObject.isHandsMode;

				pointerPosition = networkObject.pointerPosition;
				pointerRotation = networkObject.pointerRotation;

				// show hands only if the player is a device, not desktop
				if (player != null)
				{
					if (player.isDesktop)
					{
						OVRLeftHand.SetActive(false);
						OVRRightHand.SetActive(false);
						OVRLeftController.SetActive(false);
						OVRRightController.SetActive(false);

						Pointer.SetActive(false);
					}
					else
					{
						OVRLeftHand.SetActive(isLeftController && isHandsMode);
						OVRRightHand.SetActive(!isLeftController && isHandsMode);
						OVRLeftController.SetActive(isLeftController && !isHandsMode);
						OVRRightController.SetActive(!isLeftController && !isHandsMode);

						// sync Owner pointers
						Pointer.transform.localPosition = transform.InverseTransformPoint(pointerPosition);
						Pointer.transform.localRotation = Quaternion.Inverse(transform.rotation) * pointerRotation;
					}
				}
				return;
			}

			Handedness handedness = isLeftController ? Handedness.Left : Handedness.Right;
			var hand = HandJointUtils.FindHand(handedness);

			if (hand is BaseHand)
			{
				isHandsMode = true;
			}
			else if (hand is BaseController)
			{
				isHandsMode = false;
			}

			// TODO: handle SteamVR
			if (hand is OculusQuestController || hand is OculusQuestHand)
			{
				var playSpace = GameObject.Find("MixedRealityPlayspace");

				if (anchor == null)
				{
					if (isLeftController)
						anchor = playSpace.GetComponentInChildren<OVRCameraRig>()?.transform.Find("TrackingSpace/LeftHandAnchor");
					else
						anchor = playSpace.GetComponentInChildren<OVRCameraRig>()?.transform.Find("TrackingSpace/RightHandAnchor");
				}

				if (anchor != null)
				{
					transform.position = anchor.position;
					transform.rotation = anchor.rotation;
				}

				// sync Owner pointers
				var generatedPointers = playSpace.GetComponentsInChildren<ShellHandRayPointer>();

				foreach (var poseSync in generatedPointers)
				{
					if (isLeftController && poseSync.Handedness == Handedness.Left)
					{
						pointerPosition = poseSync.transform.position;
						pointerRotation = poseSync.transform.rotation;

						Pointer.transform.localPosition = transform.InverseTransformPoint(pointerPosition);
						Pointer.transform.localRotation = Quaternion.Inverse(transform.rotation) * pointerRotation;
					}
					if (!isLeftController && poseSync.Handedness == Handedness.Right)
					{
						pointerPosition = poseSync.transform.position;
						pointerRotation = poseSync.transform.rotation;

						Pointer.transform.localPosition = transform.InverseTransformPoint(pointerPosition);
						Pointer.transform.localRotation = Quaternion.Inverse(transform.rotation) * pointerRotation;
					}
				}
			}

			// When our position changes the networkObject.position will detect the
			// change based on this assignment automatically, this data will then be
			// syndicated across the network on the next update pass for this networkObject
			networkObject.isLeftController = isLeftController;
			networkObject.isHandsMode = isHandsMode;
			networkObject.position = transform.position;
			networkObject.rotation = transform.rotation;

			networkObject.pointerPosition = pointerPosition;
			networkObject.pointerRotation = pointerRotation;

			if (transform.localScale == Vector3.zero)
			{
				transform.localScale = Vector3.one;
			}
		}

		private void GetPlayer()
		{
			List<NetworkObject> networkObjectList = NetworkManager.Instance.Networker.NetworkObjectList;

			foreach (NetworkObject netobj in networkObjectList)
			{
				if (!(netobj is PlayerNetworkObject))
					continue;

				var syncObject = (PlayerNetworkObject)netobj;

				if (syncObject.AttachedBehavior is Player attachedBehavior)
				{
					if (string.IsNullOrEmpty(attachedBehavior.Name))
						break;

					if (attachedBehavior.GetNetworkIdentifier() == networkObject.playerIdentifier)
					{
						player = attachedBehavior;
						break;
					}
				}
			}
		}
	}
}
