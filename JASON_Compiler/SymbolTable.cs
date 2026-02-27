using System.Collections.Generic;

namespace JASON_Compiler
{
    public class Symbol {
        public string Name;
        public string Type;
    }
    public static class SymbolTable
    {
        public static List<Symbol> Symbols = new List<Symbol>();

        public static void AddSymbol(string name, string type)
        {
            Symbol sym = new Symbol();
            sym.Name = name;
            sym.Type = type;
            Symbols.Add(sym);
        }
        public static bool Contains(string name)
        {
            foreach (Symbol sym in Symbols) if (sym.Name == name) return true;
                
            return false;
        }
        public static Symbol GetSymbol(string name)
        {
            foreach (Symbol sym in Symbols) if (sym.Name == name) return sym;
                
            return null;
        }

    }
}