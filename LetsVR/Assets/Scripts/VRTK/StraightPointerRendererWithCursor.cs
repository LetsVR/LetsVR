// Straight Pointer Renderer|PointerRenderers|10020
namespace VRTK
{
    using LetsVR.XR.Utilities;
    using UnityEngine;

    public class StraightPointerRendererWithCursor : VRTK_BasePointerRenderer
    {
        [SerializeField] bool HideWhenTeleport;

        [Header("Straight Pointer Appearance Settings")]

        [Tooltip("The maximum length the pointer tracer can reach.")]
        public float maximumLength = 100f;
        [Tooltip("The scale factor to scale the pointer tracer object by.")]
        public float scaleFactor = 0.002f;
        [Tooltip("The scale multiplier to scale the pointer cursor object by in relation to the `Scale Factor`.")]
        public float cursorScaleMultiplier = 250f;
        [Tooltip("The cursor will be rotated to match the angle of the target surface if this is true, if it is false then the pointer cursor will always be horizontal.")]
        public bool cursorMatchTargetRotation = false;
        [Tooltip("Rescale the cursor proportionally to the distance from the tracer origin.")]
        public bool cursorDistanceRescale = false;
        [Tooltip("The maximum scale the cursor is allowed to reach. This is only used when rescaling the cursor proportionally to the distance from the tracer origin.")]
        public Vector3 maximumCursorScale = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

        [Header("Straight Pointer Custom Appearance Settings")]

        [Tooltip("A custom game object to use as the appearance for the pointer tracer. If this is empty then a Box primitive will be created and used.")]
        public GameObject customTracer;
        [Tooltip("Custom game objects to use as the appearance for the pointer cursor. If this is empty then a Sphere primitive will be created and used.")]
        public GameObject[] customCursor;
        [Tooltip("Index of the cursor. -1 is the default one.")]
        public int cursorIndex = -1;

        GameObject defaultCursorInstance;
        GameObject customCursorInstance;

		public GameObject actualContainer;
        public GameObject actualTracer;
		public GameObject actualCursor;

        protected Vector3 cursorOriginalScale = Vector3.one;

        /// <summary>
        /// The UpdateRenderer method is used to run an Update routine on the pointer.
        /// </summary>
        public override void UpdateRenderer()
        {
            if (cursorIndex >= 0 && HideWhenTeleport)
                return;

            if ((controllingPointer != null && controllingPointer.IsPointerActive()) || IsVisible())
            {
                float tracerLength = CastRayForward();
                SetPointerAppearance(tracerLength);
                MakeRenderersVisible();
            }
            base.UpdateRenderer();
        }

        /// <summary>
        /// The GetPointerObjects returns an array of the auto generated GameObjects associated with the pointer.
        /// </summary>
        /// <returns>An array of pointer auto generated GameObjects.</returns>
        public override GameObject[] GetPointerObjects()
        {
            return new GameObject[] { actualContainer, actualCursor, actualTracer };
        }

        protected override void ToggleRenderer(bool pointerState, bool actualState)
        {
            ToggleElement(actualTracer, pointerState, actualState, tracerVisibility, ref tracerVisible);
            ToggleElement(actualCursor, pointerState, actualState, cursorVisibility, ref cursorVisible);
        }

        protected override void CreatePointerObjects()
        {
            actualContainer = new GameObject(VRTK_SharedMethods.GenerateVRTKObjectName(true, gameObject.name, "StraightPointerRenderer_Container"));
            actualContainer.transform.SetParent(pointerOriginTransformFollowGameObject.transform);
            actualContainer.transform.localPosition = Vector3.zero;
            actualContainer.transform.localRotation = Quaternion.identity;
            actualContainer.transform.localScale = Vector3.one;
            VRTK_PlayerObject.SetPlayerObject(actualContainer, VRTK_PlayerObject.ObjectTypes.Pointer);

            CreateTracer();
            CreateCursor();
            Toggle(false, false);
            if (controllingPointer != null)
            {
                controllingPointer.ResetActivationTimer(true);
                controllingPointer.ResetSelectionTimer(true);
            }
        }

