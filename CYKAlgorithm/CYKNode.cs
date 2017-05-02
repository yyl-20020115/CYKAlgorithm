using System.Collections.Generic;
using System.Linq;

namespace CYKAlgorithm
{
	public class CYKNode
	{
		public virtual long Offset { get; protected set; } = 0L;

		public virtual CNFProduction Production { get; protected set; }

		public virtual ProductionType Type => this.Production != null ? this.Production.Type : ProductionType.Unknown;

		public virtual string Terminal => this.Production?.Terminal;

		public virtual string Target => this.Production?.Target;

		public virtual string Head => this.Production?.Head;
		public virtual string Tail => this.Production?.Tail;

		public virtual string Single => this.Production?.Single;

		public virtual CYKNode HeadNode { get; protected set; } = null;

		public virtual CYKNode TailNode { get; protected set; } = null;
		
		public virtual CYKNode SingleNode { get; protected set; } = null;

		public virtual CNFProduction GetHead(List<CNFProduction> Productions)
			=> this.Type == ProductionType.TwoNonterminals && Productions != null
			? (from p in Productions where p.Target == this.Head select p).FirstOrDefault()
			: null;
		public virtual CNFProduction GetTail(List<CNFProduction> Productions)
			=> this.Type == ProductionType.TwoNonterminals && Productions != null
			? (from p in Productions where p.Target == this.Tail select p).FirstOrDefault()
			: null;
		public virtual CNFProduction GetSingle(List<CNFProduction> Productions)
			=> this.Type == ProductionType.OneNonterminal && Productions != null
			? (from p in Productions where p.Target == this.Single select p).FirstOrDefault()
			: null;

		public CYKNode(CNFProduction Production, CYKNode HeadNode, CYKNode TailNode, long Offset = 0L)
		{
			this.Production = Production;
			this.HeadNode = HeadNode;
			this.TailNode = TailNode;
			this.Offset = Offset;
		}
		public CYKNode(CNFProduction Production, CYKNode SingleNode, long Offset = 0L)
		{
			this.Production = Production;
			this.SingleNode = SingleNode;
			this.Offset = Offset;
		}
		public CYKNode(CNFProduction Production, long Offset = 0L)
		{
			this.Production = Production;
			this.Offset = Offset;
		}
	}
}
