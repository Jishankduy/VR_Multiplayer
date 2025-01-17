using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class XRGrabableInteraction : XRGrabInteractable
{
    private PhotonView photonView; // Reference to PhotonView component for network ownership

    // Called when the object is first initialized
    protected override void Awake()
    {
        base.Awake(); // Call the base Awake method of XRGrabInteractable

        // Get the PhotonView component attached to this object
        photonView = GetComponent<PhotonView>();

        // Ensure PhotonView is present, otherwise log an error
        if (photonView == null)
        {
            Debug.LogError($"PhotonView missing on {gameObject.name}");
        }
    }

    // Called when an interaction (grab attempt) begins
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args); // Call the base method to maintain default XR interaction behavior

        // Check if the object is not already owned by the local player
        if (photonView != null && !photonView.IsMine && photonView.Owner != PhotonNetwork.LocalPlayer)
        {
            photonView.RequestOwnership(); // Request ownership from the network
        }
    }
}
