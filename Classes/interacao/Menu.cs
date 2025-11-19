using System;
using System.Text;
using System;
using System.IO;

  public class Menu
    {
        public static void ExibirMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Menu Principal ===");
            Console.WriteLine("1. digite seu nome");

        }

        public static String Nome()
        {
            Console.Write("Digite seu nome: ");
            string nome = Console.ReadLine();
            return nome;
        }
    }
