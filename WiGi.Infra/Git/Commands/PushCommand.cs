namespace WiGi.Git.Commands
{
	public class PushCommand : GitCommand
	{
		/// <summary>
		/// Instead of naming each ref to push, specifies that all refs under refs/heads/ be pushed. 
		/// </summary>
		public bool All
		{
			get { return string.IsNullOrEmpty(_all); }
			set { 
				if (value)
				{
					_all = " --all";
					_mirror = "";
					_tags = "";
				}
			}
		}
		private string _all;

		/// <summary>
		/// Instead of naming each ref to push, specifies that all refs under refs/ 
		/// (which includes but is not limited to refs/heads/, refs/remotes/, and refs/tags/) 
		/// be mirrored to the remote repository.
		/// </summary>
		/// <remarks>
		/// Newly created local refs will be pushed to the remote end, locally updated refs will 
		/// be force updated on the remote end, and deleted refs will be removed from the remote end. 
		/// This is the default if the configuration option remote.&lt;remote>.mirror is set. 
		/// </remarks>
		public bool Mirror
		{
			get { return string.IsNullOrEmpty(_mirror); }
			set
			{
				if (value)
				{
					_all = "";
					_mirror = " --mirror";
					_tags = "";
				}
			}
		}
		private string _mirror;

		/// <summary>
		/// All refs under refs/tags are pushed, in addition to refspecs explicitly listed on the command line. 
		/// </summary>
		public bool Tags
		{
			get { return string.IsNullOrEmpty(_tags); }
			set
			{
				if (value)
				{
					_all = "";
					_mirror = "";
					_tags = " --tags";
				}
			}
		}
		private string _tags;

		/// <summary>
		/// Do everything except actually send the updates. 
		/// </summary>
		public bool DryRun
		{
			get { return string.IsNullOrEmpty(_dryRun); }
			set { _dryRun = value ? " --dry-run" : ""; }
		}
		private string _dryRun;

		/// <summary>
		/// Produce machine-readable output. The output status line for each ref will be tab-separated and sent to 
		/// stdout instead of stderr. The full symbolic names of the refs will be given.
		/// </summary>
		public bool Porcelain
		{
			get { return string.IsNullOrEmpty(_porcelain); }
			set { _porcelain = value ? " --procelain" : ""; }
		}
		private string _porcelain;

		/// <summary>
		/// All listed refs are deleted from the remote repository. This is the same as prefixing all refs with a colon. 
		/// </summary>
		public bool Delete
		{
			get { return string.IsNullOrEmpty(_delete); }
			set { _delete = value ? " --delete" : ""; }
		}
		private string _delete;

		/// <summary>
		/// Path to the git-receive-pack program on the remote end. Sometimes useful when pushing to 
		/// a remote repository over ssh, and you do not have the program in a directory on the default $PATH.
		/// </summary>
		public string ReceivePack { get; set; }
		private string _receivePack
		{
			get
			{
				if (string.IsNullOrEmpty(ReceivePack.Trim()))
					return "";

				return "--receive-pack=" + Q(ReceivePack);
			}
		}

		/// <summary>
		/// Usually, the command refuses to update a remote ref that is not an ancestor of the local ref used to overwrite it. 
		/// This flag disables the check. This can cause the remote repository to lose commits; use it with care. 
		/// </summary>
		public bool Force
		{
			get { return string.IsNullOrEmpty(_force); }
			set { _force = value ? " --force" : ""; }
		}
		private string _force;

		/// <summary>
		/// This option is only relevant if no <see cref="Repository"/> argument is passed in the invocation.
		/// It's behavior is modified by <see cref="SetRepoOnlyIfNotTrackingAny"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This option is only relevant if no <see cref="Repository"/> argument is passed in the invocation. In this case, 
		/// git push derives the remote name from the current branch: If it tracks a remote branch, then that 
		/// remote repository is pushed to. Otherwise, the name "origin" is used. For this latter case, this option 
		/// can be used to override the name "origin". In other words, the difference between these two commands
		/// </para>
		/// <code>
		/// git push public         #1
		/// git push --repo=public  #2
		/// </code>
		/// <para>
		/// is that #1 always pushes to "public" whereas #2 pushes to "public" only if the current branch does not track 
		/// a remote branch. This is useful if you write an alias or script around git push.
		/// </para>
		/// <para>
		/// You can choose which one you want to use by setting <see cref="SetRepoOnlyIfNotTrackingAny"/> to <c>true</c>
		/// or <c>false</c>. Default is <c>false</c>.
		/// </para>
		/// </remarks>
		public string Repo { get; set; }
		public bool SetRepoOnlyIfNotTrackingAny { get; set; }
		private string _repo
		{
			get { return (SetRepoOnlyIfNotTrackingAny ? " --repo=" : "") + Repo; }
		}

		/// <summary>
		/// For every branch that is up to date or successfully pushed, add upstream (tracking) reference, used by 
		/// argument-less git-pull(1) and other commands. For more information, see branch.&lt;name>.merge in git-config(1). 
		/// </summary>
		public bool SetUpstream
		{
			get { return string.IsNullOrEmpty(_setUpstream); }
			set { _setUpstream = value ? " --set-upstream" : ""; }
		}
		private string _setUpstream;

		/// <summary>
		/// Alias to <see cref="SetUpstream"/>
		/// </summary>
		public bool U
		{
			get { return SetUpstream; }
			set { SetUpstream = value; }
		}

		/// <summary>
		/// These options are passed to git-send-pack(1). A thin transfer significantly reduces the amount of sent 
		/// data when the sender and receiver share many of the same objects in common. The default is <c>true</c>. 
		/// </summary>
		public bool Thin
		{
			get { return string.IsNullOrEmpty(_thin); }
			set { _thin = value ? "" : " --no-thin"; }
		}
		private string _thin;

		/// <summary>
		/// Suppress all output, including the listing of updated refs, unless an error occurs. Progress is not reported to the standard error stream.
		/// </summary>
		public bool Quiet
		{
			get { return string.IsNullOrEmpty(_quiet); }
			set { _quiet = value ? " --quiet" : ""; }
		}
		private string _quiet;

		/// <summary>
		/// Run verbosely. 
		/// </summary>
		public bool Verbose
		{
			get { return string.IsNullOrEmpty(_verbose); }
			set { _verbose = value ? " --verbose" : ""; }
		}
		private string _verbose;

		/// <summary>
		/// Progress status is reported on the standard error stream by default when it is attached to a terminal, 
		/// unless -q is specified. This flag forces progress status even if the standard error stream is not directed 
		/// to a terminal.
		/// </summary>
		public bool Progress
		{
			get { return string.IsNullOrEmpty(_progress); }
			set { _progress = value ? " --progress" : ""; }
		}
		private string _progress;

		/// <summary>
		/// Check whether all submodule commits used by the revisions to be pushed are available on a remote tracking branch. 
		/// Otherwise the push will be aborted and the command will exit with non-zero status. 
		/// </summary>
		public bool CheckSubmodules
		{
			get { return string.IsNullOrEmpty(_checkSubmodules); }
			set { _checkSubmodules = value ? " --recurse-submodules=check" : ""; }
		}
		private string _checkSubmodules;
		
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

		public PushCommand()
		{
			Thin = true;
		}

		public new string ToString()
		{
			return "push" + _all + _mirror + _tags + _dryRun + _receivePack
						  + _porcelain + _repo + _force + _quiet + _verbose 
						  + _setUpstream + _delete + _progress + _checkSubmodules
						  + Q(Repository) + RefSpec;
		}
	}
}
