using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


/// <summary>
/// Allows syncing for different properties without tying script to GameObject
/// </summary>
[RequireComponent(typeof(PhotonView))]
public class PhotonOtherView : MonoBehaviour, IPunObservable
{
    public GameObject otherGameObject;

    public bool syncGameObjectActive = true;
    public bool syncRendererEnabled = true;

    private PhotonView photonView;

    private Renderer otherRenderer;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if (otherGameObject == null)
        {
            Debug.LogError("PhotonOtherView: Did not assign otherGameObject. Disabling");
            enabled = false;
            return;
        }
        if (syncRendererEnabled)
        {
            otherRenderer = otherGameObject.GetComponent<Renderer>();
            if (otherRenderer == null)
            {
                Debug.LogError("PhotonOtherView: Could not find Renderer on otherGameObject. Disabling");
                enabled = false;
                return;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (syncGameObjectActive)
            {
                bool isActive = otherGameObject.activeSelf;
                stream.SendNext(isActive);
            }
            if (syncRendererEnabled)
            {
                stream.SendNext(otherRenderer.enabled);
            }
        }
        else
        {
            if (syncGameObjectActive)
            {
                bool isActive = (bool)stream.ReceiveNext();
                otherGameObject.SetActive(isActive);
            }
            if (syncRendererEnabled)
            {
                otherRenderer.enabled = (bool)stream.ReceiveNext();
            }
        }
    }
}
