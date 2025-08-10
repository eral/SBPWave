using UnityEngine;

namespace eral.SBPWave.Test.Test1 {

	public class PrefabsValue : MonoBehaviour {
		public int Value => m_Value;

		[SerializeField]
		private int m_Value;
	}

}