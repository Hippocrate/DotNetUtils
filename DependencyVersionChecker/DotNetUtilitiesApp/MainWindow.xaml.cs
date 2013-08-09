﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace DotNetUtilitiesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;
        private GithubDownloader.GithubDownloader _githubDownloader;
        private string _runningSlnPath;

        private readonly DirectoryInfo _appDataDirectory;
        private readonly DirectoryInfo _githubCacheDirectory;

        public MainWindow()
        {
            string tempDownloadPath = Path.Combine( Path.GetTempPath(), Path.GetRandomFileName() );

            _appDataDirectory = new DirectoryInfo( GetAppDataDirectoryPath() );
            if( !_appDataDirectory.Exists )
                _appDataDirectory.Create();

            _githubCacheDirectory = new DirectoryInfo( Path.Combine( _appDataDirectory.FullName, "github-cache" ) );
            if( !_githubCacheDirectory.Exists )
                _githubCacheDirectory.Create();

            // _runningSlnPath is filled in ProcessArgs(), or null'd

            _viewModel = new MainWindowViewModel();
            this.DataContext = _viewModel;

            InitializeComponent();

            _viewModel.SetControls( this.AssemblyProberUserControl, this.SemanticVersionManagerControl, this.SolutionAnalyzerControl, this.VersionAnalyzerControl );

            ProcessArgs();
        }

        private static string GetAppDataDirectoryPath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            return Path.Combine( appDataPath, assemblyName );
        }

        private void ProcessArgs()
        {
            string[] args = Environment.GetCommandLineArgs();

            if( args.Length >= 2 )
            {
                _runningSlnPath = args[1];

                _viewModel.LoadSolutionFile( _runningSlnPath );
                _viewModel.CheckAllCommand.Execute( null );
            }

            if( args.Length >= 3 )
            {
                string command = args[2];

                if( command.ToLowerInvariant() == "-assemblyanalysis" )
                {
                    OpenAssemblyAnalysis();
                }
                else if( command.ToLowerInvariant() == "-packageanalysis" )
                {
                    OpenRunPackageAnalysis();
                }
                else if( command.ToLowerInvariant() == "-versiongenerator" )
                {
                    OpenRunVersionUpdater();
                }
                else if( command.ToLowerInvariant() == "-versionanalysis" )
                {
                    OpenAnalyzeVersion();
                }
            }
            else
            {
                this.TabControl.SelectedIndex = 1;
            }
        }

        public void OpenAssemblyAnalysis()
        {
            this.TabControl.SelectedIndex = 0;
        }

        public void OpenRunPackageAnalysis()
        {
            this.TabControl.SelectedIndex = 1;
        }

        public void OpenRunVersionUpdater()
        {
            this.TabControl.SelectedIndex = 2;
        }

        public void OpenAnalyzeVersion()
        {
            this.TabControl.SelectedIndex = 3;
        }

        private void OpenFromGithubRepo_Click( object sender, RoutedEventArgs e )
        {
            if( _githubDownloader == null )
            {
                _githubDownloader = new GithubDownloader.GithubDownloader( _githubCacheDirectory );
            }

            _githubDownloader.Closing += ( s, e1 ) => { _githubDownloader = null; };
            _githubDownloader.SolutionFileReady += _githubDownloader_SolutionFileReady;

            _githubDownloader.Show();
        }

        void _githubDownloader_SolutionFileReady( object sender, WpfUtils.StringEventArgs e )
        {
            Debug.Assert( File.Exists( e.Content ) );
            _runningSlnPath = e.Content;
            _viewModel.LoadSolutionFile( e.Content );

            _githubDownloader.Close();

            this.TabControl.SelectedIndex = 1;
            _viewModel.CheckAllCommand.Execute( null );
        }
    }
}