        protected override void DestroyPointerObjects()
        {
            if (actualCursor != null)
            {
                Destroy(actualCursor);
            }
            if (actualContainer != null)
            {
                Destroy(actualContainer);
            }
        }

        protected override void ChangeMaterial(Color givenColor)
        {
            base.ChangeMaterial(givenColor);
            ChangeMaterialColor(actualTracer, givenColor);
            ChangeMaterialColor(actualCursor, givenColor);
        }

        protected override void UpdateObjectInteractor()
        {
            base.UpdateObjectInteractor();
            //if the object interactor is too far from the pointer tip then set it to the pointer tip position to prevent glitching.
            if (objectInteractor != null && actualCursor != null && Vector3.Distance(objectInteractor.transform.position, actualCursor.transform.position) > 0f)
            {
                objectInteractor.transform.position = actualCursor.transform.position;
            }
        }

        protected virtual void CreateTracer()
        {
            if (customTracer != null)
            {
                actualTracer = Instantiate(customTracer);
            }
            else
            {
                actualTracer = GameObject.CreatePrimitive(PrimitiveType.Cube);
                actualTracer.GetComponent<BoxCollider>().isTrigger = true;
                //actualTracer.AddComponent<Rigidbody>().isKinematic = true;
                actualTracer.layer = LayerMask.NameToLayer("Ignore Raycast");

                SetupMaterialRenderer(actualTracer);
            }

            actualTracer.transform.name = VRTK_SharedMethods.GenerateVRTKObjectName(true, gameObject.name, "StraightPointerRenderer_Tracer");
            actualTracer.transform.SetParent(actualContainer.transform);

            VRTK_PlayerObject.SetPlayerObject(actualTracer, VRTK_PlayerObject.ObjectTypes.Pointer);
        }

        public void UpdateCustomCursor(int index)
        {
            cursorIndex = index;

            if (index >= 0)
                CreateCustomCursor();
        }

        protected virtual void CreateCursor()
        {
            if (customCursor != null)
            {
                CreateCustomCursor();
            }

            actualCursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            actualCursor.transform.localScale = Vector3.one * (scaleFactor * cursorScaleMultiplier);
            actualCursor.GetComponent<Collider>().isTrigger = true;
            //actualCursor.AddComponent<Rigidbody>().isKinematic = true;
            actualCursor.layer = LayerMask.NameToLayer("Ignore Raycast");

            defaultCursorInstance = actualCursor;

            SetupMaterialRenderer(actualCursor);


            cursorOriginalScale = actualCursor.transform.localScale;
            actualCursor.transform.name = VRTK_SharedMethods.GenerateVRTKObjectName(true, gameObject.name, "StraightPointerRenderer_Cursor");
            actualCursor.transform.SetParent(actualContainer.transform);
            VRTK_PlayerObject.SetPlayerObject(actualCursor, VRTK_PlayerObject.ObjectTypes.Pointer);
        }

        private void CreateCustomCursor()
        {
            if (cursorIndex < 0 || cursorIndex > customCursor.Length)
                return;

            if (customCursorInstance != null)
                Destroy(customCursorInstance);

            customCursorInstance = Instantiate(customCursor[cursorIndex]);
            customCursorInstance.SetActive(false);
            customCursorInstance.transform.name = VRTK_SharedMethods.GenerateVRTKObjectName(true, gameObject.name, "StraightPointerRenderer_Cursor");
            customCursorInstance.transform.localScale = Vector3.one;
            customCursorInstance.transform.SetParent(actualContainer.transform);
            VRTK_PlayerObject.SetPlayerObject(customCursorInstance, VRTK_PlayerObject.ObjectTypes.Pointer);
        }

        protected virtual void CheckRayMiss(bool rayHit, RaycastHit pointerCollidedWith)
        {
            if (!rayHit || (destinationHit.collider != null && destinationHit.collider != pointerCollidedWith.collider))
            {
                if (destinationHit.collider != null)
                {
                    PointerExit(destinationHit);
                }

                destinationHit = new RaycastHit();
                ChangeColor(invalidCollisionColor);
            }
        }

        protected virtual void CheckRayHit(bool rayHit, RaycastHit pointerCollidedWith)
        {
            if (rayHit)
            {
                PointerEnter(pointerCollidedWith);

                destinationHit = pointerCollidedWith;
                ChangeColor(validCollisionColor);
            }
		}

