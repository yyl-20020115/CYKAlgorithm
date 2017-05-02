namespace CYKAlgorithm
{
	public class CNFProduction
	{
		public virtual ProductionType Type { get; protected set; } = ProductionType.Unknown;
		public virtual string Target { get; protected set; } = string.Empty;

		public virtual string Head { get; protected set; } = string.Empty;
		public virtual string Tail { get; protected set; } = string.Empty;

		public virtual string Terminal { get; protected set; } = string.Empty;

		public CNFProduction(string Target, string Terminal)
		{
			this.Type = ProductionType.OneTerminal;
			this.Target = Target;
			this.Terminal = Terminal;
		}
		public CNFProduction(string Target, string Head,string Tail)
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
			else
			{
				return this.Type.ToString();
			}
		}
	}
}
