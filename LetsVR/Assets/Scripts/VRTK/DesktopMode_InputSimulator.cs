using System.Collections;
using UnityEngine;
using VRTK;

namespace LetsVR.XR.VRTK
{
	public class DesktopMode_InputSimulator : SDK_InputSimulator
    {
		Camera mainCamera;

		private IEnumerator Start()
		{
			yield return new WaitUntil(() => Camera.main != null);
			mainCamera = Camera.main;
		}

		protected override void UpdatePosition()
		{
			if (!AppState.IsDesktopMode)
				return;

			base.UpdatePosition();
        }

        private new void Update()
        {
            base.Update();

            if (mainCamera == null)
                return;

			Ray ray = mainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);

			if (UnityEngine.Input.GetMouseButtonUp(0))
			{
				var hits = Physics.RaycastAll(ray);

				if (hits.Length > 0)
				{
					foreach (var hit in hits)
					{
						// check for VRTK object
						hit.collider?.gameObject?.GetComponent<VRTK_InteractableObject>()?
												  .OnInteractableObjectUsed(new InteractableObjectEventArgs { interactingObject = hit.collider.gameObject });
					}
                }
			}
		}
	}
}