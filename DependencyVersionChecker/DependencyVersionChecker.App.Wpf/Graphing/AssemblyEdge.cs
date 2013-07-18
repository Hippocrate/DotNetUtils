﻿using System.Diagnostics;
using DependencyVersionChecker;
using QuickGraph;

namespace DependencyVersionCheckerApp.Wpf.Graphing
{
    [DebuggerDisplay( "{Source.Assembly} -> {Target.Assembly}" )]
    public class AssemblyEdge : Edge<AssemblyVertex>
    {
        public IAssemblyInfo Parent { get; private set; }

        public IAssemblyInfo Child { get; private set; }

        public string Description
        {
            get
            {
                return string.Format( "{0} depends on {1}", Parent.SimpleName, Child.SimpleName );
            }
        }

        public AssemblyEdge( AssemblyVertex source, AssemblyVertex target )
            : base( source, target )
        {
            Parent = target.Assembly;
            Child = source.Assembly;
        }
    }
}