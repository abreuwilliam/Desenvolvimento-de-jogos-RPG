namespace Rpg.Classes.Itens
{
    public enum TipoEfeito
    {
        Cura,
        AumentoAtaquePermanente,
        AumentoDefesaPermanente,
        ItemChave 
    }

    public class Item
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public TipoEfeito Efeito { get; private set; }
        public int Valor { get; private set; } 
        public int Preco { get; private set; }
        public bool Consumivel { get; private set; } 

        public Item(string nome, string descricao, TipoEfeito efeito, int valor, int preco, bool consumivel = true)
        {
            Nome = nome;
            Descricao = descricao;
            Efeito = efeito;
            Valor = valor;
            Preco = preco;
            Consumivel = consumivel;
        }
    }
}
