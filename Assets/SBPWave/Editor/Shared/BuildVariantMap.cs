using eral.SBPWave.Interfaces;
using System.Collections.Generic;
using UnityEditor;

namespace eral.SBPWave {

	public class BuildVariantMap : IBuildVariantMap {
		/// <summary>
		/// �A�Z�b�g��GUID����i�[����Ă���A�Z�b�g�o���h�����̏Ɖ�
		/// </summary>
		public Dictionary<GUID, List<string>> BundleLayoutInverse {get;set;} = new Dictionary<GUID, List<string>>();

		/// <summary>
		/// InternalFileName����LinkerFileName�̏Ɖ�
		/// </summary>
		/// <remarks>CAB-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</remarks>
		public Dictionary<string, string> LinkerFileNames {get;set;} = new Dictionary<string, string>();

		/// <summary>
		/// InternalName����LinkerName�̏Ɖ�
		/// </summary>
		/// <remarks>archive:/CAB-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/CAB-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</remarks>
		public Dictionary<string, string> LinkerNames {get;set;} = new Dictionary<string, string>();
	}

}