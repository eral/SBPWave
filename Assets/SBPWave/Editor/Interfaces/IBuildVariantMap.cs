using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave.Interfaces {

	public interface IBuildVariantMap : IContextObject {
		#region Public fields and properties

		/// <summary>
		/// �A�Z�b�g��GUID����i�[����Ă���A�Z�b�g�o���h�����̏Ɖ�
		/// </summary>
		Dictionary<GUID, List<string>> BundleLayoutInverse {get;}

		/// <summary>
		/// InternalFileName����LinkerFileName�̏Ɖ�
		/// </summary>
		/// <remarks>CAB-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</remarks>
		Dictionary<string, string> LinkerFileNames{get;}

		/// <summary>
		/// InternalName����LinkerName�̏Ɖ�
		/// </summary>
		/// <remarks>archive:/CAB-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/CAB-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</remarks>
		Dictionary<string, string> LinkerNames{get;}

		#endregion
	}

}