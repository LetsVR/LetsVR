using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

namespace LetsVR.XR.UI
{
	[RequireComponent(typeof(Canvas))]
	public class DisableVRTKCanvas : MonoBehaviour
	{
		Camera mainCamera;

		private IEnumerator Start()
		{
			yield return new WaitUntil(() => Camera.main != null);
			mainCamera = Camera.main;

			if (!AppState.IsDesktopMode)
				yield break;

			DisableVrtkCanvas();
		}

		private void Update()
		{
			if (!AppState.IsDesktopMode)
				return;

			DisableVrtkCanvas();
		}

		private void DisableVrtkCanvas()
		{
			var canvas = GetComponent<Canvas>();
			canvas.worldCamera = mainCamera;

			var gr = GetComponent<GraphicRaycaster>();
			if (gr != null)
				gr.enabled = true;

			var vrtkCanvas = GetComponent<VRTK_UICanvas>();
			if (vrtkCanvas != null)
				vrtkCanvas.enabled = false;
		}
	}
}