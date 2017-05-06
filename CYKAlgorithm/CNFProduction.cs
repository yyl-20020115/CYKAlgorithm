using System;

namespace CYKAlgorithm
{
	public class CNFProduction
	{
		public virtual CNFProductionType Type { get; protected set; } = CNFProductionType.Unknown;
		public virtual string Target { get; protected set; } = string.Empty;

		public virtual string Head { get; protected set; } = string.Empty;

		public virtual string Tail { get; protected set; } = string.Empty;

		public virtual string Single { get; protected set; } = string.Empty;

		public virtual string Terminal { get; protected set; } = string.Empty;

		public CNFProduction(string Target, string Text, bool IsTerminal = true)
		{
			this.Target = Target ?? throw new ArgumentNullException(nameof(Target));

			if (IsTerminal)
			{
				this.Type = CNFProductionType.OneTerminal;
				this.Terminal = Text ?? throw new ArgumentNullException(nameof(Text));
			}
			else
			{
				this.Type = CNFProductionType.OneNonterminal;
				this.Single = Text ?? throw new ArgumentNullException(nameof(Text));
			}
		}
		public CNFProduction(string Target, string Head, string Tail)
		{
			this.Type = CNFProductionType.TwoNonterminals;
			this.Target = Target ?? throw new ArgumentNullException(nameof(Target));
			this.Head = Head ?? throw new ArgumentNullException(nameof(Head));
			this.Tail = Tail ?? throw new ArgumentNullException(nameof(Tail));
		}
		public override string ToString()
		{
			if (this.Type == CNFProductionType.OneTerminal)
			{
				return this.Target + " = \"" + this.Terminal + "\"";
			}
			else if (this.Type == CNFProductionType.TwoNonterminals)
			{
				return this.Target + " = " + this.Head + " " + this.Tail;
			}
			else if(this.Type == CNFProductionType.OneNonterminal)
			{
				return this.Target + " = " + this.Single;
			}
			else
			{
				return this.Type.ToString();
			}
		}
	}
}
