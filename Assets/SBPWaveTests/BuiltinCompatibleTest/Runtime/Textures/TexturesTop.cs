using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	[CreateAssetMenu(menuName="SBPWaveTests/BuiltinCompatibleTest/TexturesTop")]
	public class TexturesTop : ScriptableObject {
		#region Public fields and properties

		public Texture2D Value => m_Value;

		#endregion
		#region Private fields and properties

		[SerializeField]
		private Texture2D m_Value;

		#endregion
	}

}