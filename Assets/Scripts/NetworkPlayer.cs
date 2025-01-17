/*
 * NetworkPlayer - Multiplayer VR Player Synchronization
 * 
 * This script synchronizes the player's head and hand movements across the network using Photon PUN.
 * It supports VR tracking and animation syncing for other players to see the correct movements.
 * 
 * Key Features:
 * - Tracks and updates the player's head and hands in a networked VR environment.
 * - Hides the local player's body to prevent self-view obstruction.
 * - Synchronizes position and rotation using `OnPhotonSerializeView` for smooth multiplayer experience.
 * - Uses Unity's XR Interaction Toolkit for VR input handling.
 * 
 * Author: [Your Name]
 */

using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Unity.XR.CoreUtils;

public class NetworkPlayer : MonoBehaviourPun, IPunObservable
{
    public Transform Head;
    public Transform LeftHand;
    public Transform RightHand;

    private PhotonView photonView;

    public Animator LeftHandAnimator;
    public Animator RightHandAnimator;

    private Transform HeadRig;
    private Transform LeftHandRig;
    private Transform RightHandRig;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        XROrigin rig = FindObjectOfType<XROrigin>();
        HeadRig = rig.transform.Find("Camera Offset/Main Camera");
        LeftHandRig = rig.transform.Find("Camera Offset/Left Controller");
        RightHandRig = rig.transform.Find("Camera Offset/Right Controller");

        // Hide local player's body (Prevent self-view)
        if (photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(Head, HeadRig);
            MapPosition(LeftHand, LeftHandRig);
            MapPosition(RightHand, RightHandRig);

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), LeftHandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), RightHandAnimator);
        }
    }

    // Updates hand animation values based on VR controller input
    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    // Maps the tracked VR rig positions to the network player
    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }

    // Photon Synchronization - Send & Receive Position Data
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // Send data to others
        {
            stream.SendNext(Head.position);
            stream.SendNext(Head.rotation);
            stream.SendNext(LeftHand.position);
            stream.SendNext(LeftHand.rotation);
            stream.SendNext(RightHand.position);
            stream.SendNext(RightHand.rotation);
        }
        else // Receive data from other players
        {
            Head.position = (Vector3)stream.ReceiveNext();
            Head.rotation = (Quaternion)stream.ReceiveNext();
            LeftHand.position = (Vector3)stream.ReceiveNext();
            LeftHand.rotation = (Quaternion)stream.ReceiveNext();
            RightHand.position = (Vector3)stream.ReceiveNext();
            RightHand.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
