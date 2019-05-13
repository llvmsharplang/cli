using System;
using LibGit2Sharp;

namespace IonCLI.PackageManagement
{
    internal class GithubSource : DependencySource
    {
        public override DependencySourceType Type => DependencySourceType.GitHub;

        public override string URL { get; }

        public GithubSource(string repositoryUrl)
        {
            this.URL = repositoryUrl;
        }

        public override bool Fetch()
        {
            throw new NotImplementedException();
            // TODO: Figure out what the *local* path for this should be.
            string localRepoPath = "";
            // TODO: Support HTTP creds (name, password). https://github.com/libgit2/libgit2sharp/wiki/git-clone
            Repository.Clone(this.URL, localRepoPath);
            
            // TODO: Switch to branches for different releases/versions?
            // this.Repo = new Repository(localRepoPath + ".git");
            
        }
    }
}
