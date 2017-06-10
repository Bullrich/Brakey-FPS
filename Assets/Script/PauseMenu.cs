using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

// by @Bullrich

namespace game
{
	public class PauseMenu : MonoBehaviour
	{
		public static bool IsPaused = false;

		public void LeaveRoom()
		{
			NetworkManager networkManager = NetworkManager.singleton;
			MatchInfo matchInfo = networkManager.matchInfo;
			networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
			networkManager.StopHost();
		}
	}
}
