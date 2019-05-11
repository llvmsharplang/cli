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
    }
}
