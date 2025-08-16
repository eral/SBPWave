using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	public class PrefabsValue : MonoBehaviour {
		public int Value => m_Value;

		[SerializeField]
		private int m_Value;
	}

}