using MultipleChoice.CustomComponent;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultipleChoice
{
    /// <summary>
    /// Interaction logic for AccountDetailPage.xaml
    /// </summary>
    public partial class AccountDetailPage : Page
    {
        private readonly UserService _userService = new UserService();
        private readonly AttempServices _attempServices = new AttempServices();
        private User _user { get; set; }
        public AccountDetailPage()
        {
            InitializeComponent();
            _user = _userService.GetById(MenuWindow.UserId);
            InitializeFetchingData();
            List<dynamic> userAttempts = LoadUserAttemptInfo();
            attemptsDataGrid.ItemsSource = userAttempts;

            QuizzAnalyzerUserControl analyzer = new QuizzAnalyzerUserControl();
            analyzer.SetData(50, 10, 15, 5, 20); // 50 quiz với 4 mức độ
            AnalyzerBox.Children.Add(analyzer);

        }

        public void InitializeFetchingData()
        {
            TxtUsername.Text = _user.Username;
            TxtEmail.Text = _user.Email;
        }

        public List<dynamic> LoadUserAttemptInfo()
        {
            List<UserAttempt> userAttempts =  _attempServices.GetAttemptsByUserID(MenuWindow.UserId);
            List<dynamic> newUserAttemps = new List<dynamic>();
            foreach (var userAttempt in userAttempts)
            {
                var newUserAttempt = new
                {
                    userAttempt.Quizz,
                    userAttempt.Score,
                    userAttempt.Time,
                    Date = userAttempt.StartAt.ToString("dd/MM/yyyy"),
                    AtTime = userAttempt.StartAt.ToString("HH:mm"),
                    userAttempt.IsCompleted,
                };
                newUserAttemps.Add(newUserAttempt);
            }
            return newUserAttemps;
        }
    }
}
