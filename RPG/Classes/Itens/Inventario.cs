using System.Collections.Generic;
using System.Linq;

namespace Rpg.Classes.Itens
{
    public class Inventario
    {
   
        private readonly Dictionary<Item, int> _itens = new Dictionary<Item, int>();

        public void AdicionarItem(Item item, int quantidade = 1)
        {
        
            if (_itens.ContainsKey(item))
            {
                _itens[item] += quantidade;
            }
            else
            {
                _itens.Add(item, quantidade);
            }
        }

        public void RemoverItem(Item item, int quantidade = 1)
        {
            if (_itens.ContainsKey(item))
            {
                _itens[item] -= quantidade;
                
                if (_itens[item] <= 0)
                {
                    _itens.Remove(item);
                }
            }
        }

        public bool PossuiItem(Item item)
        {
            return _itens.ContainsKey(item) && _itens[item] > 0;
        }

        // Retorna uma cópia somente leitura do inventário para exibição.
        public IReadOnlyDictionary<Item, int> ObterItens()
        {
            return _itens;
        }
    }
}