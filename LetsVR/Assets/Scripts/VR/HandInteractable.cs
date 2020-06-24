using OculusSampleFramework;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
[RequireComponent(typeof(ButtonController))]
public class HandInteractable : MonoBehaviour
{
    public UnityEvent proximityEvent;
    public UnityEvent contactEvent;
    public UnityEvent actionEvent;
    public UnityEvent defaultEvent;

	private VRTK_InteractableObject interactableObject;
    private ButtonController buttonController;

    void Start()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();
        buttonController = GetComponent<ButtonController>();

        buttonController.InteractableStateChanged.AddListener(OnStateChanged);
    }

	private void OnStateChanged(InteractableStateArgs state)
	{
        if (state.NewInteractableState == InteractableState.ProximityState)
            proximityEvent?.Invoke();
        else if (state.NewInteractableState == InteractableState.ActionState)
        {
            actionEvent?.Invoke();
            interactableObject.OnInteractableObjectUsed(new InteractableObjectEventArgs { interactingObject = state.ColliderArgs.Collider.Collider.gameObject });
        }
        else if (state.NewInteractableState == InteractableState.ContactState)
            contactEvent?.Invoke();
        else
            defaultEvent?.Invoke();
	}
}
