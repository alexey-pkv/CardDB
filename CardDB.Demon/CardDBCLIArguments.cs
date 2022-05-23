using NClap.Metadata;


namespace CardDB.Demon
{
	[ArgumentSet(PublicMembersAreNamedArguments = true)]
	public class CardDBCLIArguments
	{
		[NamedArgument(ArgumentFlags.Optional,
				LongName = "host",
                Description = "The host to listen on",
                DefaultValue = "localhost")]
		public string Host { get; set; }
		
		[NamedArgument(ArgumentFlags.Optional,
                LongName = "port",
                ShortName = "p",
                Description = "The port to listen on.",
                DefaultValue = 7373)]
		public int Port { get; set; }
		
		[NamedArgument(ArgumentFlags.Optional,
                LongName = "help",
                ShortName = "h",
                Description = "Show this output.",
                DefaultValue = false)]
		public bool ShowHelp { get; set; }
		
		[NamedArgument(ArgumentFlags.Optional,
                LongName = "conf-dir",
                Description = "Path to the root config dir",
                DefaultValue = "/opt/cdb")]
		public string ConfigDir { get; set; }
		
		[NamedArgument(ArgumentFlags.Optional,
                LongName = "log-dir",
                Description = "Path to the logs dir",
                DefaultValue = "/var/log/cdb")]
		public string LogDir { get; set; }
		
		[NamedArgument(ArgumentFlags.Optional,
                LongName = "log-to-console",
                Description = "If set, output logs both to the log file and stdout",
                DefaultValue = true)]
		public bool LogToConsole { get; set; }
	}
}