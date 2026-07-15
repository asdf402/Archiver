using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archiver
{
    public class Application
    {
        string[] commandLineArguments;

        Parser parser;
        ValidationService validationService;

        public Application(string[] commandLineArguments)
        {
            this.commandLineArguments = commandLineArguments;

            parser = new Parser();
            validationService = new ValidationService();
        }

        public void Run()
        {
            ParsedArguments parsedArguments;

            parsedArguments = parser.Parse(commandLineArguments);
            validationService.Validate(parsedArguments);
        }
    }
}