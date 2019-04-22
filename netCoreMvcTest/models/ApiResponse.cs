using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netCoreMvcTest.models
{
    /// <summary>
    /// response for all webapi call made
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {
        public bool Success => ErrorMessage == null;

        public string ErrorMessage { get; set; }

        /// <summary>
        /// api response object
        /// </summary>
        public T Response { get; set; }
    }
}
