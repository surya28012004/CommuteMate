using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password,string hash);
    }
}
