using Rpg.Classes.Abstracts;
using Rpg.Classes.Itens;
using Rpg.Classes.Personagens;
using System;
using System.Collections.Generic;
using static RPG.Mapa.Ui; 

namespace RPG.Mapa
{
    public class LojaDeItens
    {
        private readonly Personagem _heroi;
        private readonly List<Item> _itensDisponiveis;

        public LojaDeItens(Personagem heroi)
        {
            _heroi = heroi;
            _itensDisponiveis = new List<Item>
            {
                new Item("Poção de Vida", "Restaura 50 pontos de vida.", TipoEfeito.Cura, 50, 250),
                new Item("Amuleto da Força", "Aumenta permanentemente o ataque em 5.", TipoEfeito.AumentoAtaquePermanente, 5, 1000),
                new Item("Anel de Proteção", "Aumenta permanentemente a defesa em 3.", TipoEfeito.AumentoDefesaPermanente, 3, 800),
                new Item("Antídoto", "Cura envenenamento.", TipoEfeito.Cura, 0, 150) 
            };

            Executar();
        }

        private void Executar()
        {
            string escolha = "";
            while (escolha != "0")
            {
                Console.Clear();
                Painel(() =>
                {
                    EscreverCentral("🧪 Bem-vindo à Loja de Itens! 🧪", 0, ConsoleColor.Green);
                    Linha();
                    Typewriter("Poções, amuletos e tudo que um aventureiro precisa!", VelocidadeTextoMs, ConsoleColor.Green);
                    Console.WriteLine();
                    EscreverCentral($"Seu ouro: {_heroi.Ouro}", 0, ConsoleColor.Yellow);
                    Linha();

                    for (int i = 0; i < _itensDisponiveis.Count; i++)
                    {
                        var item = _itensDisponiveis[i];
                        EscreverCentral($"[{i + 1}] {item.Nome} ({item.Preco} Ouro) - {item.Descricao}", 0, ConsoleColor.Green);
                    }
                    Console.WriteLine();
                    EscreverCentral("[0] Sair da Loja", 0, ConsoleColor.DarkGray);
                }, "LOJA DE ITENS", ConsoleColor.Green);

                escolha = LerEntradaPainel("Escolha o item que deseja comprar: ");

                if (int.TryParse(escolha, out int index) && index > 0 && index <= _itensDisponiveis.Count)
                {
                    ComprarItem(_itensDisponiveis[index - 1]);
                }
                else if (escolha != "0")
                {
                    Painel(() => EscreverCentral("❌ Opção inválida.", 0, ConsoleColor.Red), "ERRO", ConsoleColor.Red);
                    Pausa();
                }
            }
            Painel(() => EscreverCentral("Obrigado e volte sempre!", 0, ConsoleColor.Green), "SAINDO", ConsoleColor.Green);
        }

        private void ComprarItem(Item item)
        {
            if (_heroi.Ouro >= item.Preco)
            {
                _heroi.Ouro -= item.Preco;
                _heroi.Inventario.AdicionarItem(item);
                Painel(() =>
                {
                    EscreverCentral($"Você comprou {item.Nome}!", 0, ConsoleColor.Cyan);
                    EscreverCentral($"Ouro restante: {_heroi.Ouro}", 0, ConsoleColor.Yellow);
                }, "COMPRA REALIZADA", ConsoleColor.Cyan);
            }
            else
            {
                Painel(() => EscreverCentral("💰 Ouro insuficiente.", 0, ConsoleColor.Red), "AVISO", ConsoleColor.Red);
            }
            Pausa();
        }
    }
}