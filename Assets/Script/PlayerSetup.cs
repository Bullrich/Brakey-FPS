using UnityEngine;
using UnityEngine.Networking;

// By @Bullrich

namespace game
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerSetup : NetworkBehaviour
    {
        [SerializeField] Behaviour[] componentsToDisable;

        [SerializeField] private string
            remoteLayerMask = "RemotePlayer",
            dontDrawLayerName = "DontDraw";

        [SerializeField] private GameObject
            playerGraphics,
            playerUiPrefab;

        [HideInInspector] public GameObject playerUiInstance;

        void Start()
        {
            if (!isLocalPlayer)
            {
                DisableComponents();
                AssignRemoteLayer();
            }
            else
            {
                // Disable player graphics for local player
                SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

                // Create Player UI
                playerUiInstance = Instantiate(playerUiPrefab);
                playerUiInstance.name = playerUiPrefab.name;

                // Configure PlayerUI
                PlayerUI ui = playerUiInstance.GetComponent<PlayerUI>();
                if (ui == null)
                    Debug.LogError("No PlayerUI component on PlatyerUI prefab.");
                ui.SetController((GetComponent<PlayerController>()));
            }

            GetComponent<Player>().Setup(playerUiInstance);
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

        void SetLayerRecursively(GameObject _graphics, int _dontDrawMask)
        {
            _graphics.layer = _dontDrawMask;

            foreach (Transform child in _graphics.transform)
                SetLayerRecursively(child.gameObject, _dontDrawMask);
        }

        void AssignRemoteLayer()
        {
            gameObject.layer = LayerMask.NameToLayer(remoteLayerMask);
        }

        void OnDisable()
        {
            Destroy(playerUiInstance);

            GameManager.instance.SetSceneCameraActive(true);

            GameManager.UnRegisterPlayer(transform.name);
        }
    }
}