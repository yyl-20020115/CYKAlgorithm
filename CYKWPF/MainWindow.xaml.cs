using CYKAlgorithm;
using System.Collections.Generic;
using System.Windows;

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
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{

		}
	}
}
