using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
	public class Urso : Personagem
	{

		public Urso(string nome = "Urso")
			: base(nome, nivel: 6, ataque: 80, defesa: 150)
		{
		}

	}
}