using UnityEngine;

namespace eral.SBPWave.Test.Test1 {

	[CreateAssetMenu(menuName="SBPWaveTests/Test1/ScriptableObjectsTop")]
	public class ScriptableObjectsTop : ScriptableObject {
		public ScriptableObjectsIntValue Value => m_Value;

		[SerializeField]
		private ScriptableObjectsIntValue m_Value;
	}

}