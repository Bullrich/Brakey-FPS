using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using System.Collections;

// by @Bullrich

namespace game
{
    public class JoinGame : MonoBehaviour
    {
        private NetworkManager networkManager;
        private List<GameObject> roomList = new List<GameObject>();
        [SerializeField] private Text status;
        [SerializeField] private GameObject roomListItemPrefab;
        [SerializeField] private Transform roomListParent;

        private void Start()
        {
            networkManager = NetworkManager.singleton;
            if (networkManager.matchMaker == null)
                networkManager.StartMatchMaker();
            RefreshRoomList();
        }

        public void RefreshRoomList()
        {
            ClearRoomList();

            if (networkManager.matchMaker == null)
                networkManager.StartMatchMaker();

            networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
            status.text = "Loading...";
        }

        public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            status.text = "";

            ClearRoomList();

            if (!success || matches == null)
            {
                status.text = "Couldn't get room list";
                return;
            }
            foreach (MatchInfoSnapshot _matchInfo in matches)
            {
                GameObject _roomListItemGo = Instantiate(roomListItemPrefab);
                _roomListItemGo.transform.SetParent(roomListParent);
                RoomListItem _roomListItem = _roomListItemGo.GetComponent<RoomListItem>();
                if (_roomListItem != null)
                    _roomListItem.Setup(_matchInfo, JoinRoom);
                // Have a component sit on the gameobject that will take care of setting up the name/amount of user
                // as well as setting up a callback function that will join the game.

                roomList.Add(_roomListItemGo);
            }
            if (roomList.Count == 0)
                status.text = "No rooms at the moment!";
        }

        private void ClearRoomList()
        {
            for (int i = 0; i < roomList.Count; i++)
                Destroy(roomList[i]);

            roomList.Clear();
        }

        private void JoinRoom(MatchInfoSnapshot _match)
        {
            ClearRoomList();
            networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
            StartCoroutine(WaitForJoin());
        }

        private IEnumerator WaitForJoin()
        {
            ClearRoomList();

            int countdown = 15;
            while (countdown > 0)
            {
                status.text = string.Format("JOINING... ({0})", countdown);
                yield return new WaitForSeconds(1);
                countdown--;
            }

            // Failed to connect
            status.text = "Failed to connect";

            MatchInfo matchInfo = networkManager.matchInfo;
            if (matchInfo != null)
            {
                networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0,
                    networkManager.OnDropConnection);
                networkManager.StopHost();
            }

            yield return new WaitForSeconds(2);

            RefreshRoomList();
        }
    }
}