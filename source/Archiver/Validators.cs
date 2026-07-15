using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archiver
{
    public interface Validators
    {
        void Validate(ParsedArguments parsedArguments);
    }
}