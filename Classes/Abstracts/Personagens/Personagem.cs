using System;
using System.Threading;
using Rpg.Classes.Itens;

namespace RPG.Classes.Abstracts.Personagens
{
    public class Personagem
    {
        private Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public int Nivel { get; set; }
        public int Vida { get; set; }
        public int VidaMaxima { get; set; }
        public int Ataque { get; set; }
        public int Defesa { get; set; }
        public int Experiencia { get; set; }
        public int Ouro { get; set; }
        public bool EstaVivo => Vida > 0;

        public Inventario Inventario { get; private set; }

        public Personagem(string nome, int nivel, int ataque, int defesa)
        {
            Nome = nome;
            Nivel = nivel;
            Ataque = ataque;
            Defesa = defesa;
            Experiencia = 0;
            Ouro = 50;
            InicializarAtributos();
            Inventario = new Inventario();
        }

        private void InicializarAtributos()
        {
            VidaMaxima = 100 + Nivel * 10;
            Vida = VidaMaxima;
            Ataque = Ataque + 15 + Nivel * 2;
            Defesa = Defesa + 5 + Nivel;
        }

        public void ReceberDano(int dano)
        {
           
            int danoReal = Math.Max(1, dano - Defesa);
            Vida = Math.Max(0, Vida - danoReal); 

            Console.WriteLine($"{Nome} recebeu {danoReal} de dano!");

            if (!EstaVivo)
            {
                Console.WriteLine($" {Nome} foi derrotado!");
            }
        }

        public virtual void ConcederRecompensa(Personagem agressor)
        {
            int expGanho = Nivel * 10;
            int ouroGanho = Nivel * 50;

            agressor.AdicionarExperiencia(expGanho);
            agressor.AdicionarOuro(ouroGanho);
            Console.WriteLine($" {agressor.Nome} derrotou {Nome} e ganhou {expGanho} de EXP e {ouroGanho} de ouro!");
        }

        public void Curar(int quantidade)
        {
            int vidaAntes = Vida;
            Vida = Math.Min(Vida + quantidade, VidaMaxima);
            int vidaCurada = Vida - vidaAntes;

            Console.WriteLine($" {Nome} recuperou {vidaCurada} de vida!");
        }

        public void AdicionarExperiencia(int exp)
        {
            Experiencia += exp;
            Console.WriteLine($" {Nome} ganhou {exp} de experiência!");
            VerificarSubidaNivel();
        }

        private void VerificarSubidaNivel()
        {
            int expNecessaria = Nivel * 100; 
            if (Experiencia >= expNecessaria)
            {
                SubirNivel();
            }
        }

        public void SubirNivel()
        {
            Nivel++;
            int vidaAntes = VidaMaxima;
            int ataqueAntes = Ataque;
            int defesaAntes = Defesa;

            InicializarAtributos();
            Vida = VidaMaxima;

            Console.WriteLine($"\n🎉 {Nome} subiu para o nível {Nivel}!");
            Console.WriteLine($"   Vida: {vidaAntes} -> {VidaMaxima}");
            Console.WriteLine($"   Ataque: {ataqueAntes} -> {Ataque}");
            Console.WriteLine($"   Defesa: {defesaAntes} -> {Defesa}");
        }

        public int CalcularDano()
        {
            Random random = new Random();
            int danoBase = Ataque;
            int bonusAleatorio = random.Next(1, 6);
            return danoBase + bonusAleatorio;
        }

        public void AtacarAlvo(Personagem alvo)
        {
            if (!EstaVivo)
            {
                Console.WriteLine($" {Nome} não pode atacar porque está morto!");
                return;
            }
            if (!alvo.EstaVivo)
            {
                Console.WriteLine($" {alvo.Nome} já está morto!");
                return;
            }

            int dano = CalcularDano();
            Console.WriteLine($"\n {Nome} ataca {alvo.Nome}!");
            alvo.ReceberDano(dano);
        }

        public void AdicionarOuro(int quantidade)
        {
            Ouro += quantidade;
        }

        public bool GastarOuro(int quantidade)
        {
            if (Ouro >= quantidade)
            {
                Ouro -= quantidade;
                Console.WriteLine($" {Nome} gastou {quantidade} de ouro. Restante: {Ouro}");
                return true;
            }
            else
            {
                Console.WriteLine($" {Nome} não tem ouro suficiente!");
                return false;
            }
        }

        public void Reviver()
        {
            if (!EstaVivo)
            {
                Vida = VidaMaxima / 2;
                Console.WriteLine($" {Nome} foi revivido com metade da vida!");
            }
        }

        public void UsarItem(Item item)
        {
            if (!Inventario.PossuiItem(item))
            {
                Console.WriteLine($" {Nome} não possui o item {item.Nome}!");
                return;
            }

            switch (item.Efeito)
            {
                case TipoEfeito.Cura:
                    Curar(item.Valor);
                    break;
                case TipoEfeito.AumentoAtaquePermanente:
                    Ataque += item.Valor;
                    Console.WriteLine($" {Nome} usou {item.Nome} e seu ataque aumentou permanentemente em {item.Valor}!");
                    break;
                case TipoEfeito.AumentoDefesaPermanente:
                    Defesa += item.Valor;
                    Console.WriteLine($" {Nome} usou {item.Nome} e sua defesa aumentou permanentemente em {item.Valor}!");
                    break;
                case TipoEfeito.ItemChave:
                    Console.WriteLine($" {item.Nome}: {item.Descricao}");
                    break;
            }

            if (item.Consumivel)
            {
                Inventario.RemoverItem(item);
            }
        }

        public void MostrarStatus()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine($" STATUS DE {Nome.ToUpper()}");
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($" Vida: {Vida} / {VidaMaxima}");
            Console.WriteLine($" Ataque: {Ataque}");
            Console.WriteLine($"Defesa: {Defesa}");
            Console.WriteLine($"Nível: {Nivel} (EXP: {Experiencia})");
            Console.WriteLine($" Ouro: {Ouro}");
            Console.WriteLine(new string('=', 40));
        }
    }
}