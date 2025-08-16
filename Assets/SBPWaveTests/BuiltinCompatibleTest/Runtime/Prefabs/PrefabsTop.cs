using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	public class PrefabsTop : MonoBehaviour {
		public GameObject Value => m_Value;

		[SerializeField]
		private GameObject m_Value;
	}

}