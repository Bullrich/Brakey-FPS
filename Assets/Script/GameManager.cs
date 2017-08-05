using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By @Bullrich

namespace game
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public MatchSettings matchSettings;

        public static Dictionary<string, int> playerScore = new Dictionary<string, int>();

        [SerializeField] private GameObject sceneCamera;

        private string winnerName;

        void Awake()
        {
            if (instance != null)
                Debug.LogError("There is more than one game manager in the scene!");
            else
                instance = this;
                winnerName = null;
        }

        public void SetSceneCameraActive(bool isActive)
        {
            sceneCamera.SetActive(isActive);
        }

        public void SetScore(string _netID)
        {
            string _playerID = PLAYER_ID_PREFIX + _netID;
            int newScore = playerScore[_playerID] + 1;
            playerScore[_playerID] = newScore;
            if (newScore >= 10)
            {
                winnerName = _playerID;
            }
        }

        public string GetWinner(){
            return winnerName;
        }

        public Dictionary<string, int> GetScores()
        {
            return playerScore;
        }

        #region Player tracking
        private const string PLAYER_ID_PREFIX = "Player ";

        private static Dictionary<string, Player> players = new Dictionary<string, Player>();

        public static void RegisterPlayer(string _netID, Player _player)
        {
            string _playerID = PLAYER_ID_PREFIX + _netID;
            players.Add(_playerID, _player);
            _player.transform.name = _playerID;
            playerScore.Add(_playerID, 0);
        }

        public static void UnRegisterPlayer(string _playerID)
        {
            if (players.ContainsKey(_playerID))
                players.Remove(_playerID);
            playerScore.Remove(_playerID);
        }

        public static Player GetPlayer(string _playerID)
        {
            return players[_playerID];
        }

        // void OnGUI()
        // {
        //     GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        //     GUILayout.BeginVertical();
        //     GUILayout.Label("Player Score");
        //     foreach (string _playerID in players.Keys)
        //     {
        //         GUILayout.Label(_playerID + "  -  " + playerScore[_playerID]);
        //     }

        //     GUILayout.EndVertical();
        //     GUILayout.EndArea();
        // }

        #endregion
    }
}