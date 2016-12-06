using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Nereus.Utils;

namespace Nereus.Models
{
    public class UserUISettings
    {
        public UserUISettings()
        {
            //Defaults
           TooltipsOn = true;

        }

        public int Id {get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool TooltipsOn { get; set; }

    }
}