using System;

namespace Rpg.Classes.Abstracts
{
    public class Personagem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Nome { get; private set; }
        public int Nivel { get; private set; }
        public int Vida { get; private set; }
        public int VidaMaxima { get; private set; }
        public int Ataque { get; private set; }
        public int Defesa { get; private set; }
        public int Experiencia { get; private set; }
        public int Ouro { get; private set; }
        public bool EstaVivo => Vida > 0;

        public Personagem(string nome, int nivel, int ataque, int defesa)
        {
            Nome = nome;
            Nivel = nivel;
            Ataque = ataque;
            Defesa = defesa;
            Experiencia = 0;
            Ouro = 50;
            InicializarAtributos();
        }

        private void InicializarAtributos()
        {
            VidaMaxima = 100 + (Nivel * 10);
            Vida = VidaMaxima;
            Ataque = Ataque + 15 + (Nivel * 2);
            Defesa = Defesa + 5 + Nivel;
        }

        public void ReceberDano(int dano)
        {
            int danoReal = Math.Max(dano - Defesa, 1);
            Vida = Math.Max(Vida - danoReal, 0);

            Console.WriteLine($"💥 {Nome} recebeu {danoReal} de dano!");

            if (!EstaVivo)
            {
                Console.WriteLine($"💀 {Nome} foi derrotado!");
            }
        }

        public virtual void ConcederRecompensa(Personagem agressor)
        {

            int expGanho = Nivel * 10;
            int ouroGanho = Nivel * 50;

            agressor.AdicionarExperiencia(expGanho);
            agressor.AdicionarOuro(ouroGanho);
            Console.WriteLine($"🎉 {agressor.Nome} derrotou {Nome} e ganhou {expGanho} de EXP e {ouroGanho} de ouro!");
        }

        public void Curar(int quantidade)
        {
            int vidaAntes = Vida;
            Vida = Math.Min(Vida + quantidade, VidaMaxima);
            int vidaCurada = Vida - vidaAntes;

            Console.WriteLine($"❤️ {Nome} recuperou {vidaCurada} de vida!");
        }

        public void AdicionarExperiencia(int exp)
        {
            Experiencia += exp;
            Console.WriteLine($"⭐ {Nome} ganhou {exp} pontos de experiência!");

            VerificarSubidaNivel();
        }

        private void VerificarSubidaNivel()
        {
            int expNecessaria = Nivel * 10;

            if (Experiencia >= expNecessaria)
            {
                SubirNivel();
            }
        }


        public void SubirNivel()
        {
            Nivel++;
            int vidaAntes = VidaMaxima;

            InicializarAtributos(); 
            Vida = VidaMaxima; 

            Console.WriteLine($"\n🎉 {Nome} subiu para o nível {Nivel}!");
            Console.WriteLine($"📊 Vida: {vidaAntes} → {VidaMaxima}");
            Console.WriteLine($"⚔️ Ataque: {Ataque - 2} → {Ataque}");
            Console.WriteLine($"🛡️ Defesa: {Defesa - 1} → {Defesa}");
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
                Console.WriteLine($"❌ {Nome} não pode atacar porque está morto!");
                return;
            }

            if (!alvo.EstaVivo)
            {
                Console.WriteLine($"❌ {alvo.Nome} já está morto!");
                return;
            }

            int dano = CalcularDano();
            Console.WriteLine($"\n⚔️ {Nome} ataca {alvo.Nome}!");
            alvo.ReceberDano(dano);
        }

        public void AdicionarOuro(int quantidade)
        {
            Ouro += quantidade;
            Console.WriteLine($"💰 {Nome} ganhou {quantidade} de ouro! Total: {Ouro}");
        }

        public bool GastarOuro(int quantidade)
        {
            if (Ouro >= quantidade)
            {
                Ouro -= quantidade;
                Console.WriteLine($"💸 {Nome} gastou {quantidade} de ouro. Restante: {Ouro}");
                return true;
            }
            else
            {
                Console.WriteLine($"❌ {Nome} não tem ouro suficiente! Necessário: {quantidade}, Tem: {Ouro}");
                return false;
            }
        }

        public void MostrarStatus()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("📋 STATUS DO PERSONAGEM");
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($"👤 Nome: {Nome}");
            Console.WriteLine($"📈 Nível: {Nivel}");
            Console.WriteLine($"❤️ Vida: {Vida}/{VidaMaxima}");
            Console.WriteLine($"⚔️ Ataque: {Ataque}");
            Console.WriteLine($"🛡️ Defesa: {Defesa}");
            Console.WriteLine($"⭐ EXP: {Experiencia}/{(Nivel * 100)}");
            Console.WriteLine($"💰 Ouro: {Ouro}");
            Console.WriteLine($"💀 Estado: {(EstaVivo ? "Vivo ✅" : "Morto 💀")}");
            Console.WriteLine(new string('=', 40));
        }

        public void Reviver()
        {
            if (!EstaVivo)
            {
                Vida = VidaMaxima / 2; 
                Console.WriteLine($"✨ {Nome} foi revivido com {Vida} de vida!");
            }
            else
            {
                Console.WriteLine($"❌ {Nome} já está vivo!");
            }
        }
    
    }
    
}