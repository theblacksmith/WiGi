namespace WiGi.Git.Commands
{
	/// <summary>
	/// Represents the clone command (git-clone)
	/// </summary>
	/// <remarks>
	/// git clone 
	///		[--template=<template_directory>] [-l] [-s] [--no-hardlinks] [-q] [-n] 
	///		[--bare] [--mirror] [-o <name>] [-b <name>] [-u <upload-pack>] 
	///		[--reference <repository>] [--separate-git-dir <git dir>] [--depth <depth>] 
	///		[--recursive|--recurse-submodules] [--] <repository> [<directory>]
	/// </remarks>
	public class CloneCommand : GitCommand
	{
		public string Url { get; set; }
		public string Directory { get; set; }

		/// <summary>
		/// Specify the directory from which templates will be used; (See the "TEMPLATE DIRECTORY" section of git-init(1).)
		/// </summary>
		public string Template { get; set; }
		private string _template
		{
			get { return string.IsNullOrEmpty(Template) ? "" : " --template=" + Template; }
		}

		/// <summary>
		/// When the repository to clone from is on a local machine, this flag bypasses the normal "git aware" 
		/// transport mechanism.
		/// </summary>
		/// <remarks>
		/// When the repository to clone from is on a local machine, this flag bypasses the normal "git aware" 
		/// transport mechanism and clones the repository by making a copy of HEAD and everything under 
		/// objects and refs directories. The files under .git/objects/ directory are hardlinked to save 
		/// space when possible.
		/// This is now the default when the source repository is specified with /path/to/repo syntax, 
		/// so it essentially is a no-op option. To force copying instead of hardlinking 
		/// (which may be desirable if you are trying to make a back-up of your repository), 
		/// but still avoid the usual "git aware" transport mechanism, --no-hardlinks can be used. 
		/// </remarks>
		public bool Local
		{
			get { return string.IsNullOrEmpty(_local); }
			set { _local = value ? " --local" : ""; }
		}
		private string _local;

		/// <summary>
		/// When the repository to clone is on the local machine, instead of using hard links, 
		/// automatically setup .git/objects/info/alternates to share the objects with the source repository.
		/// </summary>
		/// <remarks>
		/// When the repository to clone is on the local machine, instead of using hard links, 
		/// automatically setup .git/objects/info/alternates to share the objects with the source repository.
		/// The resulting repository starts out without any object of its own.
		/// NOTE: this is a possibly dangerous operation; do not use it unless you understand what it does. If you clone your repository using this option and then delete branches (or use any other git command that makes any existing commit unreferenced) in the source repository, some objects may become unreferenced (or dangling). These objects may be removed by normal git operations (such as git commit) which automatically call git gc --auto. (See git-gc(1).) If these objects are removed and were referenced by the cloned repository, then the cloned repository will become corrupt.
		/// Note that running git repack without the -l option in a repository cloned with -s will copy objects from the source repository into a pack in the cloned repository, removing the disk space savings of clone -s. It is safe, however, to run git gc, which uses the -l option by default.
		/// If you want to break the dependency of a repository cloned with -s on its source repository, you can simply run git repack -a to copy all objects from the source repository into a pack in the cloned repository.
		/// </remarks>
		public bool Shared
		{
			get { return string.IsNullOrEmpty(_shared); }
			set { _shared = value ? " --shared" : ""; }
		}
		private string _shared;

		/// <summary>
		/// Optimize the cloning process from a repository on a local filesystem by copying files under .git/objects directory. 
		/// </summary>
		public bool NoHardlinks
		{
			get { return string.IsNullOrEmpty(_noHardlinks); }
			set { _noHardlinks = value ? " --no-hardlinks" : ""; }
		}
		private string _noHardlinks;

		/// <summary>
		/// Operate quietly. Progress is not reported to the standard error stream. This flag is also passed to the ‘rsync’ command when given. 
		/// </summary>
		public bool Quiet
		{
			get { return string.IsNullOrEmpty(_quiet); }
			set { _quiet = value ? " --quiet" : ""; }
		}
		private string _quiet;

		/// <summary>
		/// Run verbosely. Does not affect the reporting of progress status to the standard error stream. 
		/// </summary>
		public bool Verbose
		{
			get { return string.IsNullOrEmpty(_verbose); }
			set { _verbose = value ? " --verbose" : ""; }
		}
		private string _verbose;

		/// <summary>
		/// If set to false, No checkout of HEAD is performed after the clone is complete.
		/// Default: true
		/// </summary>
		public bool NoCheckout
		{
			get { return string.IsNullOrEmpty(_noCheckout); }
			set { _noCheckout = value ? "" : " --no-checkout"; }
		}
		private string _noCheckout;

		/// <summary>
		/// Make a bare GIT repository. 
		/// </summary>
		/// <remarks>
		/// Make a bare GIT repository. 
		/// That is, instead of creating <directory> and placing the administrative files in <directory>/.git, 
		/// make the <directory> itself the $GIT_DIR. This obviously implies the -n because there is nowhere 
		/// to check out the working tree. Also the branch heads at the remote are copied directly to corresponding 
		/// local branch heads, without mapping them to refs/remotes/origin/. When this option is used, neither 
		/// remote-tracking branches nor the related configuration variables are created.
		/// </remarks>
		public bool Bare
		{
			get { return string.IsNullOrEmpty(_bare); }
			set { _bare = value ? " --bare" : ""; }
		}
		private string _bare;

		/// <summary>
		/// Set up a mirror of the source repository. 
		/// </summary>
		/// <remarks>
		/// Set up a mirror of the source repository.
		/// This implies --bare. Compared to --bare, --mirror not only maps local branches of the source 
		/// to local branches of the target, it maps all refs (including remote-tracking branches, notes etc.) 
		/// and sets up a refspec configuration such that all these refs are overwritten by a git remote update 
		/// in the target repository.
		/// </remarks>
		public bool Mirror
		{
			get { return string.IsNullOrEmpty(_mirror); }
			set { _mirror = value ? " --mirror" : ""; }
		}
		private string _mirror;

		/// <summary>
		/// Instead of using the remote name origin to keep track of the upstream repository, use <name>. 
		/// </summary>
		public string Origin { get; set; }
		private string _origin
		{
			get { return string.IsNullOrEmpty(Origin) ? "" : " --origin " + Q(Origin); }
		}

		/// <summary>
		/// Instead of using the remote name origin to keep track of the upstream repository, use <name>. 
		/// </summary>
		public string Branch { get; set; }
		private string _branch
		{
			get { return string.IsNullOrEmpty(Branch) ? "" : " --branch " + Q(Branch); }
		}

		/// <summary>
		/// When given, and the repository to clone from is accessed via ssh, this specifies a non-default path 
		/// for the command run on the other end. 
		/// </summary>
		public string UploadPack { get; set; }
		private string _uploadPack
		{
			get { return string.IsNullOrEmpty(UploadPack) ? "" : " --upload-pack " + Q(UploadPack); }
		}

		/// <summary>
		/// If the reference repository is on the local machine, automatically setup .git/objects/info/alternates 
		/// to obtain objects from the reference repository. Using an already existing repository as an alternate 
		/// will require fewer objects to be copied from the repository being cloned, reducing network and local storage costs.
		/// NOTE: see the NOTE for the --shared option.
		/// </summary>
		public string Reference { get; set; }
		private string _reference
		{
			get { return string.IsNullOrEmpty(Reference) ? "" : " --reference " + Q(Reference); }
		}

		/// <summary>
		/// Instead of placing the cloned repository where it is supposed to be, place the cloned 
		/// repository at the specified directory, then make a filesytem-agnostic git symbolic link to there. 
		/// The result is git repository can be separated from working tree.
		/// </summary>
		public string SeparateGitDir { get; set; }
		private string _separateGitDir
		{
			get { return string.IsNullOrEmpty(SeparateGitDir) ? "" : " --separate-git-dir " + Q(SeparateGitDir); }
		}

		/// <summary>
		/// Create a shallow clone with a history truncated to the specified number of revisions.
		/// </summary>
		/// <remarks>
		/// A shallow repository has a number of limitations (you cannot clone or fetch from it, 
		/// nor push from nor into it), but is adequate if you are only interested in the recent 
		/// history of a large project with a long history, and would want to send in fixes as patches.
		/// </remarks>
		public string Depth { get; set; }
		private string _depth
		{
			get { return string.IsNullOrEmpty(Depth) ? "" : " --depth " + Q(Depth); }
		}

		/// <summary>
		/// After the clone is created, initialize all submodules within, using their default settings.
		/// </summary>
		/// <remarks>
		/// After the clone is created, initialize all submodules within, using their default settings.
		/// This is equivalent to running git submodule update --init --recursive immediately after the clone is finished. This option is ignored if the cloned repository does not have a worktree/checkout (i.e. if any of --no-checkout/-n, --bare, or --mirror is given).
		/// </remarks>
		public bool Recursive
		{
			get { return string.IsNullOrEmpty(_recursive); }
			set { _recursive = value ? " --recursive" : ""; }
		}
		private string _recursive;

		/// <summary>
		/// An alias to <see cref="Recursive"/>
		/// </summary>
		public bool RecurseSubmodules
		{
			get { return Recursive; }
			set { Recursive = value; }
		}

		public CloneCommand(string url)
		{
			Url = url;
		}

		public CloneCommand(string url, string dir)
		{
			Url = url;
			Directory = dir;
		}

		public new string ToString()
		{
			return "clone"
				+ _template + _local + _shared + _noHardlinks + _quiet + _noCheckout 
				+ _bare + _mirror + _origin + _branch + _uploadPack
				+ _reference  + _separateGitDir + _depth  
				+ _recursive + " " + Url + " " + Directory;
		}
	}
}
