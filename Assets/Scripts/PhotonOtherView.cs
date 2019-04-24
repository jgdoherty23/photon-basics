using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


[RequireComponent(typeof(PhotonView))]
public class PhotonOtherView : MonoBehaviour, IPunObservable
{
    public bool syncRendererEnabled = true;

    private PhotonView photonView;

    private Renderer renderer;

    private bool networkRendererEnabled;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        renderer = GetComponent<Renderer>();

        networkRendererEnabled = renderer.enabled;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (syncRendererEnabled)
            {
                stream.SendNext(renderer.enabled);
            }
        }
        else
        {
            if (syncRendererEnabled)
            {
                renderer.enabled = (bool)stream.ReceiveNext();
            }
        }
    }
}
