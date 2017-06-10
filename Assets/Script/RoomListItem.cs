using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

// by @Bullrich

namespace game
{
	public class RoomListItem : MonoBehaviour
	{
		public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);

		private JoinRoomDelegate joinRoomCallback;
		
		[SerializeField] private Text roomNameText;
		private MatchInfoSnapshot match;

		public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback)
		{
			match = _match;

			roomNameText.text = match.name + string.Format(" ({0}/{1})", match.currentSize, match.maxSize);

			joinRoomCallback = _joinRoomCallback;
		}

		public void JoinRoom()
		{
			joinRoomCallback.Invoke(match);
		}

	}
}
