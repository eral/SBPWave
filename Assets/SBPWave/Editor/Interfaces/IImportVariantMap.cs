using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave.Interfaces {

	public interface IImportVariantMap : IContextObject {
		#region Public fields and properties

		IBundleBuildContent BundleBuildContent {set;}
		IBuildVariantMap BuildVariantMap {set;}

		#endregion
	}

}