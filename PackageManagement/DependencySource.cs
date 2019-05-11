using System;
using System.IO;
using System.Net;

namespace IonCLI.PackageManagement
{
    public enum DependencySourceType
    {
        /// <summary>
        /// A project hosted on GitHub.
        /// </summary>
        GitHub,

        /// <summary>
        /// A project hosted on a Git URL.
        /// </summary>
        Git,

        /// <summary>
        /// A project hosted locally in the file system.
        /// </summary>
        Local
    }

    public abstract class DependencySource
    {
        public abstract DependencySourceType Type { get; }

        public abstract string URL { get; }

        /// <summary>
        /// Verifies that the remote URL is up and reachable
        /// by sending a web request without downloading any
        /// data.
        /// </summary>
        public virtual bool Verify()
        {
            try
            {
                // Create the web request instance.
                HttpWebRequest request = HttpWebRequest.Create(this.URL) as HttpWebRequest;

                // Specify to only retrieve headers, avoid downloads.
                request.Method = "HEAD";

                // Set the maximum time to wait until timeout occurs.
                request.Timeout = 5000;

                // Invoke the request and retrieve the response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                // Retrieve the response's status code.
                int statusCode = (int)response.StatusCode;

                // Return whether the status code indicates success.
                return statusCode > 100 && statusCode < 400;
            }
            // TODO: What about catching normal Exceptions? Will this render an unhandled exception error?
            catch (WebException)
            {
                // Upon any web exception, return false.
                return false;
            }
        }

        public virtual bool Fetch()
        {
            // Verify remote URL first.
            if (!this.Verify())
            {
                return false;
            }

            // Create a new web request instance.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.URL);

            // Specify decompression strategies.
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            // Invoke the web request and retrieve the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Retrieve the response's stream.
            Stream stream = response.GetResponseStream();

            // Create a new response reader instance.
            StreamReader reader = new StreamReader(stream);

            // Acquire the entire response's content.
            string content = reader.ReadToEnd();

            // TODO: Finish implementing.
            throw new NotImplementedException();
        }
    }
}
