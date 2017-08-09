using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By @Bullrich

namespace game
{

    public class PopupTransition : MonoBehaviour
    {
        private RectTransform rctTrans;
        public float
            standByTime = 1f,
            speedTransition = 0.5f;
        public bool debug;

        private void Start()
        {
            rctTrans = GetComponent<RectTransform>();
            ChangeScale(0);
            if (debug)
                ShowTransition();
        }

        private void ChangeScale(float _y)
        {
            rctTrans.localScale = new Vector3(1f, _y, 1f);
        }

		public void ShowTransition(){
			StartCoroutine(Transition());
		}

        IEnumerator Transition()
        {
            float time = Time.time;
            while (rctTrans.localScale.y < 1)
            {
				yield return new WaitForSeconds(0.1f);
                ChangeScale(rctTrans.localScale.y + (1 / speedTransition)/4);
            }
            yield return new WaitForSeconds(standByTime);
            while (rctTrans.localScale.y > 0)
            {
				yield return new WaitForSeconds(0.1f);
                ChangeScale(rctTrans.localScale.y - (1 / speedTransition)/4);
            }
        }
    }
}