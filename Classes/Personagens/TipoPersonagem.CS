using System;
using RPG.Classes.Abstracts.Personagens;

namespace Rpg.Classes.Personagens
{
    public enum TipoPersonagem
    {
        CacadorDeTesouros,
        Crocodilo,
        EscorpiaoDeFogo,
        EspiritoDaNevoa,
        EspiritoDoPantano,
        Cavaleiro,
        JacareAncestral,
        LadraoDasSombras,
        LoboDoDeserto,
        LoboSombrio,
        Megaladon,
        OgroCaverna,
        OgroDoPantano,
        OncaPintada,
        Orca,
        PiratasNavioBarbaNegra,
        Piton,
        PlantaDevoraAlmas,
        PovoKraken,
        UrsoNegro,
        ZumbiDoPantano,
        Guerreiro,
        AranhaGigante,
        BruxaDoPantano,

        Henry,
        Dragao
    }

    public static class PersonagemFactory
    {
        public static Personagem Criar(TipoPersonagem tipo, string? nomePersonalizado = null)
        {
            return tipo switch
            {
                TipoPersonagem.Cavaleiro => new Personagem(nomePersonalizado ?? "Cavaleiro", 12, 130, 220),
                TipoPersonagem.CacadorDeTesouros => new Personagem(nomePersonalizado ?? "Caçador de Tesouros", 42, 430, 660),
                TipoPersonagem.Crocodilo => new Personagem(nomePersonalizado ?? "Crocodilo", 28, 290, 450),
                TipoPersonagem.EscorpiaoDeFogo => new Personagem(nomePersonalizado ?? "Escorpião de Fogo", 38, 390, 600),
                TipoPersonagem.EspiritoDaNevoa => new Personagem(nomePersonalizado ?? "Espírito da Névoa", 26, 270, 420),
                TipoPersonagem.EspiritoDoPantano => new Personagem(nomePersonalizado ?? "Espírito do Pântano", 22, 230, 360),
                TipoPersonagem.JacareAncestral => new Personagem(nomePersonalizado ?? "Jacaré Ancestral", 18, 190, 300),
                TipoPersonagem.LadraoDasSombras => new Personagem(nomePersonalizado ?? "Ladrão das Sombras", 14, 150, 240),
                TipoPersonagem.LoboDoDeserto => new Personagem(nomePersonalizado ?? "Lobo do Deserto", 40, 410, 630),
                TipoPersonagem.LoboSombrio => new Personagem(nomePersonalizado ?? "Lobo Sombrio", 4, 60, 100),
                TipoPersonagem.Megaladon => new Personagem(nomePersonalizado ?? "Megaladon", 30, 310, 480),
                TipoPersonagem.OgroCaverna => new Personagem(nomePersonalizado ?? "Ogro da Caverna", 4, 60, 150),
                TipoPersonagem.OgroDoPantano => new Personagem(nomePersonalizado ?? "Ogro do Pântano", 20, 210, 330),
                TipoPersonagem.OncaPintada => new Personagem(nomePersonalizado ?? "Onça Pintada", 8, 90, 150),
                TipoPersonagem.Orca => new Personagem(nomePersonalizado ?? "Orca", 32, 330, 510),
                TipoPersonagem.PiratasNavioBarbaNegra => new Personagem(nomePersonalizado ?? "Piratas - Navio Barba Negra", 34, 350, 540),
                TipoPersonagem.Piton => new Personagem(nomePersonalizado ?? "Piton", 16, 170, 270),
                TipoPersonagem.PlantaDevoraAlmas => new Personagem(nomePersonalizado ?? "Planta Devora-Almas", 10, 110, 180),
                TipoPersonagem.PovoKraken => new Personagem(nomePersonalizado ?? "Povo Kraken", 36, 370, 570),
                TipoPersonagem.UrsoNegro => new Personagem(nomePersonalizado ?? "Urso Negro", 6, 80, 150),
                TipoPersonagem.ZumbiDoPantano => new Personagem(nomePersonalizado ?? "Zumbi do Pântano", 24, 250, 390),
                TipoPersonagem.AranhaGigante => new Personagem(nomePersonalizado ?? "Aranha Gigante",20,180,400),
                TipoPersonagem.BruxaDoPantano => new Personagem(nomePersonalizado ?? "Bruxa do Pântano", 50, 500, 800),
                TipoPersonagem.Dragao => new Personagem(nomePersonalizado ?? "Dragão", 100, 1000, 2000),
                TipoPersonagem.Henry => new Personagem(nomePersonalizado ?? "Henry, o Velho Guerreiro", 150, 1600, 2500),
                _ => throw new ArgumentOutOfRangeException(nameof(tipo), tipo, null)
            };
        }
    }
}
