﻿using MultipleChoice.Models;
using MultipleChoice.Services;
using Org.BouncyCastle.Asn1.Cmp;
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

namespace MultipleChoice.CustomComponent
{
    /// <summary>
    /// Interaction logic for QuestionCardUserControl.xaml
    /// </summary>
    public partial class QuestionCardUserControl : UserControl
    {
        public static readonly DependencyProperty QuizzProperty =
        DependencyProperty.Register(
        nameof(Quizz),
        typeof(Quizz),
        typeof(QuestionCardUserControl),
        new PropertyMetadata(null, OnQuizzChanged));

        private readonly UserService _userService = new UserService();
        private readonly QuizzDetailsService _quizzDetailsService = new QuizzDetailsService();
        private readonly AttempServices _attempServices = new AttempServices();
        public Quizz Quizz
        {
            get => (Quizz)GetValue(QuizzProperty);
            set => SetValue(QuizzProperty, value);
        }

        private static void OnQuizzChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is QuestionCardUserControl control && e.NewValue is Quizz quizz)
            {
                control.UpdateUI(quizz);
            }
        }

        public QuestionCardUserControl()
        {
            InitializeComponent();
        }

        private void UpdateUI(Quizz quizz)
        {
            TxtTitle.Text = quizz.Title;
            TxtAuthor.Text = $"#{_userService.GetById(quizz.CreatedBy).Username}";
            TxtCategory.Text = quizz.Type;
            TxtQuestionNumber.Text = _quizzDetailsService.GetByQuizzId(quizz.Id).Count + "";
            if (quizz.StartAt.HasValue && quizz.StartAt > DateTime.Now)
            {
                LblStatus.Foreground = Brushes.Red;
                TxtStatus.Foreground = Brushes.Red;
                TxtStatus.Text = "Not start yet";
                return;
            }
            if (quizz.EndAt.HasValue && quizz.EndAt < DateTime.Now)
            {
                LblStatus.Foreground = Brushes.Red;
                TxtStatus.Foreground = Brushes.Red;
                TxtStatus.Text = "Expired";
            }
            else
            {
                LblStatus.Foreground = Brushes.Green;
                TxtStatus.Foreground = Brushes.Green;
                TxtStatus.Text = "Available";
            }
        }

        private void BtnRanking_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) as MenuWindow;
            window.MainFrame.Navigate(new RankingPage(Quizz.Id));
        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            var attemps = _attempServices.GetAttempsOfUserByQuizzId(MenuWindow.UserId, Quizz.Id);
            if (attemps >= Quizz.AttempNumber)
            {
                MessageBox.Show("Error: Run out of attemps", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DateTime.Now > Quizz.EndAt)
            {
                MessageBox.Show("Error: Quizz has been ended", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var window = Window.GetWindow(this) as MenuWindow;
            window.MainFrame.Navigate(new ExamPage(Quizz.Id));
        }
    }
}
