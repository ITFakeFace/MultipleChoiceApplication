using MultipleChoice.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleChoice.Services
{
    internal class AnswerService : BaseService
    {
        public bool Create(List<Answer> answers)
        {
            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    // Câu lệnh SQL chèn dữ liệu vào bảng answers
                    string sql = @"INSERT INTO multiplechoiceapplication.answers
                           (question_id, attemp_id, answer)
                           VALUES 
                           (@QuestionId, @AttempID, @Answer)";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        foreach (var answer in answers)
                        {
                            cmd.Parameters.Clear();  
                            cmd.Parameters.AddWithValue("@QuestionId", answer.QuestionId);
                            cmd.Parameters.AddWithValue("@AttempID", answer.AttempId);
                            cmd.Parameters.AddWithValue("@Answer", answer.SelectedAnswer);
                            cmd.ExecuteNonQuery();
                        }
                }
                return true;
            }
                catch (Exception ex)
                {
                return false;
            }
        }
        }

    }
}
