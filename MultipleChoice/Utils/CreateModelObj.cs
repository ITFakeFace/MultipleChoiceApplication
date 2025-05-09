﻿using MultipleChoice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleChoice.Utils
{
    class CreateModelObj
    {
        //Create Attemp
        public static Attemp CreateAttemp(List<AnswerDTO> answerList, int quizzID, int timeSpent,bool isCompleted,DateTime currentDateTime)
        {
            int correctNumber = Grading(answerList);
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeSpent);
            return new Attemp
            {
                AnsweredBy = MenuWindow.UserId,
                QuizzId = quizzID,
                CorrectNumber = correctNumber,
                Time = timeSpan,
                Complete = isCompleted,
                StartAt = currentDateTime,
            };
        }

        public static int Grading(List<AnswerDTO> answerList)
        {
            int counting = 0;
            foreach (var answer in answerList)
            {
                if (answer.CorrectAnswer == answer.Answer)
                {
                    counting++;
                }
            }
            return counting;

        }

        //Create Answer
        public static List<Answer> CreateAnswers(List<AnswerDTO> answerDTOs, int attemptID)
        {
            List<Answer> answers = new List<Answer>();
            foreach(var answerDTO in answerDTOs)
            {
                answers.Add(new Answer 
                {
                    AttempId = attemptID,
                    QuestionId = answerDTO.QuestionID,
                    SelectedAnswer = answerDTO.Answer
                });
            }
            return answers;
        }

    }
}
