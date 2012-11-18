namespace WiGi.Git.Commands
{
	public enum RecurseSubmoduleOption
	{
		Yes,
		No,
		OnDemand
	}

	public class PullCommand : GitCommand
	{
		/// <summary>
		/// This is passed to both underlying git-fetch to squelch reporting of during transfer, and underlying git-merge to squelch output during merging. 
		/// </summary>
		public bool Quiet
		{
			get { return string.IsNullOrEmpty(_quiet); }
			set { _quiet = value ? " --quiet" : ""; }
		}
		private string _quiet;

		/// <summary>
		/// Pass --verbose to git-fetch and git-merge.
		/// </summary>
		public bool Verbose
		{
			get { return string.IsNullOrEmpty(_verbose); }
			set { _verbose = value ? " --verbose" : ""; }
		}
		private string _verbose;

		public RecurseSubmoduleOption RecurseSubmodules { get; set; }
		private string _recurseSubmodule
		{
			get
			{
				switch (RecurseSubmodules)
				{
					case RecurseSubmoduleOption.Yes:
						return " --recurse-submodules=yes";
					case RecurseSubmoduleOption.No:
						return " --recurse-submodules=no";
					case RecurseSubmoduleOption.OnDemand:
						return " --recurse-submodules=on-demand";
				}
				return "";
			}
		}

		/// <summary>
		/// The "remote" repository that is the source of a fetch or pull operation.
		/// </summary>
		/// <remarks>
		/// This parameter can be either a URL (see the section GIT URLS below) or the name of a remote (see the section REMOTES 
		/// below).
		/// </remarks>
		public string Repository { get; set; }

		/// <summary>
		/// The format of a &lt;refspec> parameter is an optional plus +, followed by the source ref &lt;src>, followed by a 
		/// colon :, followed by the destination ref &lt;dst>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The remote ref that matches &lt;src> is fetched, and if &lt;dst> is not empty string, the local ref that matches it 
		/// is fast-forwarded using &lt;src>. If the optional plus + is used, the local ref is updated even if it does not result 
		/// in a fast-forward update.
		/// </para>
		/// <para>
		/// <h3>NOTE</h3>
		/// If the remote branch from which you want to pull is modified in non-linear ways such as being rewound and rebased 
		/// frequently, then a pull will attempt a merge with an older version of itself, likely conflict, and fail. It is under 
		/// these conditions that you would want to use the + sign to indicate non-fast-forward updates will be needed. There 
		/// is currently no easy way to determine or declare that a branch will be made available in a repository with this 
		/// behavior; the pulling user simply must know this is the expected usage pattern for a branch.
		/// </para>
		/// <para>
		/// <h3>NOTE</h3>
		/// You never do your own development on branches that appear on the right hand side of a &lt;refspec> colon on Pull: 
		/// lines; they are to be updated by git fetch. If you intend to do development derived from a remote branch B, have 
		/// a Pull: line to track it (i.e. Pull: B:remote-B), and have a separate branch my-B to do your development on top 
		/// of it. The latter is created by git branch my-B remote-B (or its equivalent git checkout -b my-B remote-B). Run git 
		/// fetch to keep track of the progress of the remote side, and when you see something new on the remote branch, merge 
		/// it into your development branch with git pull . remote-B, while you are on my-B branch.
		/// </para>
		/// <para>
		/// <h3>NOTE</h3>
		/// There is a difference between listing multiple &lt;refspec> directly on git pull command line and having multiple 
		/// Pull: &lt;refspec> lines for a &lt;repository> and running git pull command without any explicit &lt;refspec> 
		/// parameters. &lt;refspec> listed explicitly on the command line are always merged into the current branch after 
		/// fetching. In other words, if you list more than one remote refs, you would be making an Octopus. While git pull 
		/// run without any explicit &lt;refspec> parameter takes default &lt;refspec>s from Pull: lines, it merges only the 
		/// first &lt;refspec> found into the current branch, after fetching all the remote refs. This is because making an 
		/// Octopus from remote refs is rarely done, while keeping track of multiple remote heads in one-go by fetching more 
		/// than one is often useful.
		/// </para>
		/// <para>
		/// Some short-cut notations are also supported.
		/// <list type="bullet">
		///   <item>
		///     <description>
		///       tag &lt;tag> means the same as refs/tags/&lt;tag>:refs/tags/&lt;tag>; it requests fetching everything up to 
		///       the given tag.
		///     </description>
		///   </item>
		///   <item>
		///     <description>
		///       A parameter &lt;ref> without a colon is equivalent to &lt;ref>: when pulling/fetching, so it merges &lt;ref> 
		///       into the current branch without storing the remote branch anywhere locally
		///     </description>
		///   </item>
		/// </list>
		/// </para>
		/// </remarks>
		public string RefSpec { get; set; }

		public new string ToString()
		{
			// Options meant for git pull itself and the underlying git merge must be given
			// BEFORE the options meant for git fetch.
			return "pull" + _quiet + _verbose + _recurseSubmodule + Q(Repository) + RefSpec;
		}
	}
}
