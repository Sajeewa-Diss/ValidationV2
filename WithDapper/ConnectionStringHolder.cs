using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WithDapper
{
    public sealed class ConnectionStringHolder
    {
        public ConnectionStringHolder(string value) => Value = value;

        public string Value { get; }
    }
}
