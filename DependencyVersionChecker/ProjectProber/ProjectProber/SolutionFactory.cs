﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProjectProber.Impl;
using ProjectProber.Interfaces;

namespace ProjectProber
{
    public static class SolutionFactory
    {
        /// <summary>
        /// Regex pattern. In match order: Project type Guid, Name, Path, and project Guid.
        /// </summary>
        /// <example>
        /// Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "BCrosnier.Utils.Net", "BCrosnier.Utils.Net\BCrosnier.Utils.Net.csproj", "{B0435029-68B7-439A-8896-8D41BC963551}"
        /// </example>
        /// <remarks>
        /// Non-literal: ^Project\(\"([^"]*)\"\) = \"([^"]*)\", \"([^"]*)\", \"([^"]*)\"$
        /// </remarks>
        private static readonly string SOLUTION_PROJECT_PATTERN = @"^Project\(\""([^""]*)\""\) = \""([^""]*)\"", \""([^""]*)\"", \""([^""]*)\""$";

        public static ISolution ReadFromSolutionFile( string filePath )
        {
            if( String.IsNullOrEmpty( filePath ) )
                throw new ArgumentNullException( "filePath" );
            if( !File.Exists( filePath ) )
                throw new ArgumentException( "File must exist", "filePath" );

            List<ISolutionProjectItem> projectItems = ParseItemsFromSolutionFile( filePath );

            Solution solution = new Solution() { ProjectItems = projectItems, DirectoryPath = Path.GetDirectoryName(filePath) };

            return solution;
        }

        public static IEnumerable<ISolution> ReadSolutionsFromDirectory( string directoryPath )
        {
            if( String.IsNullOrEmpty( directoryPath ) )
                throw new ArgumentNullException( "directoryPath" );
            if( !Directory.Exists( directoryPath ) )
                throw new ArgumentException( "Directory must exist", "directoryPath" );

            List<ISolution> solutions = new List<ISolution>();

            DirectoryInfo dir = new DirectoryInfo( directoryPath );

            IEnumerable<FileInfo> solutionFiles = dir.GetFiles( "*.sln", SearchOption.TopDirectoryOnly );

            foreach( FileInfo solutionFile in solutionFiles )
            {
                ISolution s = ReadFromSolutionFile( solutionFile.FullName );
                solutions.Add( s );
            }

            return solutions;
        }

        private static List<ISolutionProjectItem> ParseItemsFromSolutionFile( string filePath )
        {
            List<ISolutionProjectItem> projectItems = new List<ISolutionProjectItem>();

            StreamReader reader = File.OpenText( filePath );

            while( !reader.EndOfStream )
            {
                string line = reader.ReadLine();
                Match m = Regex.Match( line, SOLUTION_PROJECT_PATTERN );
                if( m.Success )
                {
                    Guid projectTypeGuid = Guid.Parse( m.Groups[1].Value );
                    string projectName = m.Groups[2].Value;
                    string projectPath = m.Groups[3].Value;
                    Guid projectGuid = Guid.Parse( m.Groups[4].Value );

                    SolutionProjectItem item = new SolutionProjectItem( projectTypeGuid, projectGuid, projectName, projectPath );

                    projectItems.Add( item );
                }
            }

            return projectItems;
        }
    }
}
