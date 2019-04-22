using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netCoreMvcTest.models
{
    public class LoginResultApiModel
    {
        /// <summary>
        /// authentication token used to stay for future requests
        /// </summary>
        public string Token { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
