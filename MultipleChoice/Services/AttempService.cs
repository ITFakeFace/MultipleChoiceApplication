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
        public List<Attemp> GetByQuizzId(int quizzId)
        {
            var result = new List<Attemp>();

            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    string sql = "SELECT * FROM attemps WHERE quizz_id = @QuizzId";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@QuizzId", quizzId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var attempt = new Attemp
                                {
                                    Id = reader.GetInt32("id"),
                                    AnsweredBy = reader.GetInt32("answered_by"),
                                    QuizzId = reader.GetInt32("quizz_id"),
                                    CorrectNumber = reader.IsDBNull(reader.GetOrdinal("correct_number"))
                                                    ? 0
                                                    : reader.GetInt32("correct_number"),
                                    Time = reader.GetTimeSpan("time"),
                                    Complete = reader.GetBoolean("complete")
                                };

                                result.Add(attempt);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log hoặc xử lý lỗi
                }
            }

            return result;
        }
        public List<Attemp> GetAll()
        {
            var result = new List<Attemp>();

            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    string sql = "SELECT * FROM attemps";

                    using (var cmd = new MySqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var attempt = new Attemp
                            {
                                Id = reader.GetInt32("id"),
                                AnsweredBy = reader.GetInt32("answered_by"),
                                QuizzId = reader.GetInt32("quizz_id"),
                                CorrectNumber = reader.IsDBNull(reader.GetOrdinal("correct_number"))
                                                ? 0
                                                : reader.GetInt32("correct_number"),
                                Time = reader.GetTimeSpan("time"),
                                Complete = reader.GetBoolean("complete")
                            };

                            result.Add(attempt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý hoặc log lỗi
                }
            }

            return result;
        }
        public int Create(Attemp attempt)
        {
            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    // Câu lệnh SQL chèn dữ liệu vào bảng attemps
                    string sql = @"INSERT INTO multiplechoiceapplication.attemps 
                           (answered_by, quizz_id, correct_number, `time`, complete,start_at)
                           VALUES 
                           (@AnsweredBy, @QuizzId, @CorrectNumber, @Time, @Complete,@StartAt)";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        // Thêm tham số vào câu lệnh SQL
                        cmd.Parameters.AddWithValue("@AnsweredBy", attempt.AnsweredBy);
                        cmd.Parameters.AddWithValue("@QuizzId", attempt.QuizzId);
                        cmd.Parameters.AddWithValue("@CorrectNumber", attempt.CorrectNumber);
                        cmd.Parameters.AddWithValue("@Time", attempt.Time);
                        cmd.Parameters.AddWithValue("@Complete", attempt.Complete);
                        cmd.Parameters.AddWithValue("@StartAt", attempt.StartAt);

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
                        SELECT u.username, q.title, a.correct_number, a.`time`, a.complete,a.start_at
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
                                    Complete = reader.GetBoolean("complete"),
                                    StartAt = reader.GetDateTime("start_at"),

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
        public int GetHistoryAttempts(int userId, int quizzId)
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

        public List<UserAttempt> GetAttemptsByUserID(int answeredBy)
        {
            List<UserAttempt> results = new List<UserAttempt>();

            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = @"
                    SELECT 
                        a.correct_number,
                        a.`time`,
                        a.start_at,
                        (SELECT q.title 
                         FROM quizzes q 
                         WHERE q.id = a.quizz_id) AS 'quizz',
                        qc.total_number,
                        a.complete
                    FROM attemps a
                    JOIN (
                        SELECT q.quizz_id, COUNT(q.id) AS total_number
                        FROM quizzdetails q 
                        GROUP BY q.quizz_id
                    ) AS qc ON a.quizz_id = qc.quizz_id
                    WHERE a.answered_by =  @answeredBy";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@answeredBy", answeredBy);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserAttempt result = new UserAttempt()
                                {
                                    Quizz = reader["quizz"].ToString(),
                                    Score = (float)Math.Round(((float)Convert.ToInt32(reader["correct_number"]) * 10 / Convert.ToInt32(reader["total_number"])), 2),
                                    Time = (TimeSpan)reader["time"],
                                    StartAt = Convert.ToDateTime(reader["start_at"]),
                                    IsCompleted = reader["complete"].ToString() == "True" ? "Completed" : "Not Completed"
                                };
                                results.Add(result);
                            }
                            return results;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null!;
                }
            }
        }

        public int GetTotalQuizzDetails(int userId, int quizzId)
        {
            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    string sql = @"
                SELECT COUNT(*) 
                FROM attemps A 
                JOIN quizzDetails QD ON A.quizz_id = QD.quizz_id 
                WHERE A.answered_by = @UserId AND A.quizz_id = @QuizzId";

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
                    return -1; // Trả về -1 nếu có lỗi
                }
            }
        }
    }
}
