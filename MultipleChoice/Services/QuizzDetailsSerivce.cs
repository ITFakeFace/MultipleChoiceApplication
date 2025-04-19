﻿using MultipleChoice.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultipleChoice.Services
{
    public class QuizzDetailsService : BaseService
    {
        public bool Create(QuizzDetails detail)
        {
            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string sql = @"INSERT INTO QuizzDetails 
                        (quizz_id, question, answer1, answer2, answer3, answer4, correct_answer)
                        VALUES 
                        (@QuizzId, @Question, @Answer1, @Answer2, @Answer3, @Answer4, @CorrectAnswer)";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@QuizzId", detail.QuizzId);
                        cmd.Parameters.AddWithValue("@Question", detail.Question);
                        cmd.Parameters.AddWithValue("@Answer1", detail.Answer1);
                        cmd.Parameters.AddWithValue("@Answer2", detail.Answer2);
                        cmd.Parameters.AddWithValue("@Answer3", detail.Answer3);
                        cmd.Parameters.AddWithValue("@Answer4", detail.Answer4);
                        cmd.Parameters.AddWithValue("@CorrectAnswer", detail.CorrectAnswer);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
        }

        public List<QuizzDetails> GetByQuizzId(int quizzId)
        {
            var list = new List<QuizzDetails>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM QuizzDetails WHERE quizz_id = @QuizzId";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@QuizzId", quizzId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new QuizzDetails
                            {
                                Id = reader.GetInt32("id"),
                                QuizzId = reader.GetInt32("quizz_id"),
                                Question = reader.GetString("question"),
                                Answer1 = reader.GetString("answer1"),
                                Answer2 = reader.GetString("answer2"),
                                Answer3 = reader.GetString("answer3"),
                                Answer4 = reader.GetString("answer4"),
                                CorrectAnswer = reader.GetInt16("correct_answer")
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void Update(QuizzDetails detail)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"UPDATE QuizzDetails SET
                    question = @Question,
                    answer1 = @Answer1,
                    answer2 = @Answer2,
                    answer3 = @Answer3,
                    answer4 = @Answer4,
                    correct_answer = @CorrectAnswer
                    WHERE id = @Id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Question", detail.Question);
                    cmd.Parameters.AddWithValue("@Answer1", detail.Answer1);
                    cmd.Parameters.AddWithValue("@Answer2", detail.Answer2);
                    cmd.Parameters.AddWithValue("@Answer3", detail.Answer3);
                    cmd.Parameters.AddWithValue("@Answer4", detail.Answer4);
                    cmd.Parameters.AddWithValue("@CorrectAnswer", detail.CorrectAnswer);
                    cmd.Parameters.AddWithValue("@Id", detail.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM QuizzDetails WHERE id = @Id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
