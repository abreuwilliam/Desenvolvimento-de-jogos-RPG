using System;
using System.IO;
using System.Threading;
using RPG.Classes.Abstracts.Personagens;

public class Combate
{
    private Personagem _protagonista;
    private Personagem _vilao;

    public Combate(Personagem protagonista, Personagem vilao)
    {
        _protagonista = protagonista;
        _vilao = vilao;
    }

    public void Iniciar()
    {
        var audio = new AudioPlayer();
        var musicPath = Path.Combine(AppContext.BaseDirectory, "Assets", "combate.mp3");


        audio.PlayLoop(musicPath);
         

            while (_protagonista.EstaVivo && _vilao.EstaVivo)
            {
                _protagonista.AtacarAlvo(_vilao);
                if (!_vilao.EstaVivo) { break; }

              
                _vilao.AtacarAlvo(_protagonista);
                if (!_protagonista.EstaVivo) break;
            }

            if (_protagonista.EstaVivo && !_vilao.EstaVivo)
            {
                Console.WriteLine($"vc venceu o combate!");
                _vilao.ConcederRecompensa(_protagonista);
            }
            else
            {
                Console.WriteLine("vc perdeu o combate.");
            }
        }
    }

