using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archiver
{
    public interface IValidator
    {
        void Validate(ParsedArguments parsedArguments);
    }
}