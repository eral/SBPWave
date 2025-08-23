using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	public class PrefabsValue : MonoBehaviour {
		#region Public fields and properties

		public int Value => m_Value;

		#endregion
		#region Private fields and properties

		[SerializeField]
		private int m_Value;

		#endregion
	}

}