namespace CYKAlgorithm
{
	public class CNFProduction
	{
		public virtual ProductionType Type { get; protected set; } = ProductionType.Unknown;
		public virtual string Target { get; protected set; } = string.Empty;

		public virtual string Head { get; protected set; } = string.Empty;

		public virtual string Tail { get; protected set; } = string.Empty;

		public virtual string Single { get; protected set; } = string.Empty;

		public virtual string Terminal { get; protected set; } = string.Empty;

		public CNFProduction(string Target, string Text, bool IsTerminal = true)
		{
			this.Target = Target;

			if (IsTerminal)
			{
				this.Type = ProductionType.OneTerminal;
				this.Terminal = Text;
			}
			else
			{
				this.Type = ProductionType.OneNonterminal;
				this.Single = Text;
			}
		}
		public CNFProduction(string Target, string Head, string Tail)
		{
			this.Type = ProductionType.TwoNonterminals;
			this.Target = Target;
			this.Head = Head;
			this.Tail = Tail;
		}
		public override string ToString()
		{
			if (this.Type == ProductionType.OneTerminal)
			{
				return this.Target + " = \"" + this.Terminal + "\"";
			}
			else if (this.Type == ProductionType.TwoNonterminals)
			{
				return this.Target + " = " + this.Head + " " + this.Tail;
			}
			else if(this.Type == ProductionType.OneNonterminal)
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
