﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectProber.Interfaces;

namespace ProjectProber.Impl
{
    class SolutionProjectItem : ISolutionProjectItem
    {
        #region ISolutionProjectItem Members

        public Guid ProjectTypeGuid { get; private set; }

        public Guid ProjectGuid { get; private set; }

        public string ProjectName { get; private set; }

        public string ProjectPath { get; private set; }

        #endregion

        internal SolutionProjectItem( Guid projectTypeGuid, Guid projectGuid, string projectName, string projectPath )
        {
            ProjectGuid = projectGuid;
            ProjectName = projectName;
            ProjectTypeGuid = projectTypeGuid;
            ProjectPath = projectPath;
        }
    }
}
