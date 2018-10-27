﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2_AnalyseProgramme_HBrochu_PDuhaime
{
    class Program
    {
        static void Main(string[] args)
        
        {
            string file_name = "file.txt";
            string textLine = "";

            if(System.IO.File.Exists(file_name) == true)
            {
                System.IO.StreamReader objReader;
                objReader = new System.IO.StreamReader(file_name);

                do
                {
                    textLine = textLine + objReader.ReadLine();
                } while (objReader.Peek() != -1);
            }
            else
            {
                Console.WriteLine("Fichier introuvable!");
                Console.ReadLine();
                
            }

            Analyseur analyseur = new Analyseur(textLine);
            Console.WriteLine(analyseur.Analyse());
            Console.ReadLine();


        }



    }
}