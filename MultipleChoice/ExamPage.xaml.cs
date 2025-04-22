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
    /// Interaction logic for ExamPage.xaml
    /// </summary>
    public partial class ExamPage : Page
    {
        int quizzID;
        int timeLimit;
        bool IsResultShowable;
        private QuizzService _quizzService = new QuizzService();
        private UserService _userService = new UserService();

        public ExamPage(int quizzID)
        {
            InitializeComponent();
            this.quizzID = quizzID;
            Quizz quizz = _quizzService.GetById(quizzID);
            CreatedByText.Text = _userService.GetById(quizz.CreatedBy).Username.ToString();
            TypeText.Text = quizz.Type.ToString();
            AttemptText.Text = quizz.AttempNumber.ToString();
            TimeLimitText.Text = quizz.TimeLimit.ToString();
            StartAtText.Text = quizz.StartAt.ToString();
            ResultShowableText.Text = quizz.IsResultShowable ? "allow" : "not allow";
            this.timeLimit = (int)(quizz.TimeLimit?.TotalMinutes ?? 0);

            this.IsResultShowable = quizz.IsResultShowable;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            QuizzTakingWindows quizzTakingWindows = new QuizzTakingWindows(this.quizzID, (MenuWindow)parent, this.timeLimit, this.IsResultShowable);
            quizzTakingWindows.Show();
            parent.Hide();
        }
    }
}
