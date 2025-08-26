using System;

namespace Rpg.Principal.Abstracts
{
    public class Personagem
    {
        // Propriedades básicas
        public string Nome { get; private set; }
        public int Nivel { get; private set; }
        public int Vida { get; private set; }
        public int VidaMaxima { get; private set; }
        public int Ataque { get; private set; }
        public int Defesa { get; private set; }
        public int Experiencia { get; private set; }
        public int Ouro { get; private set; }
        public bool EstaVivo => Vida > 0;

        // Construtor
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
            Ataque = 15 + (Nivel * 2);
            Defesa = 5 + Nivel;
        }

        // Método para receber dano
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
            int ouroGanho = Nivel * 5;

            agressor.AdicionarExperiencia(expGanho);
            agressor.AdicionarOuro(ouroGanho);
            Console.WriteLine($"🎉 {agressor.Nome} derrotou {Nome} e ganhou {expGanho} de EXP e {ouroGanho} de ouro!");
        }

        // Método para curar
        public void Curar(int quantidade)
        {
            int vidaAntes = Vida;
            Vida = Math.Min(Vida + quantidade, VidaMaxima);
            int vidaCurada = Vida - vidaAntes;

            Console.WriteLine($"❤️ {Nome} recuperou {vidaCurada} de vida!");
        }

        // Método para adicionar experiência
        public void AdicionarExperiencia(int exp)
        {
            Experiencia += exp;
            Console.WriteLine($"⭐ {Nome} ganhou {exp} pontos de experiência!");
            
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
            
            InicializarAtributos(); // Recalcula atributos com novo nível
            Vida = VidaMaxima; // Cura completamente
            
            Console.WriteLine($"\n🎉 {Nome} subiu para o nível {Nivel}!");
            Console.WriteLine($"📊 Vida: {vidaAntes} → {VidaMaxima}");
            Console.WriteLine($"⚔️ Ataque: {Ataque - 2} → {Ataque}");
            Console.WriteLine($"🛡️ Defesa: {Defesa - 1} → {Defesa}");
        }

        // Método para calcular dano do ataque
        public int CalcularDano()
        {
            Random random = new Random();
            int danoBase = Ataque;
            int bonusAleatorio = random.Next(1, 6); // Dado de 1 a 5
            return danoBase + bonusAleatorio;
        }

        // Método para atacar outro personagem
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

        // Método para adicionar ouro
        public void AdicionarOuro(int quantidade)
        {
            Ouro += quantidade;
            Console.WriteLine($"💰 {Nome} ganhou {quantidade} de ouro! Total: {Ouro}");
        }

        // Método para gastar ouro
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

        // Método para mostrar status completo
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

        // Método para reviver personagem
        public void Reviver()
        {
            if (!EstaVivo)
            {
                Vida = VidaMaxima / 2; // Revive com metade da vida
                Console.WriteLine($"✨ {Nome} foi revivido com {Vida} de vida!");
            }
            else
            {
                Console.WriteLine($"❌ {Nome} já está vivo!");
            }
        }
    }
}
