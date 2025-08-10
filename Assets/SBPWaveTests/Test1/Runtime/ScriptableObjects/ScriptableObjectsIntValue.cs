using UnityEngine;

namespace eral.SBPWave.Test.Test1 {

	[CreateAssetMenu(menuName="SBPWaveTests/Test1/ScriptableObjectsIntValue")]
	public class ScriptableObjectsIntValue : ScriptableObject {
		public int Value => m_Value;

		[SerializeField]
		private int m_Value;
	}

}