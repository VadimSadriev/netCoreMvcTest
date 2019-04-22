using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netCoreMvcTest.models
{
    /// <summary>
    /// credentials for api client log into the server and recieve token back
    /// </summary>
    public class LoginCredentialsApiModel
    {
        public string UserNameOrEmail { get; set; }

        public string Password { get; set; }
    }
}
