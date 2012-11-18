namespace WiGi.Git.Commands
{
	using System.Text;

	public abstract class GitCommand
	{
		/// <summary>
		/// Quotes and Escapes a command-line argument for Git and Bash.
		/// </summary>
		protected string Q(string argument)
		{
			var result = new StringBuilder(argument.Length + 10);
			result.Append("\"");
			for (int i = 0; i < argument.Length; i++)
			{
				var ch = argument[i];
				switch (ch)
				{
					case '\\':
					case '\"':
						result.Append('\\');
						result.Append(ch);
						break;

					default:
						result.Append(ch);
						break;
				}
			}

			result.Append("\"");
			return result.ToString();
		}
	}
}
