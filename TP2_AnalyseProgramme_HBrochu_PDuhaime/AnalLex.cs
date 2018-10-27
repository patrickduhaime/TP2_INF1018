using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2_AnalyseProgramme_HBrochu_PDuhaime
{
    //Classe qui décompose le code reçu en unités lexicales.
    //TODO: Vérifier que l'analyse fonctionne bien lors de l'absence d'espace entre les lexemes.
    class AnalLex
    {
        public string Code { get; private set; }
        public char Char { get; private set; }
        public string Type { get; private set; }
        public string Lexeme { get; private set; } = "";
        private int countID = 0;
        private List<string> listIDs = new List<string>();


        public AnalLex(string code) => Code = code;


        public List<string> Analyse()
        {
            List<string> lexemes = new List<string>();

            while (Code != "")
            {
                lexemes.Add(GetLexeme());
                Lexeme = "";
            }
            return lexemes;
        }


        private string GetLexeme()
        {
            do {
                GetChar();
            }while (Char == ' ');
            
            switch (Type)
            {
                case "Letter":

                    AddChar();
                    GetChar();
                    while (Type == "Letter" || Type == "Digit" || Type == "_")
                    {
                        if (Type == "_" && Lexeme != "Fin")
                            break;
                        AddChar();
                        GetChar();
                    }

                    if (Lexeme == "Procedure" || Lexeme == "Fin_Procedure" || Lexeme == "declare" || Lexeme == "entier" || Lexeme == "reel")
                        return Lexeme;


                    if (listIDs.Contains(Lexeme))
                        return "ID" + listIDs.FindIndex(x => x == Lexeme);
                    else
                    {
                        listIDs.Add(Lexeme);
                        return "ID" + countID++;
                    }
                        

                case "Digit":
                    AddChar();
                    GetChar();
                    while (Type == "Digit")
                    {
                        AddChar();
                        GetChar();
                    }

                    return "Number";


                case ":":
                case ";":
                case "=":
                case "+":
                    return Char.ToString();
                default:
                    return Char.ToString();
            }

        }


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
            else if (Char == '_' || Char == ':' || Char == ';' || Char == '=' || Char == '+')
                Type = Char.ToString();
            else
                Type = "Other";
        }

    }
}
