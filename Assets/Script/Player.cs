using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// By @Bullrich

namespace game
{
    [RequireComponent(typeof(PlayerSetup))]
    public class Player : NetworkBehaviour
    {
        [SyncVar] private bool _isDead = false;

        public bool isDead
        {
            get { return _isDead; }
            protected set { _isDead = value; }
        }

        [SerializeField] private int maxHealth = 100;

        [SyncVar] private int currentHealth;

        [SerializeField] private Behaviour[] disableOnDeath;
        private bool[] wasEnabled;

        [SerializeField] private GameObject[] disableGameObjectsOnDeath;

        [SerializeField]
        private GameObject
            deathEffect,
            spawnEffect;

        private bool firstSetup = true;

        public int GetCurrentHealth()
        {
            return currentHealth;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public void SetupPlayer()
        {
            if (isLocalPlayer)
            {
                // Switch cameras
                GameManager.instance.SetSceneCameraActive(false);
                GetComponent<PlayerSetup>().playerUiInstance.SetActive(true);
            }

            CmdBroadCastNewPlayerSetup();
        }

        [Command]
        private void CmdBroadCastNewPlayerSetup()
        {
            RpcSetupPlayerOnAllClients();
        }

        [ClientRpc]
        private void RpcSetupPlayerOnAllClients()
        {
            if (firstSetup)
            {
                wasEnabled = new bool[disableOnDeath.Length];
                for (int i = 0; i < wasEnabled.Length; i++)
                    wasEnabled[i] = disableOnDeath[i].enabled;

                firstSetup = false;
                AchievmentManager.instance.ShowAchievement("start");
            }

            SetDefaults();
        }

        /*
        void Update()
        {
            if (!isLocalPlayer)
                return;

            if (Input.GetKeyDown(KeyCode.K))
            {
                RpcTakeDamage(999);
            }
        }//*/

        public void SetDefaults()
        {
            isDead = false;

            currentHealth = maxHealth;

            // Set components active
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].enabled = wasEnabled[i];
            }
            for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
            {
                disableGameObjectsOnDeath[i].SetActive(true);
            }

            // Enable the collider
            Collider _col = GetComponent<Collider>();
            if (_col != null)
                _col.enabled = true;
            if (isLocalPlayer)
            {
                GameManager.instance.SetSceneCameraActive(false);
                GetComponent<PlayerSetup>().playerUiInstance.SetActive(true);
            }

            // Create spawn effect
            GameObject _spwnEffect = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
            Destroy(_spwnEffect, 3f);
        }

        [ClientRpc]
        public void RpcTakeDamage(int _amount, string _damageNetId)
        {
            if (isDead)
                return;

            currentHealth -= _amount;

            Debug.Log(string.Format("{0} now has {1} health.", transform.name, currentHealth));

            if (currentHealth <= 0)
            {
                Die();
                GameManager.instance.SetScore(_damageNetId);
            }
        }

        private void Die()
        {
            isDead = true;

            // Disable components
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].enabled = false;
            }
            for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
            {
                disableGameObjectsOnDeath[i].SetActive(false);
            }

            AchievmentManager.instance.ShowAchievement("die");

            // Disable the collider
            Collider _col = GetComponent<Collider>();
            if (_col != null)
                _col.enabled = false;

            // Instatiate death effect
            GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(_gfxIns, 3f);

            // Switch cameras
            if (isLocalPlayer)
            {
                GameManager.instance.SetSceneCameraActive(true);
                GetComponent<PlayerSetup>().playerUiInstance.SetActive(false);
            }

            Debug.Log(transform.name + " is DEAD!");

            StartCoroutine(Respawn());
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

            Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
            transform.position = _spawnPoint.position;
            transform.rotation = _spawnPoint.rotation;

            yield return new WaitForSeconds(0.1f);

            SetupPlayer();

            Debug.Log(string.Format("{0} respawned.", transform.name));
        }
    }
}