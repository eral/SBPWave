using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	public class PrefabsTop : MonoBehaviour {
		#region Public fields and properties

		public GameObject Value => m_Value;

		#endregion
		#region Private fields and properties

		[SerializeField]
		private GameObject m_Value;

		#endregion
	}

}