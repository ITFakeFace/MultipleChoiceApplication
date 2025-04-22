using MultipleChoice.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleChoice.Services
{
    internal class AttempServices : BaseService
    {

        public int Create(Attemp attempt)
        {
            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    // Câu lệnh SQL chèn dữ liệu vào bảng attemps
                    string sql = @"INSERT INTO multiplechoiceapplication.attemps 
                           (answered_by, quizz_id, correct_number, `time`, complete)
                           VALUES 
                           (@AnsweredBy, @QuizzId, @CorrectNumber, @Time, @Complete)";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        // Thêm tham số vào câu lệnh SQL
                        cmd.Parameters.AddWithValue("@AnsweredBy", attempt.AnsweredBy);
                        cmd.Parameters.AddWithValue("@QuizzId", attempt.QuizzId);
                        cmd.Parameters.AddWithValue("@CorrectNumber", attempt.CorrectNumber);
                        cmd.Parameters.AddWithValue("@Time", attempt.Time);
                        cmd.Parameters.AddWithValue("@Complete", attempt.Complete);

                        // Thực thi câu lệnh SQL và lấy ID của bản ghi vừa chèn
                        cmd.ExecuteNonQuery();

                        // Lấy ID của bản ghi vừa chèn vào
                        string getIdSql = "SELECT LAST_INSERT_ID()";
                        using (var idCmd = new MySqlCommand(getIdSql, conn))
                        {
                            // Trả về ID của bản ghi vừa thêm
                            return Convert.ToInt32(idCmd.ExecuteScalar());
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu có
                    return -1;  // Trả về -1 nếu có lỗi trong quá trình thêm dữ liệu
                }
            }
        }
        public AttemptInfo? GetAttemptInfoByID(int attemptId)
        {
            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    // Câu lệnh SQL thực hiện JOIN giữa bảng attemps, users và quizzes
                    string sql = @"
                        SELECT u.username, q.title, a.correct_number, a.`time`, a.complete
                        FROM attemps a
                        JOIN users u ON u.id = a.answered_by
                        JOIN quizzes q ON a.quizz_id = q.id
                        WHERE a.id = @AttemptId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        // Thêm tham số vào câu lệnh SQL
                        cmd.Parameters.AddWithValue("@AttemptId", attemptId);

                        // Thực thi câu lệnh và lấy kết quả
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Trả về một đối tượng AttemptInfo với dữ liệu từ câu truy vấn
                                return new AttemptInfo
                                {
                                    Username = reader.GetString("username"),
                                    Title = reader.GetString("title"),
                                    CorrectNumber = reader.GetInt32("correct_number"),
                                    Time = reader.GetTimeSpan("time"),
                                    Complete = reader.GetBoolean("complete")
                                };
                            }
                        }
                    }
                    return null;  // Trả về null nếu không tìm thấy kết quả
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu có
                    return null;  // Trả về null nếu có lỗi trong quá trình truy vấn
                }
            }
        }

        public int GetAttempsOfUserByQuizzId(int userId, int quizzId)
        {
            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    string sql = @"SELECT COUNT(*) as ATTEMPS 
                           FROM attemps 
                           WHERE answered_by = @UserId AND quizz_id = @QuizzId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@QuizzId", quizzId);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log nếu cần
                    return -1; // Trả về -1 nếu lỗi
                }
            }
        }

    }
}
