﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Package;

namespace ProjectProber
{
	public static class SemanticVersionGenerator
	{
		//don't support metadata in semantic versioning
		public static SemanticVersion GenerateSemanticVersion( SemanticVersion version,
			bool publicBreakingChange,
			bool deprecatedOrNewFunction,
			bool bugFixe,
			string preRelease )
		{
			if( publicBreakingChange ) return new SemanticVersion( version.Version.Major + 1, 0, 0, preRelease );
			if( deprecatedOrNewFunction ) return new SemanticVersion( version.Version.Major, version.Version.Minor + 1, 0, preRelease );
			if( bugFixe ) return new SemanticVersion( version.Version.Major, version.Version.Minor, version.Version.Build + 1, preRelease );
			return new SemanticVersion( new Version( 0, 0, 0) );
		}

	}
}
