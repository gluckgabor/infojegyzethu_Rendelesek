using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace infojegyzethu_Rendelesek
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] linesRaktarkeszlet = File.ReadAllLines("C:/Users/Glück Gábor/source/repos/infojegyzethu_Rendelesek/infojegyzethu_Rendelesek/raktarkeszlet.csv");
            string[] linesRendelesMetaadat = File.ReadAllLines("C:/Users/Glück Gábor/source/repos/infojegyzethu_Rendelesek/infojegyzethu_Rendelesek/rendeles.csv");
            string[] linesTetel = File.ReadAllLines("C:/Users/Glück Gábor/source/repos/infojegyzethu_Rendelesek/infojegyzethu_Rendelesek/rendeles.csv");


            List<Raktarkeszlet> raktarKeszletList = new List<Raktarkeszlet>();
            List<RendelesMetaadat> rendelesMetaadatList = new List<RendelesMetaadat>();
            List<Tetel> tetelList = new List<Tetel>();
            

            for (int i = 0; i < linesRaktarkeszlet.Length; i++)
            {
                raktarKeszletList.Add(new Raktarkeszlet(linesRaktarkeszlet[i]));
            }

            for (int i = 0; i < linesRendelesMetaadat.Length; i++)
            {
                if (linesRendelesMetaadat[i].Substring(0,1) == "M")
                {
                    rendelesMetaadatList.Add(new RendelesMetaadat(linesRendelesMetaadat[i]));
                }                
            }

            for (int i = 0; i < linesTetel.Length; i++)
            {
                if (linesTetel[i].Substring(0, 1) == "T")
                {
                    tetelList.Add(new Tetel(linesTetel[i]));
                }
            }

            secondExcercise(raktarKeszletList, rendelesMetaadatList, tetelList);

            thirdExcercise(raktarKeszletList, rendelesMetaadatList, tetelList);

            fourthExcercise(raktarKeszletList);
       
        }

        private static void fourthExcercise(List<Raktarkeszlet> raktarKeszletList)
        {
            using (StreamWriter sw = new StreamWriter("C:/Users/Glück Gábor/source/repos/infojegyzethu_Rendelesek/infojegyzethu_Rendelesek/beszerzes.csv", false))
            {
                List<Raktarkeszlet> sortedraktarKeszletList = raktarKeszletList.OrderBy(o => o.termekCikkszam).ToList();

                foreach (var raktarKeszlet in sortedraktarKeszletList)
                {
                    if (raktarKeszlet.quantityToBeProcuredThatCouldNotBeSatisfied > 0)
                        {
                            sw.WriteLine(raktarKeszlet.termekCikkszam + ";" + Convert.ToString(raktarKeszlet.quantityToBeProcuredThatCouldNotBeSatisfied - raktarKeszlet.mennyiseg));
                        }
                    }
            sw.Close();
            }
        }

        private static void thirdExcercise(List<Raktarkeszlet> raktarKeszletList, List<RendelesMetaadat> rendelesMetaadatList, List<Tetel> tetelList)
        {
            using (StreamWriter sw = new StreamWriter("C:/Users/Glück Gábor/source/repos/infojegyzethu_Rendelesek/infojegyzethu_Rendelesek/levelek.csv", false))
            {
                foreach (var rendelesMetaadat in rendelesMetaadatList)
                {
                    if (rendelesMetaadat.state != "Pending")
                    {
                        sw.WriteLine(rendelesMetaadat.emailCim + ";A rendelését két napon belül szállítjuk. A rendelés értéke: " + rendelesMetaadat.totalValue + " Ft");
                    }
                    else
                    {
                        sw.WriteLine(rendelesMetaadat.emailCim + ";A rendelése függő állapotba került. Hamarosan értesítjük a szállítás időpontjáról.");

                    }
                }
                sw.Close();
            }
           
        }

        private static void secondExcercise(List<Raktarkeszlet> raktarKeszletList, List<RendelesMetaadat> rendelesMetaadatList, List<Tetel> tetelList)
        {
            List<Tuple<string, int>> orderedGoodsTupleList = new List<Tuple<string, int>>();
            List<bool> orderCanBeSatisfiedList = new List<bool>();

            foreach (var rendelesMetaadat in rendelesMetaadatList)
            {               
                foreach (var tetel in tetelList)
                {
                    if (rendelesMetaadat.rendelesSzam == 17 && tetel.termekCikkszam == "BG01")
                    {
                        Debug.Print(Convert.ToString("Order no.: " + rendelesMetaadat.rendelesSzam + ", ordered qty: " + tetel.rendeltMennyiseg));
                    }

                    if (rendelesMetaadat.rendelesSzam == tetel.rendelesSzam)
                    {
                        foreach (var keszlet in raktarKeszletList)
                        {
                            if (tetel.termekCikkszam == keszlet.termekCikkszam)
                            {
                                if (tetel.rendeltMennyiseg <= keszlet.mennyiseg)
                                {
                                    orderedGoodsTupleList.Add(Tuple.Create(tetel.termekCikkszam, tetel.rendeltMennyiseg));
                                    orderCanBeSatisfiedList.Add(true);
                                }
                                else if (tetel.rendeltMennyiseg > keszlet.mennyiseg)
                                {
                                    orderCanBeSatisfiedList.Add(false);

                                    if (keszlet.termekCikkszam == "BG01")
                                    {
                                        Debug.Print(Convert.ToString("From " + keszlet.termekCikkszam + ":" + tetel.rendeltMennyiseg + "could not be sold"));
                                        Debug.Print(Convert.ToString("qty on stock was: " + keszlet.mennyiseg + ", which was not enough to supply demand: " + tetel.rendeltMennyiseg));
                                    }

                                    keszlet.quantityToBeProcuredThatCouldNotBeSatisfied += tetel.rendeltMennyiseg;

                                    if (keszlet.termekCikkszam == "BG01")
                                    {
                                        Debug.Print(Convert.ToString(keszlet.termekCikkszam + ":" + keszlet.quantityToBeProcuredThatCouldNotBeSatisfied));
                                    }
                                }
                                rendelesMetaadat.totalValue += calculateOrderLineItemValue(tetel, keszlet);
                            }
                        }
                    }
                    
                }

                if (!orderCanBeSatisfiedList.Contains(false))
                {
                    removeOrderedQuantitiesFromStock(orderedGoodsTupleList, raktarKeszletList, rendelesMetaadatList, tetelList);
                    orderedGoodsTupleList.Clear();
                }
                else
                {
                    rendelesMetaadat.state = "Pending";
                    //Debug.Print(Convert.ToString("*********************** kielégíthetelen rendelés száma:" + rendelesMetaadat.rendelesSzam));
                }
                
                orderCanBeSatisfiedList.Clear();
            }            
        }

        private static int calculateOrderLineItemValue(Tetel tetel, Raktarkeszlet keszlet)
        {
           return tetel.orderLineItemValue = tetel.rendeltMennyiseg * keszlet.termekAr;
        }

        private static void removeOrderedQuantitiesFromStock(List<Tuple<string, int>> orderedGoodsTupleList, List<Raktarkeszlet> raktarKeszletList, List<RendelesMetaadat> rendelesMetaadatList, List<Tetel> tetelList)
        {
            foreach (var orderedGoodTuple in orderedGoodsTupleList)
            {
                foreach (var keszlet in raktarKeszletList)
                {
                    if (orderedGoodTuple.Item1 == keszlet.termekCikkszam)
                    {
                    //Debug.Print(Convert.ToString(keszlet.termekNev + " előtte:" + keszlet.mennyiseg));
                    keszlet.mennyiseg = keszlet.mennyiseg - orderedGoodTuple.Item2;
                    //Debug.Print(Convert.ToString(keszlet.termekNev + " utana:" + keszlet.mennyiseg));
                    }
                }   
            }
        }
    }
}
