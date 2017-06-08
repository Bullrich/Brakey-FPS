using UnityEngine;
using UnityEngine.Networking;

// by @Bullrich

namespace game
{
    public class HostGame : MonoBehaviour
    {
        [SerializeField] private uint roomSize = 6;

        private string roomName;

        private NetworkManager networkManager;

        private void Start()
        {
            networkManager = NetworkManager.singleton;
            if (networkManager.matchMaker == null)
                networkManager.StartMatchMaker();
        }

        public void SetRoomName(string _name)
        {
            roomName = _name;
        }

        public void CreateRoom()
        {
            if (!string.IsNullOrEmpty(roomName))
            {
                Debug.Log(string.Format("Creating Room: {0} with room for {1} players.", roomName, roomSize));
                // Create room
                networkManager.matchMaker.CreateMatch(
                    roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
            }
        }
    }
}