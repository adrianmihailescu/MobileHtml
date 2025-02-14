using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Configuration.Install;
using KMobile.BasicTools;

namespace xhtml.Tools
{
	[RunInstaller(true)]
	public class Log : Installer, IKEventLogSettings
	{
		internal static Log Instance = new Log( false );
		
		private EventLog _eventLog;
		private TraceSwitch _eventLogSwitch;
		
		#region Properties

		public EventLog EventLog
		{
			get{ return this._eventLog; }
		}

		public TraceSwitch EventLogSwitch
		{
			get{ return this._eventLogSwitch; }
		}

		#endregion Properties

		#region Constructor

		public Log() : this( true ) {}

		private Log( bool addInstallers )
		{
			// Les Log & Sources ne peuvent pas être placés dans un fichier de config 
			// car la config n'est pas chargée au moment de l'installation avec l'outil
			// installutil.exe
			this._eventLog = new EventLog( "Application", ".", "KMobile.Wap" );
			
			// Le paramètre displayName doit correspondre à au nom d'un switch dans le fichier 
			// de configuration, sinon le switch est absent et les logs pas tracés.
			this._eventLogSwitch = new TraceSwitch( "EventLogSwitch", "Switch Description" );

			if( addInstallers )
			{
				EventLogInstaller eventLogInstaller = new EventLogInstaller();
				eventLogInstaller.CopyFromComponent( this._eventLog );
				eventLogInstaller.UninstallAction =	UninstallAction.Remove;
				this.Installers.Add( eventLogInstaller );
			}
		}

		#endregion Constructor

		internal static void LogInformation( string msg )
		{
			KMobile.BasicTools.Trace.LogInfoEvent( Instance, msg );
		}

		internal static void LogError( string msg, Exception exc )
		{
			KMobile.BasicTools.Trace.LogErrorEvent( Instance, msg, exc );
		}

		internal static void LogWarning( string msg )
		{
			KMobile.BasicTools.Trace.LogWarningEvent( Instance, msg );
		}

		internal static void LogWarning( string msg, Exception exc )
		{
			KMobile.BasicTools.Trace.LogWarningEvent( Instance, msg, exc );
		}
	}
}

