using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace netCoreMvcTest.Data
{
    /// <summary>
    /// Settings database table
    /// </summary>
    public class SettingsDataModel
    {
        [Key]
        public string Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(2048)]       
        public string Value { get; set; }

    }
}
