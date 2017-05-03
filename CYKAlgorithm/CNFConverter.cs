using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYKAlgorithm
{
	public class CNFConverter
	{
		public static string NewName => $"#{Guid.NewGuid().ToString()}";

		public static List<CNFProduction> ConvertToCNF(
			IEnumerable<(string Target, (string Text, bool IsTerminal)[] Tokens)> Productions)
		{
			List<CNFProduction> Ret = new List<CNFProduction>();

			if (Productions != null)
			{
				foreach (var Production in Productions)
				{
					if (Production.Tokens != null && Production.Tokens.Length > 0)
					{

						Queue<string> Nonterminals = new Queue<string>();
						Dictionary<string, string> Terminals = new Dictionary<string, string>();

						for (int i = 0; i < Production.Tokens.Length; i++)
						{
							var Token = Production.Tokens[i];
							if (Token.IsTerminal)
							{
								string Name = NewName;

								Nonterminals.Enqueue(Name);
								Terminals.Add(Token.Text, Name);
							}
							else
							{
								Nonterminals.Enqueue(Token.Text);
							}
						}
						foreach (var p in Terminals)
						{
							Ret.Add(new CNFProduction(p.Value, p.Key, true));
						}

						if (Nonterminals.Count > 0)
						{
							if (Nonterminals.Count == 1)
							{
								Ret.Add(new CNFProduction(
									Production.Target,
									Nonterminals.Dequeue(),
									false));
							}
							else
							{
								string H = Nonterminals.Dequeue();

								while (Nonterminals.Count > 0)
								{
									string T = Nonterminals.Dequeue();
									string N = Nonterminals.Count == 0
										? Production.Target : NewName;

									Ret.Add(new CNFProduction(N, H, T));

									H = N;
								}
							}
						}
					}
				}
			}
			return Ret;
		}
	}
}
