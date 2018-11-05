using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2_AnalyseProgramme_HBrochu_PDuhaime
{
    class AnalSem
    {
        private Dictionary<string, string> dictionary;
        private List<string> procedure;
        private List<List<string>> instructions;

        public AnalSem(Dictionary<string, string> dictionary, List<string> procedure, List<List<string>> instructions)
        {
            this.dictionary = dictionary;
            this.procedure = procedure;
            this.instructions = instructions;

            //Erreur.callErreur("113");
        }

        public bool Analyse()
        {
            //Vérifie si les deux identificateurs de la procédure sont les mêmes.
            if (procedure[1] != procedure.Last())
                return false;

            foreach (List<string> instruction in instructions)
            {
                //Vérifie si toutes les variables des instructions sont déclarées.
                foreach (string lexeme in instruction)
                    if (lexeme.StartsWith("ID") && !dictionary.ContainsKey(lexeme))
                        return false;

                //Si le résultat attendu n'est pas un integer, passe à l'instruction suivante.
                if (dictionary[instruction[0]] != "Integer")
                    continue;
                
                //Vérification s'il y a affectation d'une valeur réelle à un identificateur entier.
                foreach (string lexeme in instruction.Skip(1))
                {
                    //Erreur si l'instruction contient une division.
                    if (lexeme.StartsWith("/"))
                        return false;
                    //Erreur si le lexeme est une variable réelle.
                    if (lexeme.StartsWith("ID") && dictionary[lexeme] == "Reel")
                        return false;
                    //Erreur si le lexeme est un nombre réel.
                    if (lexeme == "Reel")
                        return false;
                }
            }

            return true;
        }
    }
}
