using UnityEngine;

// by @Bullrich

namespace game
{
	public class Util {
		public static void SetLayerRecursively(GameObject _obj, int _newLayer)
		{
			if (_obj == null)
			{
				Debug.LogWarning("Given a null GameObject");
				return;
			}

			_obj.layer = _newLayer;

			foreach (Transform _child in _obj.transform)
			{
				if(_child == null)
					continue;
				SetLayerRecursively(_child.gameObject, _newLayer);
			}
		}
	}
}
