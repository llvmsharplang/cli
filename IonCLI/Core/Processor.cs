using Ion.SyntaxAnalysis;
using Ion.Parsing;
using Ion.CodeGeneration;
using System.IO;
using System.Collections.Generic;

namespace IonCLI.Core
{
    internal class Processor
    {
        protected readonly Handler handler;

        protected readonly Options options;

        public Processor(Handler handler, Options options)
        {
            this.handler = handler;
            this.options = options;
        }

        public string ProcessFile(string path)
        {
            // Ensure path file exists.
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Provided file path does not exist");
            }

            // Retrieve file contents.
            string content = File.ReadAllText(path);

            // Create the lexer.
            Lexer lexer = new Lexer(content);

            // Tokenize contents.
            List<Token> tokens = lexer.Tokenize();

            // Create the token stream.
            TokenStream stream = new TokenStream(tokens.ToArray());

            // TODO: Restrict file name by regex.
            // Extract the file name of the path.
            string fileName = Path.GetFileNameWithoutExtension(path);

            // Create the named driver.
            Driver driver = new Driver(stream, fileName);

            // Invoke the driver continuously.
            while (driver.HasNext)
            {
                driver.Next();
            }

            // Prepare expected output file path.
            string outputFile = Path.ChangeExtension(fileName, FileExtension.IR);

            // Resolve within the output directory.
            outputFile = this.options.PathResolver.Output(outputFile);

            // Emit the result.
            string result = this.handler.ProcessOperation(outputFile, driver);

            // Return the result.
            return result;
        }
    }
}
