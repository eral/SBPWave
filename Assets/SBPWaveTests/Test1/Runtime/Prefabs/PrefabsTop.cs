using UnityEngine;

namespace eral.SBPWave.Test.Test1 {

	public class PrefabsTop : MonoBehaviour {
		public GameObject Value => m_Value;

		[SerializeField]
		private GameObject m_Value;
	}

}