		public void SetValidColor()
		{
			ChangeColor(validCollisionColor);
		}

		protected virtual float CastRayForward()
        {
            Transform origin = GetOrigin();
            Ray pointerRaycast = new Ray(origin.position, origin.forward);
            RaycastHit pointerCollidedWith;
            bool rayHit = VRTK_CustomRaycast.Raycast(customRaycast, pointerRaycast, out pointerCollidedWith, defaultIgnoreLayer, maximumLength);

            CheckRayMiss(rayHit, pointerCollidedWith);
            CheckRayHit(rayHit, pointerCollidedWith);

            float actualLength = maximumLength;
            if (rayHit && pointerCollidedWith.distance < maximumLength)
            {
                actualLength = pointerCollidedWith.distance;
            }

            return OverrideBeamLength(actualLength);
        }

        public override bool IsValidCollision()
        {
            return true;
        }

        protected virtual void SetPointerAppearance(float tracerLength)
        {
            if (actualContainer != null)
            {
                //if the additional decimal isn't added then the beam position glitches
                float beamPosition = tracerLength / (2f + BEAM_ADJUST_OFFSET);

                actualTracer.transform.localScale = new Vector3(scaleFactor, scaleFactor, tracerLength);
                actualTracer.transform.localPosition = Vector3.forward * beamPosition;

                bool showCustomCursor = false;

                if (customCursorInstance)
					customCursorInstance.SetActive(showCustomCursor);
                
				defaultCursorInstance.SetActive(!showCustomCursor);
                actualCursor = showCustomCursor ? customCursorInstance : defaultCursorInstance;

                //Debug.Log(destinationHit.transform);

                if (showCustomCursor)
                {
                    if (cursorIndex == 0)
                        actualCursor.transform.localScale = Vector3.one;
                }
                //else if (destinationHit.transform != null && destinationHit.transform.name == "Keyboard")
                //    actualCursor.transform.localScale = Vector3.one * 0.3f * (scaleFactor * cursorScaleMultiplier);
                else
                    actualCursor.transform.localScale = Vector3.one * (scaleFactor * cursorScaleMultiplier);
                actualCursor.transform.localPosition = new Vector3(0f, 0f, tracerLength);

                Transform origin = GetOrigin();

                float objectInteractorScaleIncrease = 1.05f;
                ScaleObjectInteractor(actualCursor.transform.lossyScale * objectInteractorScaleIncrease);

                if (destinationHit.transform != null)
                {

                    if (cursorMatchTargetRotation)
                    {
                        actualCursor.transform.forward = -destinationHit.normal;
                    }
                    if (cursorDistanceRescale)
                    {
                        float collisionDistance = Vector3.Distance(destinationHit.point, origin.position) * 0.5f;
                        if (showCustomCursor)
                        {
                            if (cursorIndex == 0)
                                actualCursor.transform.localScale = Vector3.one;
                        }
                        else
                            actualCursor.transform.localScale = Vector3.Min(cursorOriginalScale * collisionDistance, maximumCursorScale);
                    }

                    if (showCustomCursor)
                    {

                        actualCursor.transform.LookAt(actualTracer.transform);
                        Vector3 lookAtVector = actualCursor.transform.rotation.eulerAngles;
                        Vector3 lookTopVector = Quaternion.FromToRotation(Vector3.up, destinationHit.normal).eulerAngles;
                        actualCursor.transform.rotation = Quaternion.Euler(lookTopVector.x, lookAtVector.y, lookAtVector.z);

                    }
                    else
                        actualCursor.transform.rotation = Quaternion.FromToRotation(Vector3.up, destinationHit.normal);

                }
                else
                {
                    if (cursorMatchTargetRotation)
                    {
                        actualCursor.transform.forward = origin.forward;
                    }
                    if (cursorDistanceRescale)
                    {
                        actualCursor.transform.localScale = Vector3.Min(cursorOriginalScale * tracerLength, maximumCursorScale);
                    }
                }

                ToggleRenderer(controllingPointer.IsPointerActive(), false);
                UpdateDependencies(actualCursor.transform.position);
            }
        }
    }
}