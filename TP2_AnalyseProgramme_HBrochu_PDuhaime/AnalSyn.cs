using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TP2_AnalyseProgramme_HBrochu_PDuhaime
{
    //Classe qui analyse la syntaxe du code.
    class AnalSyn
    {
        private AnalLex analLex;
        private AnalSem analSem = new AnalSem();

        public AnalSyn(AnalLex analLex) => this.analLex = analLex;

        private string Next() => analLex.GetLexeme();

        public bool Analyse() => Procedure();

        //Analyse de la procédure
        private bool Procedure()
        {
            if (Next() != "Procedure")
                return Erreur(1);

            string firstId = Next();
            if (!firstId.StartsWith("ID"))
                return Erreur(1);

            //Analyse des déclarations et des instructions
            if (!Declarations() || !Instructions())
                return false;

            if (Next() != "Fin_Procedure") 
                return Erreur(1);

            string secondId = Next();
            if (!secondId.StartsWith("ID"))
                return Erreur(1);

            //Appel de l'analyse sémantique.
            return analSem.SameIdProcedure(firstId, secondId);
        }

        //Analyse des déclarations.
        private bool Declarations()
        {
            //Tant que le prochain lexeme est "declare", on détermine qu'il reste une déclaration et on l'analyse.
            while (Next() == "declare")
                if (!Declaration())
                    return false;

            analLex.PutBackLexeme();
            return true;
        }

        //Analyse d'une déclaration.
        private bool Declaration()
        {
            string id = Next();
            if (!id.StartsWith("ID") || Next() != ":")
                return Erreur(2);

            string type = Next();
            if ((type != "entier" && type != "reel") || Next() != ";")
                return Erreur(2);

            //On ajoute la valeur déclarée à un dictionnaire de l'analyseur sémantique.
            analSem.DeclareId(id, type);

            return true;
        }

        //Analyse des instructions.
        private bool Instructions()
        {
            if (!Instruction())
                return false;

            //Tant que le prochain lexeme est ";", on détermine qu'il reste une instruction et on l'analyse.
            string endIns = Next();
            while (endIns == ";")
            {
                if (!Instruction())
                    return false;
                endIns = Next();
            }

            analLex.PutBackLexeme();
            return true;
        }

        //Analyse d'une instruction.
        private bool Instruction()
        {
            string leftElement = Next();
            if (!leftElement.StartsWith("ID"))
                return Erreur(3);

            //Analyse sémantique : On vérifie que l'ID est déclaré.
            if (!analSem.IdIsDeclared(leftElement))
                return false;

            //Analyse sémantique : On déclare si le résultat de l'instruction attend un réel ou un entier.
            analSem.SetLeftElementType(leftElement);

            if (Next() != "=")
                return Erreur(4);

            return Expression();
        }

        //Analyse d'une expression.
        private bool Expression()
        {
            if (!Terme())
                return false;
            string operateur = Next();
            while(operateur == "+" || operateur == "-")
            {
                if (!Terme())
                    return false;
                operateur = Next();
            }
            analLex.PutBackLexeme();
            return true;
        }

        //Analyse d'un terme.
        private bool Terme()
        {
            if (!Facteur())
                return false;
            string operateur = Next();
            while (operateur == "*" || operateur == "/")
            {
                //Analyse sémantique : Si l'opérateur est "/", on vérifie que le résultat attend un réel.
                if (operateur == "/" && !analSem.LeftElementIsReel())
                    return false;

                if (!Facteur())
                    return false;
                operateur = Next();
            }
            analLex.PutBackLexeme();
            return true;
        }

        //Analuse d'un facteur.
        private bool Facteur()
        {
            string facteur = Next();
            if (facteur == "(")
            {
                if (!Expression())
                    return false;
                if (Next() != ")")
                    return Erreur(6);
            }
            else if (!facteur.StartsWith("ID") && !facteur.StartsWith("entier") && !facteur.StartsWith("reel"))
                return Erreur(5);

            //Analyse sémantique : Vérifie que l'ID est déclaré et que l'ID n'est pas un réel si le résultat attendu est un entier.
            if (facteur.StartsWith("ID") && (!analSem.IdIsDeclared(facteur) || !analSem.IdHasValidType(facteur)))
                return false;

            //Analyse sémantique : Vérifie que le nombre n'est pas un réel si le résultat attendu est un entier.
            if (facteur.StartsWith("entier") && !analSem.NumberHasValidType(facteur))
                return false;

            return true;
        }

        //Traitement de l’erreur
        private bool Erreur(int code)
        {
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
                    Console.WriteLine("Une expression est invalide. Analyse syntaxique Erreur 05");
                    break;
                case 6:
                    Console.WriteLine("Une parenthèse est ouverte mais n'est pas fermée convenablement ! Analyse syntaxique Erreur 06");
                    break;
                default:
                    Console.WriteLine("Analyse syntaxique Erreur");
                    break;
            }
            return false;
        }
    }
}
