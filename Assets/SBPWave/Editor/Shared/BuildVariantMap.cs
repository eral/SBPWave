using eral.SBPWave.Interfaces;
using System.Collections.Generic;
using UnityEditor;

namespace eral.SBPWave {

	public class BuildVariantMap : IBuildVariantMap {
		/// <summary>
		/// アセットのGUIDから格納されているアセットバンドル名の照会
		/// </summary>
		public Dictionary<GUID, List<string>> BundleLayoutInverse {get;set;} = new Dictionary<GUID, List<string>>();

		/// <summary>
		/// InternalFileNameからLinkerFileNameの照会
		/// </summary>
		/// <remarks>CAB-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</remarks>
		public Dictionary<string, string> LinkerFileNames {get;set;} = new Dictionary<string, string>();

		/// <summary>
		/// InternalNameからLinkerNameの照会
		/// </summary>
		/// <remarks>archive:/CAB-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/CAB-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</remarks>
		public Dictionary<string, string> LinkerNames {get;set;} = new Dictionary<string, string>();
	}

}