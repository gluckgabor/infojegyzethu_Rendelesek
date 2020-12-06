using System;
using System.Collections.Generic;
using System.Text;

namespace infojegyzethu_Rendelesek
{
    class Raktarkeszlet
    {
        public string termekCikkszam { get; set; }
        public string termekNev { get; set; }
        public int termekAr { get; set; }
        public int mennyiseg { get; set; }
        public int quantityToBeProcuredThatCouldNotBeSatisfied { get; set; }

        public Raktarkeszlet(string line)
        {
            string[] lineSplitted = line.Split(";");

            termekCikkszam = lineSplitted[0];
            termekNev = lineSplitted[1];
            termekAr = Convert.ToInt32(lineSplitted[2]);
            mennyiseg = Convert.ToInt32(lineSplitted[3]);
        }


    }
}
