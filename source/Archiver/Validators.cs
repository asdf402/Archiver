using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archiver
{
    public interface IValidator
    {
        bool IsValid(ParsedArguments parsedArguments);
    }
}