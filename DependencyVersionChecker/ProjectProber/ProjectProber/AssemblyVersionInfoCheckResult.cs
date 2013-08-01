﻿using ProjectProber.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectProber
{
	public class AssemblyVersionInfoCheckResult
	{

		/// <summary>
		///
		/// </summary>
		public string SolutionDirectoryPath { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public IReadOnlyDictionary<string, Version> SharedAssemblyInfoVersions { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public IReadOnlyDictionary<ISolutionProjectItem, CSProjCompileLinkInfo> CsProjs { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// </remarks>
		public IReadOnlyDictionary<ISolutionProjectItem, Version> ProjectVersions
		{
			get;
			private set;
		}

		public bool HaveSharedAssemblyInfo
		{
			get { return _haveSharedAssemblyInfo; }
		}
		public bool MultipleSharedAssemblyInfo
		{
			get { return _multipleSharedAssemblyInfo; }
		}
		public bool MultipleSharedAssemblyInfoDifferenteVersion
		{
			get { return _multipleSharedAssemblyInfoDifferenteVersion; }
		}
		public bool MultipleRelativeLinkInCSProj
		{
			get { return _multipleRelativeLinkInCSProj; }
		}
		public bool MultipleVersionInPropretiesAssemblyInfo
		{
			get { return _multipleVersionInPropretiesAssemblyInfo; }
		}

		bool _haveSharedAssemblyInfo = false;
		bool _multipleSharedAssemblyInfo = false;
		bool _multipleSharedAssemblyInfoDifferenteVersion = false;
		bool _multipleRelativeLinkInCSProj = false;
		bool _multipleVersionInPropretiesAssemblyInfo = false;

		internal AssemblyVersionInfoCheckResult( string solutionDirectoryPath,
			IReadOnlyDictionary<string, Version> sharedAssemblyInfoVersions,
			IReadOnlyDictionary<ISolutionProjectItem, CSProjCompileLinkInfo> csProjs,
			IReadOnlyDictionary<ISolutionProjectItem, Version> projectVersions )
		{
			SolutionDirectoryPath = solutionDirectoryPath;
			SharedAssemblyInfoVersions = sharedAssemblyInfoVersions;
			CsProjs = csProjs;
			ProjectVersions = projectVersions;

			if( sharedAssemblyInfoVersions.Count > 1 )
			{
				_haveSharedAssemblyInfo = true;
				_multipleSharedAssemblyInfo = true;
				IList<Version> versionToCompare = new List<Version>();
				foreach( Version version in sharedAssemblyInfoVersions.Values )
				{
					foreach( Version versionCompare in versionToCompare )
					{
						if( versionCompare != version )
						{
							versionToCompare.Add( version );
							_multipleSharedAssemblyInfoDifferenteVersion = true;
							break;
						}
					}
					if( versionToCompare.Count == 0 )
					{
						versionToCompare.Add( version );
					}
				}
			}
			else if( sharedAssemblyInfoVersions.Count == 1 )
			{
				//pas sûr
				_haveSharedAssemblyInfo = true;
				IList<CSProjCompileLinkInfo> csProjCompileLinkInfoToCompare = new List<CSProjCompileLinkInfo>();
				foreach( CSProjCompileLinkInfo csProjCompileLinkInfo in csProjs.Values )
				{
					foreach( CSProjCompileLinkInfo csProjCompileLinkInfoCompare in csProjCompileLinkInfoToCompare )
					{
						if( csProjCompileLinkInfoCompare != csProjCompileLinkInfo )
						{
							csProjCompileLinkInfoToCompare.Add( csProjCompileLinkInfo );
							_multipleRelativeLinkInCSProj = true;
							break;
						}
					}
					if( csProjCompileLinkInfoToCompare.Count == 0 )
					{
						csProjCompileLinkInfoToCompare.Add( csProjCompileLinkInfo );
					}
				}
			}
			else
			{
				IList<Version> versionToCompare = new List<Version>();
				foreach( Version version in projectVersions.Values )
				{
					foreach( Version versionCompare in versionToCompare )
					{
						if( versionCompare != version )
						{
							versionToCompare.Add( version );
							_multipleVersionInPropretiesAssemblyInfo = true;
							break;
						}
					}
					if( versionToCompare.Count == 0 )
					{
						versionToCompare.Add( version );
					}
				}
			}
			
		}
	}
}
