using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2_AnalyseProgramme_HBrochu_PDuhaime
{
    //Classe qui analyse les unités lexicales et vérifie que la structure du code est valide.

    //TODO: Vérification de la validité des opérations mathématiques (ligne 83)
    class AnalSyn
    {
        public List<string> Lexemes = new List<string>();

        public AnalSyn(List<string> lexemes) => Lexemes = lexemes;

        public bool Analyse()
        {
            if (!AnalyseProcedure())
                return false;

            while (Lexemes[0] == "declare")
                if (!AnalyseDeclarations())
                    return false;

            while (Lexemes.Count != 0)
                if (!AnalyseInstructions())
                    return false;

            return true;
        }

        private bool AnalyseProcedure()
        {
            if (Lexemes[0] != "Procedure" || Lexemes[Lexemes.Count - 2] != "Fin_Procedure" || Lexemes[1] != Lexemes.Last())
                return false;

            Lexemes.RemoveAt(0);
            Lexemes.RemoveAt(0);
            Lexemes.RemoveAt(Lexemes.Count - 1);
            Lexemes.RemoveAt(Lexemes.Count - 1);
            return true;
        }

        private bool AnalyseDeclarations()
        {
            List<string> declaration = new List<string>();
            int index = 0;
            while (true)
            {
                declaration.Add(Lexemes[index++]);
                if (declaration.Last() == ";")
                    break;
            }

            if (declaration.Count() != 5 || declaration[0] != "declare" ||
                !declaration[1].StartsWith("ID") || declaration[2] != ":" ||
                (declaration[3] != "entier" && declaration[3] != "reel") ||
                declaration[4] != ";")
                return false;

            for (int i = 0; i < declaration.Count; i++)
                Lexemes.RemoveAt(0);

            return true;
        }

        private bool AnalyseInstructions()
        {
            List<string> instruction = new List<string>();
            int index = 0;
            while (true)
            {
                instruction.Add(Lexemes[index++]);
                if (instruction.Last() == ";" || index == Lexemes.Count)
                    break;
            }

            if (instruction[1] != "=")
                return false;

            //TODO: Vérification de la validité des opérations mathématiques

            for (int i = 0; i < instruction.Count; i++)
                Lexemes.RemoveAt(0);

            return true;
        }

    }
}
