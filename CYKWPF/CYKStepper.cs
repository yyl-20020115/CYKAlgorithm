using CYKAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CYKWPF
{
	public class CYKStepper : CYK
	{
		public delegate void StepReport
		(
			int row,
			int column,
			int i1,
			int j1,
			int i2,
			int j2,
			List<CYKNode> B,
			List<CYKNode> C,
			List<CYKNode> M
		);

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

		public CYKStepper()
		{

		}

		public virtual void Init(string[] Input, List<CNFProduction> Productions)
		{
			this.productions = Productions ?? throw new ArgumentNullException(nameof(Productions));
			if (this.productions.Count == 0) throw new ArgumentException("productions count can not be zero", nameof(Productions));
			this.input = Input ?? throw new ArgumentNullException(nameof(Input));
			if ((this.inputLength = this.input.Length) == 0) throw new ArgumentException("input length can not be zero", nameof(input));

			this.offset = 0L;

			this.matrix = new List<CYKNode>[InputLength, InputLength];

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
		}

		public virtual bool NextStep(StepReport report = null)
		{
			if (this.matrix != null)
			{
				this.StepAction(report);

				if (++this.cx == this.row)
				{
					this.cx = 0;

					if (++this.column == this.InputLength - this.row)
					{
						this.column = 0;

						if (++this.row == this.InputLength)
						{
							//done
							return false;
						}
						this.i1 = this.row - 1;
						this.j1 = this.column;

						this.i2 = 0;
						this.j2 = this.row + this.column;
					}
				}
				return true;
			}
			return false;
		}

		protected virtual void StepAction(StepReport report)
		{
			if (this.Matrix != null)
			{
				List<CYKNode> B = Matrix[this.i1, this.j1];
				List<CYKNode> C = Matrix[this.i2, this.j2];
				List<CYKNode> M = Matrix[this.row, this.column];


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
				report?.Invoke(this.row, this.column, this.i1, this.j1, this.i2, this.j2, B, C, M);

				this.i1--;
				this.i2++;
				this.j2--;
			}
		}

		public virtual (List<CYKNode> Nodes, bool Accepted) GetResult()
		{
			(List<CYKNode> Nodes, bool Accepted) ret =
				(null, false);
			if (this.matrix != null)
			{
				ret.Nodes = Matrix[InputLength - 1, 0];

				if (ret.Nodes
					.Any(node => node.Production == this.Productions[0]))
				{
					ret.Accepted = true;
				}
			}

			return ret;
		}
	}
}
