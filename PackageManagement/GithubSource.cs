using System;
using LibGit2Sharp;

namespace IonCLI.PackageManagement
{
    internal class GithubSource : DependencySource
    {
        public override DependencySourceType Type => DependencySourceType.GitHub;

        public override string URL { get; }
        private Repository repo { }

        public GithubSource(string repositoryUrl)
        {
            this.URL = repositoryUrl;
            this.repo = new Repository(repositoryUrl);
        }

        public override bool Fetch()
        {
            // TODO: Implement.
            throw new NotImplementedException();
        }
    }
}
