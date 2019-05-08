using Ion.SyntaxAnalysis;
using Ion.Parsing;
using Ion.CodeGeneration;
using System.IO;
using System.Collections.Generic;

namespace Ion.CLI.Core
{
    public class Processor
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

            // Create the driver.
            Driver driver = new Driver(stream);

            // Invoke the driver continuously.
            while (driver.HasNext)
            {
                driver.Next();
            }

            // Emit the result.
            string result = this.handler.Emit(driver.Module);

            // Return the result.
            return result;
        }
    }
}
