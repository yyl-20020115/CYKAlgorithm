using System.Collections.Generic;
using System.Linq;

namespace CYKAlgorithm
{
	public class CYK
	{
		public static (List<CYKNode>[,] Matrix,List<CYKNode> Nodes, bool Accepted) Parse(string Input, List<CNFProduction> Productions)
		{
			return Parse(Input?.ToCharArray(), Productions);
		}
		public static (List<CYKNode>[,] Matrix, List<CYKNode> Nodes, bool Accepted) Parse(IEnumerable<char> Input, List<CNFProduction> Productions)
		{
			return Parse(Input?.Select(c=>c.ToString()), Productions);
		}
		public static (List<CYKNode>[,] Matrix, List<CYKNode> Nodes, bool Accepted) Parse(IEnumerable<string> Input, List<CNFProduction> Productions)
		{
			return Parse(Input?.ToArray(), Productions);
		}
		public static (List<CYKNode>[,] Matrix, List<CYKNode> Nodes, bool Accepted) Parse(string[] Input, List<CNFProduction> Productions)
		{
			(List<CYKNode>[,] Matrix,List<CYKNode> Nodes, bool Accepted) ret = (null,null, false);

			if ((Input != null && Input.Length > 0) && Productions != null && Productions.Count > 0)
			{
				long offset = 0L;

				int InputLength = Input.Length;

				List<CYKNode>[,] Matrix = new List<CYKNode>[InputLength, InputLength];

				ret.Matrix = Matrix;

				for (int row = 0; row < InputLength; row++)
				{
					for (int column = 0; column < InputLength; column++)
					{
						Matrix[row, column] = new List<CYKNode>();

						if (row == 0)
						{
							Matrix[row, column].AddRange(
								from p in Productions
								where p.Type == ProductionType.OneTerminal
								&& p.Terminal == Input[column]
								select new CYKNode(p,
									(offset += Input[column].Length) - Input[column].Length)
							);
							if (Matrix[row, column].Count == 0)
							{
								return ret;
							}
						}
					}
				}
				/**
				 *    
				 * row 0 *  *  *  *  *  *  * 
				 * row 1 *  *  *  *  *  *
				 * row 2 *  *  *  *  *
				 * row 3 *  *  *  *
				 * 
				 */
				for (int row = 1; row < InputLength; row++)
				{
					for (int column = 0; column < InputLength - row; column++)
					{
						// {Xi1,Xj1} U {Xi2,Xj2} == Xi1Xi2 Xi1Xj2 Xj1Xi2 Xj1Xj2
						int i1 = row - 1;
						int j1 = column;

						int i2 = 0;
						int j2 = row + column;

						for (int cx = 0; cx < row; cx++)
						{
							List<CYKNode> B = Matrix[i1, j1];
							List<CYKNode> C = Matrix[i2, j2];
							List<CYKNode> M = Matrix[row, column];

							i1--;
							i2++;
							j2--;

							if (B.Count == 0 && C.Count == 0)
							{

							}
							else if (B.Count > 0 && C.Count > 0)
							{
								M.AddRange(
									from b in B
									from c in C
									from p in Productions
									where
									p.Type == ProductionType.TwoNonterminals
									&& p.Head == b.Target
									&& p.Tail == c.Target
									select new CYKNode(p, b, c)
									);
							}
							else
							{
								List<CYKNode> D = B.Count > 0 ? B : C;
								M.AddRange(
									from d in D
									from p in Productions
									where
									p.Type == ProductionType.OneNonterminal
									&& p.Single == d.Target
									select new CYKNode(p, d)
									);
							}
						}
					}
				}

				ret.Nodes = Matrix[InputLength - 1, 0];
				if (ret.Nodes
					.Any(node => node.Production == Productions[0]))
				{
					ret.Accepted = true;
				}
			}
			return ret;
		}
	}
}
