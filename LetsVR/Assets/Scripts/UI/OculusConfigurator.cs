using LetsVR.XR.Utilities;
using System.Collections;
using UnityEngine;


namespace LetsVR.XR.UI
{
	public class OculusConfigurator : MonoBehaviour
	{
		[Header("Oculus hands and controllers")] //TODO: refactor to separate class
		[SerializeField] GameObject OVRLeftHand;
		[SerializeField] GameObject OVRRightHand;
		[SerializeField] GameObject OVRLeftHandController;
		[SerializeField] GameObject OVRRightHandController;
		[SerializeField] GameObject HandsManager;
		[SerializeField] GameObject InteractableToolsSDKDriver;

		private IEnumerator Start()
		{
			yield return new WaitUntil(() => Camera.main != null);

			ConfigureOculus();
		}

		private void ConfigureOculus()
		{
			if (AppState.IsDesktopMode)
			{
				OVRLeftHand.SetActive(false);
				OVRRightHand.SetActive(false);
				OVRLeftHandController.SetActive(false);
				OVRRightHandController.SetActive(false);
				HandsManager.SetActive(false);
				InteractableToolsSDKDriver.SetActive(false);
			}
			else
			{
				OVRLeftHand.SetActive(AppState.IsHandsMode);
				OVRRightHand.SetActive(AppState.IsHandsMode);
				HandsManager.SetActive(AppState.IsHandsMode);
				InteractableToolsSDKDriver.SetActive(AppState.IsHandsMode);
				OVRLeftHandController.SetActive(!AppState.IsHandsMode);
				OVRRightHandController.SetActive(!AppState.IsHandsMode);
			}
		}
	}
}
