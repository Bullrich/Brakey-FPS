using UnityEngine;
using UnityEngine.Networking;

// By @Bullrich

namespace game
{
    [RequireComponent(typeof(Player))]
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

            GetComponent<Player>().Setup();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            string _netID = GetComponent<NetworkIdentity>().netId.ToString();
            Player _player = GetComponent<Player>();

            GameManager.RegisterPlayer(_netID, _player);
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

                GameManager.UnRegisterPlayer(transform.name);
        }
    }
}