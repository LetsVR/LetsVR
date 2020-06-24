using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRig : MonoBehaviour
{
    [Range(0, 1)]
    public float turnSmoothness = 0.2f;
    public Transform headConstraint;
    public Vector3 headBodyOffset;

    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    bool startFinished;

    public bool OnlyYOffet { get; internal set; }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => Camera.main != null);
        headBodyOffset = transform.position - headConstraint.position;

        if (OnlyYOffet)
        {
            headBodyOffset.x = headBodyOffset.z = 0;
        }

        startFinished = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!startFinished)
            return;

        transform.position = headConstraint.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(-headConstraint.up, Vector3.up).normalized, turnSmoothness);
    
        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}

[Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;

    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        if (vrTarget == null)
            return;

        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}