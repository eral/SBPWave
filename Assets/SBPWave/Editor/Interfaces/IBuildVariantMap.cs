using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave.Interfaces {

	public interface IBuildVariantMap : IContextObject {
		#region Public fields and properties

		/// <summary>
		/// アセットのGUIDから格納されているアセットバンドル名の照会
		/// </summary>
		Dictionary<GUID, List<string>> BundleLayoutInverse {get;}

		/// <summary>
		/// InternalFileNameからLinkerFileNameの照会
		/// </summary>
		/// <remarks>CAB-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</remarks>
		Dictionary<string, string> LinkerFileNames{get;}

		/// <summary>
		/// InternalNameからLinkerNameの照会
		/// </summary>
		/// <remarks>archive:/CAB-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/CAB-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</remarks>
		Dictionary<string, string> LinkerNames{get;}

		#endregion
	}

}