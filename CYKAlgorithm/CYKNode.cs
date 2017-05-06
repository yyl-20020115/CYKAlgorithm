using System;
using System.Collections.Generic;
using System.Linq;

namespace CYKAlgorithm
{
	public class CYKNode
	{
		public virtual long Offset { get; protected set; } = 0L;

		public virtual CNFProduction Production { get; protected set; }

		public virtual CNFProductionType Type => this.Production != null ? this.Production.Type : CNFProductionType.Unknown;

		public virtual string Terminal => this.Production?.Terminal;

		public virtual string Target => this.Production?.Target;

		public virtual string Head => this.Production?.Head;
		public virtual string Tail => this.Production?.Tail;

		public virtual string Single => this.Production?.Single;

		public virtual CYKNode HeadNode { get; protected set; } = null;

		public virtual CYKNode TailNode { get; protected set; } = null;
		
		public virtual CYKNode SingleNode { get; protected set; } = null;

		public virtual CNFProduction GetHead(List<CNFProduction> Productions)
			=> this.Type == CNFProductionType.TwoNonterminals && Productions != null
			? (from p in Productions where p.Target == this.Head select p).FirstOrDefault()
			: null;
		public virtual CNFProduction GetTail(List<CNFProduction> Productions)
			=> this.Type == CNFProductionType.TwoNonterminals && Productions != null
			? (from p in Productions where p.Target == this.Tail select p).FirstOrDefault()
			: null;
		public virtual CNFProduction GetSingle(List<CNFProduction> Productions)
			=> this.Type == CNFProductionType.OneNonterminal && Productions != null
			? (from p in Productions where p.Target == this.Single select p).FirstOrDefault()
			: null;

		public CYKNode(CNFProduction Production, CYKNode HeadNode, CYKNode TailNode, long Offset = 0L)
		{
			this.Production = Production ?? throw new ArgumentNullException(nameof(Production));
			this.HeadNode = HeadNode ?? throw new ArgumentNullException(nameof(HeadNode));
			this.TailNode = TailNode ?? throw new ArgumentNullException(nameof(TailNode));
			this.Offset = Offset;
		}
		public CYKNode(CNFProduction Production, CYKNode SingleNode, long Offset = 0L)
		{
			this.Production = Production ?? throw new ArgumentNullException(nameof(Production));
			this.SingleNode = SingleNode ?? throw new ArgumentNullException(nameof(SingleNode));
			this.Offset = Offset;
		}
		public CYKNode(CNFProduction Production, long Offset = 0L)
		{
			this.Production = Production ?? throw new ArgumentNullException(nameof(Production));
			this.Offset = Offset;
		}
		public override string ToString()
		{
			return this.Production.ToString();
		}
	}
}
