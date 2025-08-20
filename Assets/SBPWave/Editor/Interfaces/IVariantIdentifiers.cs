using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave.Interfaces {

	public interface IVariantIdentifiers : IDeterministicIdentifiers {
		#region Public methods

		string GenerateLinkerFileName(string name);

		#endregion
	}

}