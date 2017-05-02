using System.Collections.Generic;
using System.Linq;

namespace CYKAlgorithm
{
	public class CYKNode
	{
		public static IEnumerable<CYKNode> ToNodes(IEnumerable<CNFProduction> Productions)
		{
			foreach (CNFProduction p in Productions)
			{
				if (p != null)
				{
					yield return new CYKNode(p);
				}
			}
		}
		public virtual long Offset { get; protected set; } = 0L;

		public virtual CNFProduction Production { get; protected set; }

		public virtual ProductionType Type => this.Production != null ? this.Production.Type : ProductionType.Unknown;

		public virtual string Terminal => this.Production?.Terminal;

		public virtual string Target => this.Production?.Target;

		public virtual string Head => this.Production?.Head;
		public virtual string Tail => this.Production?.Tail;

		public virtual CYKNode HeadNode { get; protected set; } = null;

		public virtual CYKNode TailNode { get; protected set; } = null;

		public virtual CNFProduction GetHead(List<CNFProduction> Productions)
			=> this.Type == ProductionType.TwoNonterminals && Productions != null
			? (from p in Productions where p.Target == this.Head select p).FirstOrDefault()
			: null;
		public virtual CNFProduction GetTail(List<CNFProduction> Productions)
			=> this.Type == ProductionType.TwoNonterminals && Productions != null
			? (from p in Productions where p.Target == this.Tail select p).FirstOrDefault()
			: null;

		public CYKNode(CNFProduction Production, CYKNode HeadNode = null, CYKNode TailNode = null, long Offset = 0L)
		{
			this.Production = Production;
			this.HeadNode = HeadNode;
			this.TailNode = TailNode;
			this.Offset = Offset;
		}
	}
}
