using MultipleChoice.Models;
using MultipleChoice.Services;
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
using System.Windows.Shapes;

namespace MultipleChoice
{
    /// <summary>
    /// Interaction logic for AttemptResultWindow.xaml
    /// </summary>
    public partial class AttemptResultWindow : Window
    {
        AttemptInfo? attemptInfo;
        AttempServices services;
        MenuWindow menu;
        public AttemptResultWindow(int attempID)
        {
            InitializeComponent();
            services = new AttempServices();
            attemptInfo = services.GetAttemptInfoByID(attempID);
            MenuWindow? menu = Application.Current.Properties["MenuWindow"] as MenuWindow;
            // Bind dữ liệu vào Grid
            BindDataToGrid();
        }

        private void BindDataToGrid()
        {
            if (attemptInfo != null)
            {
                // Gán giá trị vào các TextBlock trong Grid
                AnswerBy.Text = attemptInfo.Username;
                QuizzName.Text = attemptInfo.Title;
                CorrectNumber.Text = attemptInfo.CorrectNumber.ToString();
                Time.Text = attemptInfo.Time.ToString();
                IsCompelete.Text = attemptInfo.Complete ? "Yes" : "No";  // Hoặc có thể hiển thị "Complete" nếu cần
            }
            else
            {
                MessageBox.Show("Attempt data not found.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            menu.Show();
        }
    }

}
