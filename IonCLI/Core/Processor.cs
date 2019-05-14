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

        public Processor(Handler handler)
        {
            this.handler = handler;
        }

        public string ProcessFile(string path)
        {
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

            // Emit the result.
            string result = this.handler.ProcessOperation(driver);

            // Return the result.
            return result;
        }
    }
}
