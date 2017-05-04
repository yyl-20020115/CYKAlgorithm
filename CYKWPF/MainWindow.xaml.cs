﻿using CYKAlgorithm;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace CYKWPF
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{

		//S->AB | BC
		//A->BA | a
		//B->CC | b
		//C->AB | a
		//try: baaba
		List<CNFProduction> Productions = new List<CNFProduction>()
		{
			new CNFProduction("S","A","B"),
			new CNFProduction("S","B","A"),
			new CNFProduction("A","B","A"),
			new CNFProduction("A","a"),
			new CNFProduction("B","C","C"),
			new CNFProduction("B","b"),
			new CNFProduction("C","A","B"),
			new CNFProduction("C","a")
		};

		protected CYKStepper stepper = null;
		protected TextBlock[,] blocks = null;

		public virtual string Input { get; set; } = "baaba";

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			var ret = CYK.Parse(this.Input, this.Productions);

			if (ret.Accepted)
			{
				this.BuildGrid(ret.Matrix);
				this.BuildTree(ret.Nodes);
			}
		}
		protected virtual void BuildTree(List<CYKNode> Nodes)
		{
			if (Nodes != null)
			{
				this.MyTree.Items.Clear();

				foreach(CYKNode node in Nodes)
				{
					this.MyTree.Items.Add(this.BuildTreeItem(node));
				}
			}
		}
		protected virtual TreeViewItem BuildTreeItem(CYKNode node)
		{
			TreeViewItem item = null;

			if (node != null)
			{
			    item = new TreeViewItem() { Header = node.ToString(), Tag = node };

				switch (node.Type)
				{
					case ProductionType.OneTerminal:
						item.Items.Add(new TreeViewItem { Header = node.Terminal, Tag = node });
						break;
					case ProductionType.OneNonterminal:
						item.Items.Add(this.BuildTreeItem(node.SingleNode));
						break;
					case ProductionType.TwoNonterminals:
						item.Items.Add(this.BuildTreeItem(node.HeadNode));
						item.Items.Add(this.BuildTreeItem(node.TailNode));
						break;
				}

			}
			return item;
		}
		protected virtual void BuildGrid(List<CYKNode>[,] matrix)
		{
			this.MyGrid.Children.Clear();
			this.MyGrid.RowDefinitions.Clear();
			this.MyGrid.ColumnDefinitions.Clear();

			if (matrix != null)
			{
				this.blocks = new TextBlock[matrix.GetLength(0), matrix.GetLength(1)];

				for (int row = 0; row < matrix.GetLength(0); row++)
				{
					this.MyGrid.RowDefinitions.Add(new RowDefinition());
				}
				for (int column = 0; column < matrix.GetLength(1); column++)
				{
					this.MyGrid.ColumnDefinitions.Add(new ColumnDefinition());
				}

				for (int row = 0; row < matrix.GetLength(0); row++)
				{
					for (int column = 0; column < matrix.GetLength(1) - row; column++)
					{
						List<CYKNode> nodes = matrix[row, column];

						string text = string.Join(" ", nodes.Select(n => n.Target));

						TextBlock block = new TextBlock() { Text = text };

						block.Background = Brushes.LightGray;

						this.blocks[row, column] = block;

						Grid.SetColumn(block, column);
						Grid.SetRow(block, row);

						this.MyGrid.Children.Add(block);

					}
				}
			}
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if(this.stepper!=null && this.stepper.IsDone)
			{

				this.stepper = null;

				this.blocks = null;
			}
			if(this.stepper == null)
			{
				this.stepper = new CYKStepper();

				this.BuildGrid(this.stepper.Init(this.Input, this.Productions));

			}
			else
			{
				if (!this.stepper.Step(this.OnReport))
				{
					var ret = this.stepper.GetResult();

					if (ret.Accepted)
					{
						this.BuildTree(ret.Nodes);
					}
					MessageBox.Show("Accepted = " + ret.Accepted);

				}
			}
		}

		protected virtual void OnReport
		(
			(List<CYKNode> L, int row, int column) M,
			(List<CYKNode> L, int row, int column) B,
			(List<CYKNode> L, int row, int column) C
		)
		{
			this.ClearColors();

			TextBlock current = this.blocks[M.row, M.column];

			current.Background = Brushes.Green;

			current.Text = string.Join(" ", M.L.Select(n => n.Target));

			TextBlock b = this.blocks[B.row, B.column];

			b.Background = Brushes.Blue;

			b.Text = string.Join(" ", B.L.Select(n => n.Target));

			TextBlock c = this.blocks[C.row, C.column];

			c.Background = Brushes.Yellow;

			c.Text = string.Join(" ", C.L.Select(n => n.Target));
		}

		protected virtual void ClearColors()
		{
			if (this.blocks != null)
			{
				for (int row = 0; row < blocks.GetLength(0); row++)
				{
					for (int column = 0; column < blocks.GetLength(1) - row; column++)
					{

						TextBlock block = this.blocks[row, column];

						block.Background = Brushes.LightGray;

					}
				}
			}
		}
	}
}
