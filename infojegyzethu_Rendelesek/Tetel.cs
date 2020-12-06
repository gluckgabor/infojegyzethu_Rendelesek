using System;
using System.Collections.Generic;
using System.Text;

namespace infojegyzethu_Rendelesek
{
    class Tetel
    {
        public int rendelesSzam { get; set; }
        public string termekCikkszam { get; set; }
        public int rendeltMennyiseg { get; set; }
        public int orderLineItemValue { get; set; }

        public Tetel(string line)
        {
            string[] lineSplitted = line.Split(";");

            rendelesSzam = Convert.ToInt32(lineSplitted[1]);
            termekCikkszam = lineSplitted[2];
            rendeltMennyiseg = Convert.ToInt32(lineSplitted[3]);
        }

    }
}
