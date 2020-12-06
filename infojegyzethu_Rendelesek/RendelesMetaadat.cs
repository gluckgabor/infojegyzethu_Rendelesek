using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace infojegyzethu_Rendelesek
{
    class RendelesMetaadat
    {
        CultureInfo cultures = new CultureInfo("hu-HU");

        public DateTime rendelesDatuma { get; set; }
        public int rendelesSzam { get; set; }
        public string emailCim { get; set; }
        public string state { get; set; }
        public int totalValue { get; set; }

        public RendelesMetaadat(string line)
        {
            string[] lineSplitted = line.Split(";");

            rendelesDatuma = Convert.ToDateTime(lineSplitted[1], cultures);
            rendelesSzam = Convert.ToInt32(lineSplitted[2]); 
            emailCim = lineSplitted[3];
        }

    }
}
