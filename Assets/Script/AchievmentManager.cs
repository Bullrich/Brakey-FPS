using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By @Bullrich

namespace game {

	public class AchievmentManager : MonoBehaviour {
		public static AchievmentManager instance;
		[SerializeField]
		public Achievements[] achievements;
		public UnityEngine.UI.Text achievementDetail;
		private PopupTransition transition;

		void Awake () {
			instance = this;
			transition = GetComponent<PopupTransition>();
		}

		public void ShowAchievement(string _keyName){
			foreach (Achievements ach in achievements){
				if(ach.keyName == _keyName)
				{
					if(!ach.obtained){
						achievementDetail.text=ach.achievementText;
						transition.ShowTransition();
						ach.obtained = true;
					}
					break;
				}
			}
		}
	}
	[System.Serializable]
	public class Achievements {
		public string keyName, achievementText;
		[HideInInspector]
		public bool obtained;
	}
}