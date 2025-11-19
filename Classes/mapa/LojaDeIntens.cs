using System;
using System.Collections.Generic;

namespace RPG.Mapa
{
    public class LojaDeItens
    {
        private int ouro;
        private  List<Item> _itensDisponiveis = new();

        public LojaDeItens(int ouroInicial)
        {
            ouro = ouroInicial;
            _itensDisponiveis = new List<Item>
            {
                new Item("Poção de Vida", "Restaura 50 pontos de vida.", 250),
                new Item("Amuleto da Força", "Aumenta permanentemente o ataque em 5.", 1000),
                new Item("Anel de Proteção", "Aumenta permanentemente a defesa em 3.", 800),
                new Item("Antídoto", "Cura envenenamento.", 150)
            };
        }

        public void Executar()
        {
            var Som = new AudioPlayer();
            Som.PlayLoop("mundo.mp3");
            string escolha = "";

            while (escolha != "0")
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" Bem-vindo à Loja de Itens! ");
                Console.ResetColor();

                Console.WriteLine("Poções, amuletos e tudo que um aventureiro precisa!");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Seu ouro: {ouro}");
                Console.ResetColor();
                Console.WriteLine("-------------------------------");

                for (int i = 0; i < _itensDisponiveis.Count; i++)
                {
                    var item = _itensDisponiveis[i];
                    Console.WriteLine($"[{i + 1}] {item.Nome} ({item.Preco} ouro) - {item.Descricao}");
                }

                Console.WriteLine("[0] Sair da Loja");
                Console.Write("\nEscolha o item que deseja comprar: ");
                escolha = Console.ReadLine() ?? "";

                if (int.TryParse(escolha, out int index) && index > 0 && index <= _itensDisponiveis.Count)
                {
                    ComprarItem(_itensDisponiveis[index - 1]);
                }
                else if (escolha != "0")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n Opção inválida!");
                    Console.ResetColor();
                    Console.WriteLine("Pressione ENTER para continuar...");
                    Console.ReadLine();
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nObrigado por visitar a loja! Volte sempre!");
            Console.ResetColor();

            Som.Stop(); 
        }

        private void ComprarItem(Item item)
        {
            Console.Clear();
            if (ouro >= item.Preco)
            {
                ouro -= item.Preco;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Você comprou {item.Nome}!");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Ouro restante: {ouro}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Ouro insuficiente!");
                Console.ResetColor();
            }

            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
        }
    }

    public class Item
    {
        public string Nome { get; }
        public string Descricao { get; }
        public int Preco { get; }

        public Item(string nome, string descricao, int preco)
        {
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
        }
    }
}

// codigo espaguete jornafa do heroi histoiria telin