using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2_AnalyseProgramme_HBrochu_PDuhaime
{
    //Classe qui analyse la sémantique du code.
    class AnalSem
    {
        private Dictionary<string, string> dictionary = new Dictionary<string, string>();
        private string leftElementType = "";

        public AnalSem() { }

       
        public void DeclareId(string id, string type) => dictionary.Add(id, type);

        public void SetLeftElementType(string leftElement) => leftElementType = dictionary[leftElement];

        //Vérifie si les deux identificateurs de la procédure sont les mêmes.
        public bool SameIdProcedure(string firstId, string secondId) => (firstId == secondId) ? true : Erreur(1);

        //Vérifie si un identificateur a été déclaré.
        public bool IdIsDeclared(string id) => dictionary.ContainsKey(id) ? true : Erreur(2);

        //Vérifie si un identificateur ne fait pas plus de 8 caractères.
        public bool IdIsNotOverEigth(string id) => id.Length < 9 ? true : Erreur(99); 

        //Vérifie qu'un id d'une expression n'est pas un réel si l'élément de gauche est un entier
        public bool IdHasValidType(string rightElement) => (leftElementType == "reel" || dictionary[rightElement] == "entier") ? true : Erreur(3);

        //Vérifie qu'un numbre d'une expression n'est pas un réel si l'élément de gauche est un entier
        public bool NumberHasValidType(string number) => (leftElementType == "reel" || number == "entier") ? true : Erreur(3);

        //Vérifie que l'élément de gauche n'est pas un entier (utilisé lorsqu'il y a une division dans l'expression).
        public bool LeftElementIsReel() => leftElementType == "reel" ? true : Erreur(3);

        //Traitement de l’erreur
        private bool Erreur(int code)
        {
            switch (code)
            {
                case 1:
                    Console.WriteLine(" Les deux identificateurs d’une procédure doivent être identiques ! Analyse sémantique Erreur 01");
                    break;
                case 2:
                    Console.WriteLine(" Toute variable utilisée dans une expression doit être préalablement déclarée ! Analyse sémantique Erreur 02");
                    break;
                case 3:
                    Console.WriteLine(" On ne peut pas affecter le résultat d’une expression réelle (type reel) à une variable de type entier ! Analyse sémantique Erreur 03");
                    break;
                default:
                    Console.WriteLine("Analyse sémantique Erreur");
                    break;
            }
            return false;
        }
    }
}
