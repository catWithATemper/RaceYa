using System;
using System.Collections.Generic;
using System.Text;

namespace RaceYa.Models
{
    public class GlobalParameters
    {
        public static GlobalParameters instance = null;
        public static GlobalParameters Instance()
        {
            if (instance == null)
                instance = new GlobalParameters();
            return instance;
        }

        public string NextRaceId { get; set; }
    }
}
