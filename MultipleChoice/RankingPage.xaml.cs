using MultipleChoice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace MultipleChoice
{
    /// <summary>
    /// Interaction logic for RankingPage.xaml
    /// </summary>
    public partial class RankingPage : Page
    {
        public int QuizzId { get; set; }
        public RankingPage(int quizzId)
        {
            InitializeComponent();
            QuizzId = quizzId;
            UpdateRow();
        }

        public void UpdateRow()
        {
            List<Attemp> attemps = new List<Attemp>();
            // Clear tất cả Row cũ (trừ tiêu đề)
            while (RankingTableGrid.RowDefinitions.Count > 2)
            {
                RankingTableGrid.RowDefinitions.RemoveAt(RankingTableGrid.RowDefinitions.Count - 1);
            }
            RankingTableGrid.Children.RemoveRange(4, RankingTableGrid.Children.Count - 4); // Giữ lại tiêu đề

            if (attemps.Count == 0)
            {
                int newRowIndex = RankingTableGrid.RowDefinitions.Count;
                RankingTableGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var border = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(5)
                };

                var textBlock = new TextBlock
                {
                    Text = "No records",
                    FontSize = 24,
                    FontWeight = FontWeights.SemiBold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10)
                };

                border.Child = textBlock;

                Grid.SetRow(border, newRowIndex);
                Grid.SetColumn(border, 0);
                Grid.SetColumnSpan(border, 4); // Chiếm 4 cột

                RankingTableGrid.Children.Add(border);
            }
            else
            {
                // Load dữ liệu thật
                int rank = 1;
                foreach (var item in attemps)
                {
                    //AddRankingRow(rank++, item.User?.Username ?? "Unknown", item.Score, item.CreatedAt?.ToString("HH:mm:ss") ?? "");
                }
            }
        }

        public void AddRankingRow(int rank, string username, int score, string time)
        {
            int newRowIndex = RankingTableGrid.RowDefinitions.Count;

            // Thêm hàng mới
            RankingTableGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Tạo từng ô trong hàng
            AddCellToGrid(RankingTableGrid, newRowIndex, 0, rank.ToString());
            AddCellToGrid(RankingTableGrid, newRowIndex, 1, username);
            AddCellToGrid(RankingTableGrid, newRowIndex, 2, score.ToString());
            AddCellToGrid(RankingTableGrid, newRowIndex, 3, time);
        }

        private void AddCellToGrid(Grid grid, int row, int column, string text)
        {
            var border = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1)
            };

            var textBlock = new TextBlock
            {
                Text = text,
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            border.Child = textBlock;

            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
            grid.Children.Add(border);
        }
    }
}
