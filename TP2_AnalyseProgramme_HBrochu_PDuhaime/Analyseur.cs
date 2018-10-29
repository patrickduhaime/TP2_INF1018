using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TP2_AnalyseProgramme_HBrochu_PDuhaime
{
    class Analyseur
    {
        public string Code { get; private set; }
        public List<string> Lexemes = new List<string>();

        public Analyseur(string code) => Code = code;

        public bool Analyse()
        {
            AnalLex analLex = new AnalLex(Code);
            Lexemes = analLex.Analyse();

            AnalSyn analSyn = new AnalSyn(Lexemes);
            if (!analSyn.Analyse())
                return false;

            //TODO: Analyseur sémantique (AnalSem)
            AnalSem analSem = new AnalSem(analSyn.Dictionary, analSyn.Procedure, analSyn.Instructions);
            if (!analSem.Analyse())
                return false;

            return true;
        }

    }
}
