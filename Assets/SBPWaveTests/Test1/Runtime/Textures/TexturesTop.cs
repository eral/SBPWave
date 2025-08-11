using UnityEngine;

namespace eral.SBPWave.Test.Test1 {

	[CreateAssetMenu(menuName="SBPWaveTests/Test1/TexturesTop")]
	public class TexturesTop : ScriptableObject {
		public Texture2D Value => m_Value;

		[SerializeField]
		private Texture2D m_Value;
	}

}