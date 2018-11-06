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
                return methodeErreur(1);

            while (lexemes[0] == "declare")
                if (!AnalyseDeclarations())
                    return methodeErreur(2);

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

            //TODO: Vérification de la validité des opérations mathématiques

            if (!instruction[0].StartsWith("ID"))
                return methodeErreur(3);

            if (instruction[1] != "=")
                return methodeErreur(4);

            if (instruction[2] != "Number")
                return methodeErreur(5);

            if (instruction[instruction.Count-1] != "Number")
                return methodeErreur(6);

            for (int i = 4; i < instruction.Count; i+=2)
            {
                if (instruction[i] != "Number")
                    return methodeErreur(7);
                if (instruction[i - 1] != "+" && instruction[i - 1] != "-" && instruction[i - 1] != "*" && instruction[i - 1] != "/")
                    return methodeErreur(8);
            }

            Instructions.Add(instruction);

            for (int i = 0; i < instruction.Count; i++)
                lexemes.RemoveAt(0);

            return true;  
        }
        

        private bool methodeErreur(int code)
        {
            ///////Traitement de l’erreur

            switch (code)
            {
                case 1:
                    Console.WriteLine("La syntaxe de la procedure est erronée ! Analyse syntaxique Erreur 01");
                    break;
                case 2:
                    Console.WriteLine("La syntaxe de la declaration est erronée ! Analyse syntaxique Erreur 02");
                    break;
                case 3:
                    Console.WriteLine("Une instruction doit debuter par un identificateur ! Analyse syntaxique Erreur 03");
                    break;
                case 4:
                    Console.WriteLine("Une instruction doit avoir un signe '=' apres l'identificateur ! Analyse syntaxique Erreur 04");
                    break;
                case 5:
                    Console.WriteLine("Une instruction doit avoir un nombre apres le signe '=' ! Analyse syntaxique Erreur 05");
                    break;
                case 6:
                    Console.WriteLine("Une instruction doit se terminer par un nombre ! Analyse syntaxique Erreur 06");
                    break;
                case 7:
                    Console.WriteLine("Une instruction doit contenir des nombres seulement ! Analyse syntaxique Erreur 07");
                    break;
                case 8:
                    Console.WriteLine("Une instruction doit contenir les operateurs mathematiques + - * ou / seulement ! Analyse syntaxique Erreur 08");
                    break;
                default:
                    Console.WriteLine("Analyse syntaxique Erreur");
                    break;
            }

            return false;
        }
    }
}
