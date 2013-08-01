﻿using System;
using System.Windows;

namespace DotNetUtilitiesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _runningSlnPath;

        public MainWindow()
        {
            InitializeComponent();
            ProcessArgs();
        }

        private void ProcessArgs()
        {
            string[] args = Environment.GetCommandLineArgs();

            if( args.Length >= 3 )
            {
                string command = args[1];
                string path = args[2];

                if( command.ToLowerInvariant() == "-analyzeassemblyfolder" )
                {
                    AnalyzeAssemblyFolder( path );
                }
                else if( command.ToLowerInvariant() == "-analyzesolution" )
                {
                    AnalyzeSolution( path );

                    _runningSlnPath = path;
                }
                else if( command.ToLowerInvariant() == "-analyzesolutionversion" )
                {
                    PrepareSemanticVersion( path );

                    _runningSlnPath = path;
                }
            }
        }

        public void AnalyzeAssemblyFolder( string folderPath )
        {
            this.AssemblyProberUserControl.LoadFolder( folderPath );
            this.TabControl.SelectedIndex = 0;
        }

        public void AnalyzeSolution( string slnPath )
        {
            this.SolutionAnalyzerControl.LoadSolutionFile( slnPath );
            this.TabControl.SelectedIndex = 1;
        }

        public void PrepareSemanticVersion( string slnPath )
        {
            this.SemanticVersionManagerControl.LoadFromSolution( slnPath );
            this.TabControl.SelectedIndex = 2;
        }
    }
}