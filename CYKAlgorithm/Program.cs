using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CYKAlgorithm
{
	class Program
	{
		static void Main(string[] args)
		{
			//S->AB | BC
			//A->BA | a
			//B->CC | b
			//C->AB | a
			//try: baaba
			List<CNFProduction> Productions = new List<CNFProduction>()
			{
				new CNFProduction("S","A","B"),
				new CNFProduction("S","B","A"),
				new CNFProduction("A","B","A"),
				new CNFProduction("A","a"),
				new CNFProduction("B","C","C"),
				new CNFProduction("B","b"),
				new CNFProduction("C","A","B"),
				new CNFProduction("C","a")
			};

			string Input = "b a a b a";

			//while ((Input = Console.ReadLine()) != null && Input.Length > 0)
			{
				List<CYKNode> Finals = CYK.Parse(Input.Split(' '), Productions);
				if (Finals.Count>0)
				{
					Console.WriteLine("Input is accepted");
				}
				else
				{
					Console.WriteLine("Input is rejected");
				}
			}
			Console.ReadKey();
		}
	}
}
