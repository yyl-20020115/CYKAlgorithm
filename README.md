
CYKAlgorithm in C#

by Yilin Yang


	public static partial class CYK
	{
		public static List<CYKNode> Parse(string[] Input, List<CNFProduction> Productions)
		{
			List<CYKNode> Finals = new List<CYKNode>();
			if ((Input != null && Input.Length > 0) && Productions != null && Productions.Count > 0)
			{
				int InputLength = Input.Length;

				List<CYKNode>[,] Matrix = new List<CYKNode>[InputLength, InputLength];

				for (int row = 0; row < InputLength; row++)
				{
					for (int column = 0; column < InputLength; column++)
					{
						Matrix[row, column] = new List<CYKNode>();

						if (row == 0)
						{
							Matrix[row, column].AddRange(
								CYKNode.ToNodes(
								from p in Productions
								where p.Type == ProductionType.OneTerminal
								&& p.Terminal == Input[column]
								select p)
							);
							if (Matrix[row, column].Count == 0)
							{
								return Finals;
							}
						}
					}
				}

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

							i1--;
							i2++;
							j2--;
						}
					}
				}

				Finals = Matrix[InputLength - 1, 0];
				if (!Finals
					.Any(node => node.Production == Productions[0]))
				{
					Finals.Clear();
				}
			}
			return Finals;
		}


	}
