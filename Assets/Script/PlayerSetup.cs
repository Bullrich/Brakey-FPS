using UnityEngine;
using UnityEngine.Networking;

// By @Bullrich

namespace game
{

    public class PlayerSetup : NetworkBehaviour
    {

        [SerializeField]
        Behaviour[] componentsToDisable;

        [SerializeField]
        string remoteLayerMask = "RemotePlayer";

        Camera sceneCamera;

        void Start()
        {
            if (!isLocalPlayer)
            {
                DisableComponents();
                AssignRemoteLayer();
            }
            else
            {
                sceneCamera = Camera.main;
                if (sceneCamera != null)
                    sceneCamera.gameObject.SetActive(false);
            }
            RegisterPlayer();
        }

        void RegisterPlayer()
        {
            string _id = "Player " + GetComponent<NetworkIdentity>().netId;
            transform.name = _id;
        }

        void DisableComponents()
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }

        void AssignRemoteLayer()
        {
            gameObject.layer = LayerMask.NameToLayer(remoteLayerMask);
        }

        void OnDisable()
        {
            if (sceneCamera != null)
                sceneCamera.gameObject.SetActive(true);
        }
    }
}