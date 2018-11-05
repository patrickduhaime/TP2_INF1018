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
        private List<string> lexemes;
        public Dictionary<string, string> Dictionary { get; } = new Dictionary<string, string>();
        public List<string> Procedure { get; } = new List<string>();
        public List<List<string>> Instructions { get; } = new List<List<string>>();

        public AnalSyn(List<string> lexemes) => this.lexemes = lexemes;

        public bool Analyse()
        {
            if (!AnalyseProcedure())
                return false;

            while (lexemes[0] == "declare")
                if (!AnalyseDeclarations())
                    return false;

            while (lexemes.Count != 0)
                if (!AnalyseInstructions())
                    return false;

            return true;
        }

        private bool AnalyseProcedure()
        {
            if (lexemes[0] != "Procedure" || !lexemes[1].StartsWith("ID") || lexemes[lexemes.Count - 2] != "Fin_Procedure" 
                || !lexemes[lexemes.Count - 1].StartsWith("ID"))
                return false;

            Procedure.Add(lexemes[0]);
            Procedure.Add(lexemes[1]);
            Procedure.Add(lexemes[lexemes.Count - 2]);
            Procedure.Add(lexemes[lexemes.Count - 1]);

            lexemes.RemoveAt(0);
            lexemes.RemoveAt(0);
            lexemes.RemoveAt(lexemes.Count - 1);
            lexemes.RemoveAt(lexemes.Count - 1);
            return true;

        }

        private bool AnalyseDeclarations()
        {
            List<string> declaration = new List<string>();
            int index = 0;
            while (true)
            {
                declaration.Add(lexemes[index++]);
                if (declaration.Last() == ";")
                    break;
            }

            if (declaration.Count != 5 || declaration[0] != "declare" ||
                !declaration[1].StartsWith("ID") || declaration[2] != ":" ||
                (declaration[3] != "entier" && declaration[3] != "reel") ||
                declaration[4] != ";")
                return false;

            Dictionary.Add(declaration[1],declaration[3]);

            for (int i = 0; i < declaration.Count; i++)
                lexemes.RemoveAt(0);

            return true;
        }

        private bool AnalyseInstructions()
        {
            List<string> instruction = new List<string>();
            int index = 0;
            while (true)
            {
                instruction.Add(lexemes[index++]);
                if (instruction.Last() == ";" || index == lexemes.Count)
                    break;
            }

            if (instruction[1] != "=")
                return false;

            //TODO: Vérification de la validité des opérations mathématiques

            Instructions.Add(instruction);

            for (int i = 0; i < instruction.Count; i++)
                lexemes.RemoveAt(0);

            return true;
        }

    }
}
