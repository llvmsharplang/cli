namespace IonCLI.PackageManagement
{
    internal class GithubSource : DependencySource
    {
        public GithubSource(string repositoryUrl)
        {
            this.URL = repositoryUrl;
        }
    }
}
