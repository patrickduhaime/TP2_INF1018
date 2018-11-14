using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2_AnalyseProgramme_HBrochu_PDuhaime
{
    //Classe qui décompose le code reçu en unités lexicales.
    class AnalLex
    {
        private string Code, Type, Lexeme;
        private char Char;
        private List<string> listIDs = new List<string>();

        public AnalLex(string code) => Code = code;

        public void PutBackLexeme() => Code = Lexeme + " " + Code;

        public List<string> GetList() => listIDs;

        //Isole et retourne le prochain lexeme du code.
        public string GetLexeme()
        {
            Lexeme = "";

            //Sélectionne le premier caractère qui n'est pas un espace vide.
            do {
                GetChar();
            }while (Char == ' ');
            
            switch (Type)
            {
                //Si le premier caractère est une lettre, alors le lexeme est soit un ID, soit un mot réservé.
                case "Letter":

                    AddChar();
                    GetChar();

                    //Tant que les caratères sont corrects, ajoute ces derniers au lexeme.
                    while (Type == "Letter" || Type == "Digit" || Type == "_")
                    {
                        if (Type == "_" && Lexeme != "Fin")
                            break;
                        AddChar();
                        GetChar();
                    }

                    if (Type != "End")
                        PutBackChar();

                    //Si c'est un mot réservé, retourne ce dernier.
                    if (Lexeme == "Procedure" || Lexeme == "Fin_Procedure" || Lexeme == "declare" || Lexeme == "entier" || Lexeme == "reel")
                        return Lexeme;

                    //Si l'ID n'existe pas encore, l'ajoute à une liste d'IDs.
                    if (!listIDs.Contains(Lexeme))
                        listIDs.Add(Lexeme);

                    //Retourne l'ID
                    return "ID" + listIDs.FindIndex(x => x == Lexeme);


                //Si le premier caractère est un chiffre, alors le lexeme est un chiffre entier ou réel.
                case "Digit":
                    AddChar();
                    GetChar();
                    bool point = false;
                    //Tant que les caratères sont des chiffres ou un unique point ou virgule, ajoute ces derniers au lexeme.
                    while (Type == "Digit" || Type == "." || Type == ",")
                    {
                        if (Type == "." || Type == ",")
                        {
                            if (point)
                                break;
                            else
                                point = true;
                        }
                        AddChar();
                        GetChar();
                    }

                    if (Type != "End")
                        PutBackChar();

                    //Retourne la valeur réel ou entier s'il y a un point (ou virgule) ou non.
                    return point ? "reel" : "entier";

                    //Retourne le caratère par défaut
                default:
                    AddChar();
                    return Lexeme;
            }
        }

        
        private void PutBackChar() => Code = Char + " " + Code;

        private void AddChar() => Lexeme = Lexeme + Char;

        private void GetChar()
        {
            if (Code.Length == 0)
            {
                Type = "End";
                return;
            }

            Char = Code[0];
            Code = Code.Remove(0, 1);
            GetTypeChar();
        }

        private void GetTypeChar()
        {
            if (Char >= 'A' && Char <= 'Z' || Char >= 'a' && Char <= 'z')
                Type = "Letter";
            else if (Char >= '0' && Char <= '9')
                Type = "Digit";
            else
                Type = Char.ToString();
        }

    }
}
