using System;
using System.Text;
using System.IO;

public class Menu
{
    private static AudioPlayer audioPlayer;
    private static string nome;  // ← variável nome armazenada aqui

    public static void ExibirMenu()
    {
        audioPlayer = new AudioPlayer();
        audioPlayer.PlayLoop("menu.mp3");

        Console.Clear();
        Console.WriteLine("=== Menu Principal ===");
        Console.Write("\nDigite seu nome: ");
        nome = Console.ReadLine();   

        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();

        audioPlayer.Stop(); 
    }

    public static string Nome()
    {
        return nome;   
    }
}
