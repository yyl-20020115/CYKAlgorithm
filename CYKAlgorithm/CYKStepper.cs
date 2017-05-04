using System;
using System.Collections.Generic;
using System.Linq;

namespace CYKAlgorithm
{
	public class CYKStepper : CYK
	{
		public delegate void StepReport
		(
			(List<CYKNode> L, int row, int column) M,
			(List<CYKNode> L, int row, int column) B,
			(List<CYKNode> L, int row, int column) C
		);

		protected bool done = false;

		protected int row = 0;

		protected int column = 0;

		protected int i1 = 0;

		protected int j1 = 0;

		protected int i2 = 0;

		protected int j2 = 0;

		protected int cx = 0;

		protected long offset = 0L;

		protected string[] input = null;

		protected int inputLength = -1;

		protected List<CYKNode>[,] matrix = null;

		protected List<CNFProduction> productions = null;

		public virtual int Row => this.row;

		public virtual int Column => this.column;

		public virtual int I1 => this.i1;

		public virtual int J1 => this.j1;

		public virtual int I2 => this.i2;

		public virtual int J2 => this.j2;

		public virtual long Offset => this.offset;

		public virtual string[] Input => this.input;

		public virtual int InputLength => this.inputLength;

		public virtual List<CYKNode>[,] Matrix => this.matrix;

		public virtual List<CNFProduction> Productions => this.productions;

		public virtual bool IsDone => this.done;

		public CYKStepper()
		{

		}

		public virtual List<CYKNode>[,] Init(string Input, List<CNFProduction> Productions)
		{
			return Init(Input?.ToCharArray(), Productions);
		}
		public virtual List<CYKNode>[,] Init(IEnumerable<char> Input, List<CNFProduction> Productions)
		{
			return Init(Input?.Select(c => c.ToString()), Productions);
		}
		public virtual List<CYKNode>[,] Init(IEnumerable<string> Input, List<CNFProduction> Productions)
		{
			return Init(Input?.ToArray(), Productions);
		}
		public virtual List<CYKNode>[,] Init(string[] Input, List<CNFProduction> Productions)
		{
			this.productions = Productions ?? throw new ArgumentNullException(nameof(Productions));
			if (this.productions.Count == 0) throw new ArgumentException("productions count can not be zero", nameof(Productions));
			this.input = Input ?? throw new ArgumentNullException(nameof(Input));
			if ((this.inputLength = this.input.Length) == 0) throw new ArgumentException("input length can not be zero", nameof(input));

			this.offset = 0L;

			this.matrix = new List<CYKNode>[this.inputLength, this.inputLength];

			for (int row = 0; row < this.inputLength; row++)
			{
				for (int column = 0; column < this.inputLength; column++)
				{
					this.matrix[row, column] = new List<CYKNode>();

					if (row == 0)
					{
						this.matrix[row, column].AddRange(
							from p in this.productions
							where p.Type == ProductionType.OneTerminal
							&& p.Terminal == Input[column]
							select new CYKNode(p,
								(offset += Input[column].Length) - Input[column].Length)
						);
						if (this.matrix[row, column].Count == 0)
						{
							throw new InvalidOperationException($"{Input[column]} is not a valid terminal");
						}
					}
				}
			}

			this.row = 1;
			this.column = 0;

			this.i1 = this.row - 1;
			this.j1 = this.column;

			this.i2 = 0;
			this.j2 = this.row + this.column;

			this.cx = 0;

			this.done = false;

			return this.matrix;
		}

		public virtual bool Step(StepReport report = null)
		{
			if (this.matrix == null) throw new InvalidOperationException("call init first");
			if (this.done) return false;

			this.Reduce(report);

			this.i1--;
			this.i2++;
			this.j2--;

			this.cx++;

			if (this.cx >= this.row)
			{
				this.cx = 0;

				this.column++;

				this.UpdateIJ();

			}

			if (this.column >= this.inputLength - this.row)
			{
				this.column = 0;

				this.row++;

				this.UpdateIJ();
			}

			if (this.row >= this.inputLength)
			{
				this.done = true;

				return false;
			}


			return true;
		}
		protected virtual void UpdateIJ()
		{
			this.i1 = this.row - 1;
			this.j1 = this.column;

			this.i2 = 0;
			this.j2 = this.row + this.column;
		}

		protected virtual void Reduce(StepReport report)
		{
			if (this.matrix != null)
			{
				List<CYKNode> B = this.matrix[this.i1, this.j1];
				List<CYKNode> C = this.matrix[this.i2, this.j2];
				List<CYKNode> M = this.matrix[this.row, this.column];

				if (B.Count == 0 && C.Count == 0)
				{

				}
				else if (B.Count > 0 && C.Count > 0)
				{
					M.AddRange(
						from b in B
						from c in C
						from p in this.productions
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
						from p in this.productions
						where
						p.Type == ProductionType.OneNonterminal
						&& p.Single == d.Target
						select new CYKNode(p, d)
						);
				}
				report?.Invoke((M,this.row, this.column),(B, this.i1, this.j1),(C, this.i2, this.j2));
			}
		}

		public virtual (List<CYKNode>[,] Matrix,List<CYKNode> Nodes, bool Accepted) GetResult()
		{
			(List<CYKNode>[,] Matrix,List<CYKNode> Nodes, bool Accepted) ret = (this.matrix, null, false);

			if (this.matrix != null)
			{
				ret.Nodes = this.matrix[this.inputLength - 1, 0];

				if (ret.Nodes
					.Any(node => node.Production == this.productions[0]))
				{
					ret.Accepted = true;
				}
			}

			return ret;
		}
	}
}
