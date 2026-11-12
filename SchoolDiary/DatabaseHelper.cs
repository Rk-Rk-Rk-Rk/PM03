using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using SchoolDiary.Models;

namespace SchoolDiary
{
    public static class DatabaseHelper
    {
        private static string _dbPath = string.Empty;
        private static string _provider = "Microsoft.ACE.OLEDB.12.0";

        public static string DatabasePath
        {
            get
            {
                if (string.IsNullOrEmpty(_dbPath) || !File.Exists(_dbPath))
                {
                    _dbPath = FindDatabasePath();
                }
                return _dbPath;
            }
        }

        private static string FindDatabasePath()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // Prioritize the canonical repo Database folder so all updates persist directly in Database\SchoolDiary.accdb
            string[] candidates = new[]
            {
                @"C:\Users\xipwl\OneDrive\Desktop\PM03\Database\SchoolDiary.accdb",
                Path.Combine(baseDir, "..", "..", "..", "Database", "SchoolDiary.accdb"),
                Path.Combine(baseDir, "..", "..", "..", "..", "Database", "SchoolDiary.accdb"),
                Path.Combine(baseDir, "..", "Database", "SchoolDiary.accdb"),
                Path.Combine(baseDir, "Database", "SchoolDiary.accdb"),
                Path.Combine(baseDir, "SchoolDiary.accdb")
            };

            foreach (var candidate in candidates)
            {
                try
                {
                    string full = Path.GetFullPath(candidate);
                    if (File.Exists(full))
                    {
                        return full;
                    }
                }
                catch { }
            }

            // Fallback default
            return Path.Combine(baseDir, "SchoolDiary.accdb");
        }

        public static void SyncDatabaseCopies()
        {
            try
            {
                string activePath = DatabasePath;
                if (!File.Exists(activePath)) return;

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string localBinDb = Path.GetFullPath(Path.Combine(baseDir, "SchoolDiary.accdb"));
                string canonicalDb = Path.GetFullPath(@"C:\Users\xipwl\OneDrive\Desktop\PM03\Database\SchoolDiary.accdb");

                if (string.Equals(activePath, canonicalDb, StringComparison.OrdinalIgnoreCase))
                {
                    if (File.Exists(localBinDb) && !string.Equals(activePath, localBinDb, StringComparison.OrdinalIgnoreCase))
                    {
                        File.Copy(activePath, localBinDb, true);
                    }
                }
                else
                {
                    if (File.Exists(canonicalDb))
                    {
                        File.Copy(activePath, canonicalDb, true);
                    }
                }
            }
            catch
            {
                // File lock by Access UI or active process - ignore
            }
        }

        public static string GetConnectionString()
        {
            return $"Provider={_provider};Data Source={DatabasePath};Persist Security Info=False;Mode=Share Deny None;";
        }

        public static OleDbConnection GetOpenConnection()
        {
            OleDbConnection conn = new OleDbConnection(GetConnectionString());
            try
            {
                conn.Open();
                return conn;
            }
            catch
            {
                // Try fallback to ACE.OLEDB.16.0 if 12.0 failed
                if (_provider == "Microsoft.ACE.OLEDB.12.0")
                {
                    _provider = "Microsoft.ACE.OLEDB.16.0";
                    conn = new OleDbConnection(GetConnectionString());
                    conn.Open();
                    return conn;
                }
                throw;
            }
        }

        public static bool TestConnection(out string errorMessage)
        {
            try
            {
                using var conn = GetOpenConnection();
                errorMessage = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        // ==================== AUTHENTICATION ====================

        public static User? AuthenticateUser(string username, string password)
        {
            using var conn = GetOpenConnection();
            string query = "SELECT [UserID], [Username], [Password], [FullName], [Role] FROM [Users] WHERE [Username] = ? AND [Password] = ?";
            using var cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("?", username.Trim());
            cmd.Parameters.AddWithValue("?", password.Trim());

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    UserID = Convert.ToInt32(reader["UserID"]),
                    Username = reader["Username"].ToString() ?? string.Empty,
                    Password = reader["Password"].ToString() ?? string.Empty,
                    FullName = reader["FullName"].ToString() ?? string.Empty,
                    Role = reader["Role"].ToString() ?? string.Empty
                };
            }
            return null;
        }

        // ==================== STUDENTS ====================

        public static DataTable GetStudentsTable()
        {
            using var conn = GetOpenConnection();
            string query = "SELECT [StudentID] AS [Код], [FullName] AS [ФИО ученика], [ClassGrade] AS [Класс], [Phone] AS [Телефон] FROM [Students] ORDER BY [FullName]";
            using var adapter = new OleDbDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public static List<Student> GetStudentsList()
        {
            var list = new List<Student>();
            using var conn = GetOpenConnection();
            string query = "SELECT [StudentID], [FullName], [ClassGrade], [Phone] FROM [Students] ORDER BY [FullName]";
            using var cmd = new OleDbCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Student
                {
                    StudentID = Convert.ToInt32(reader["StudentID"]),
                    FullName = reader["FullName"].ToString() ?? string.Empty,
                    ClassGrade = reader["ClassGrade"].ToString() ?? string.Empty,
                    Phone = reader["Phone"].ToString() ?? string.Empty
                });
            }
            return list;
        }

        public static void AddStudent(Student student)
        {
            using var conn = GetOpenConnection();
            string query = "INSERT INTO [Students] ([FullName], [ClassGrade], [Phone]) VALUES (?, ?, ?)";
            using var cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("?", student.FullName);
            cmd.Parameters.AddWithValue("?", student.ClassGrade);
            cmd.Parameters.AddWithValue("?", student.Phone);
            cmd.ExecuteNonQuery();
            SyncDatabaseCopies();
        }

        public static void UpdateStudent(Student student)
        {
            using var conn = GetOpenConnection();
            string query = "UPDATE [Students] SET [FullName] = ?, [ClassGrade] = ?, [Phone] = ? WHERE [StudentID] = ?";
            using var cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("?", student.FullName);
            cmd.Parameters.AddWithValue("?", student.ClassGrade);
            cmd.Parameters.AddWithValue("?", student.Phone);
            cmd.Parameters.AddWithValue("?", student.StudentID);
            cmd.ExecuteNonQuery();
            SyncDatabaseCopies();
        }

        public static void DeleteStudent(int studentId)
        {
            using var conn = GetOpenConnection();
            // Delete student's grades first
            using (var cmdGrades = new OleDbCommand("DELETE FROM [Grades] WHERE [StudentID] = ?", conn))
            {
                cmdGrades.Parameters.AddWithValue("?", studentId);
                cmdGrades.ExecuteNonQuery();
            }
            // Delete student
            using (var cmdStudent = new OleDbCommand("DELETE FROM [Students] WHERE [StudentID] = ?", conn))
            {
                cmdStudent.Parameters.AddWithValue("?", studentId);
                cmdStudent.ExecuteNonQuery();
            }
            SyncDatabaseCopies();
        }

        // ==================== SUBJECTS ====================

        public static List<Subject> GetSubjectsList()
        {
            var list = new List<Subject>();
            using var conn = GetOpenConnection();
            string query = "SELECT [SubjectID], [SubjectName], [TeacherName] FROM [Subjects] ORDER BY [SubjectName]";
            using var cmd = new OleDbCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Subject
                {
                    SubjectID = Convert.ToInt32(reader["SubjectID"]),
                    SubjectName = reader["SubjectName"].ToString() ?? string.Empty,
                    TeacherName = reader["TeacherName"].ToString() ?? string.Empty
                });
            }
            return list;
        }

        // ==================== GRADES ====================

        public static DataTable GetGradesTable(int? studentId = null, int? subjectId = null)
        {
            using var conn = GetOpenConnection();
            string query = @"SELECT 
                                g.[GradeID] AS [ID],
                                g.[StudentID],
                                s.[FullName] AS [Ученик],
                                s.[ClassGrade] AS [Класс],
                                g.[SubjectID],
                                sub.[SubjectName] AS [Предмет],
                                g.[LessonDate] AS [Дата],
                                g.[GradeValue] AS [Оценка],
                                g.[Comment] AS [Примечание]
                             FROM ([Grades] g
                             INNER JOIN [Students] s ON g.[StudentID] = s.[StudentID])
                             INNER JOIN [Subjects] sub ON g.[SubjectID] = sub.[SubjectID]
                             WHERE 1=1 ";

            if (studentId.HasValue && studentId.Value > 0)
                query += " AND g.[StudentID] = ? ";
            if (subjectId.HasValue && subjectId.Value > 0)
                query += " AND g.[SubjectID] = ? ";

            query += " ORDER BY g.[LessonDate] DESC, g.[GradeID] DESC";

            using var cmd = new OleDbCommand(query, conn);
            if (studentId.HasValue && studentId.Value > 0)
                cmd.Parameters.AddWithValue("?", studentId.Value);
            if (subjectId.HasValue && subjectId.Value > 0)
                cmd.Parameters.AddWithValue("?", subjectId.Value);

            using var adapter = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public static void AddGrade(Grade grade)
        {
            using var conn = GetOpenConnection();
            string query = "INSERT INTO [Grades] ([StudentID], [SubjectID], [LessonDate], [GradeValue], [Comment]) VALUES (?, ?, ?, ?, ?)";
            using var cmd = new OleDbCommand(query, conn);
            cmd.Parameters.Add("?", OleDbType.Integer).Value = grade.StudentID;
            cmd.Parameters.Add("?", OleDbType.Integer).Value = grade.SubjectID;
            cmd.Parameters.Add("?", OleDbType.Date).Value = grade.LessonDate.Date;
            cmd.Parameters.Add("?", OleDbType.Integer).Value = grade.GradeValue;
            cmd.Parameters.Add("?", OleDbType.VarWChar, 255).Value = grade.Comment ?? string.Empty;
            cmd.ExecuteNonQuery();
            SyncDatabaseCopies();
        }

        public static void UpdateGrade(Grade grade)
        {
            using var conn = GetOpenConnection();
            string query = "UPDATE [Grades] SET [StudentID] = ?, [SubjectID] = ?, [LessonDate] = ?, [GradeValue] = ?, [Comment] = ? WHERE [GradeID] = ?";
            using var cmd = new OleDbCommand(query, conn);
            cmd.Parameters.Add("?", OleDbType.Integer).Value = grade.StudentID;
            cmd.Parameters.Add("?", OleDbType.Integer).Value = grade.SubjectID;
            cmd.Parameters.Add("?", OleDbType.Date).Value = grade.LessonDate.Date;
            cmd.Parameters.Add("?", OleDbType.Integer).Value = grade.GradeValue;
            cmd.Parameters.Add("?", OleDbType.VarWChar, 255).Value = grade.Comment ?? string.Empty;
            cmd.Parameters.Add("?", OleDbType.Integer).Value = grade.GradeID;
            cmd.ExecuteNonQuery();
            SyncDatabaseCopies();
        }

        public static void DeleteGrade(int gradeId)
        {
            using var conn = GetOpenConnection();
            string query = "DELETE FROM [Grades] WHERE [GradeID] = ?";
            using var cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("?", gradeId);
            cmd.ExecuteNonQuery();
            SyncDatabaseCopies();
        }

        // ==================== GRADEBOOK JOURNAL (MATRIX) ====================

        public static void EnsureLessonsTableExists(OleDbConnection conn)
        {
            try
            {
                var schema = conn.GetSchema("Tables");
                bool exists = false;
                foreach (DataRow row in schema.Rows)
                {
                    if (string.Equals(row["TABLE_NAME"]?.ToString(), "Lessons", StringComparison.OrdinalIgnoreCase))
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    string sql = "CREATE TABLE [Lessons] ([LessonID] AUTOINCREMENT PRIMARY KEY, [ClassGrade] VARCHAR(50), [SubjectID] INT, [LessonDate] DATETIME, [Topic] VARCHAR(255))";
                    using var cmd = new OleDbCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        public static List<string> GetDistinctClasses()
        {
            var list = new List<string>();
            using var conn = GetOpenConnection();
            string query = "SELECT DISTINCT [ClassGrade] FROM [Students] WHERE [ClassGrade] IS NOT NULL AND [ClassGrade] <> '' ORDER BY [ClassGrade]";
            using var cmd = new OleDbCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string cls = reader["ClassGrade"]?.ToString() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(cls)) list.Add(cls);
            }
            return list;
        }

        public static List<Student> GetStudentsByClass(string classGrade)
        {
            var list = new List<Student>();
            using var conn = GetOpenConnection();
            string query = "SELECT [StudentID], [FullName], [ClassGrade], [Phone] FROM [Students] ";
            if (!string.IsNullOrEmpty(classGrade))
                query += " WHERE [ClassGrade] = ? ";
            query += " ORDER BY [FullName]";

            using var cmd = new OleDbCommand(query, conn);
            if (!string.IsNullOrEmpty(classGrade))
                cmd.Parameters.AddWithValue("?", classGrade);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Student
                {
                    StudentID = Convert.ToInt32(reader["StudentID"]),
                    FullName = reader["FullName"]?.ToString() ?? string.Empty,
                    ClassGrade = reader["ClassGrade"]?.ToString() ?? string.Empty,
                    Phone = reader["Phone"]?.ToString() ?? string.Empty
                });
            }
            return list;
        }

        public static List<DateTime> GetLessonDates(string classGrade, int subjectId)
        {
            var dates = new SortedSet<DateTime>();
            using var conn = GetOpenConnection();
            EnsureLessonsTableExists(conn);

            // 1. From Lessons table
            string queryLessons = "SELECT [LessonDate] FROM [Lessons] WHERE [SubjectID] = ? ";
            if (!string.IsNullOrEmpty(classGrade))
                queryLessons += " AND [ClassGrade] = ? ";

            using (var cmd = new OleDbCommand(queryLessons, conn))
            {
                cmd.Parameters.AddWithValue("?", subjectId);
                if (!string.IsNullOrEmpty(classGrade))
                    cmd.Parameters.AddWithValue("?", classGrade);

                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    if (r["LessonDate"] != DBNull.Value)
                    {
                        dates.Add(Convert.ToDateTime(r["LessonDate"]).Date);
                    }
                }
            }

            // 2. Also from Grades table
            string queryGrades = @"SELECT DISTINCT g.[LessonDate] 
                                   FROM [Grades] g 
                                   INNER JOIN [Students] s ON g.[StudentID] = s.[StudentID] 
                                   WHERE g.[SubjectID] = ? ";
            if (!string.IsNullOrEmpty(classGrade))
                queryGrades += " AND s.[ClassGrade] = ? ";

            using (var cmd = new OleDbCommand(queryGrades, conn))
            {
                cmd.Parameters.AddWithValue("?", subjectId);
                if (!string.IsNullOrEmpty(classGrade))
                    cmd.Parameters.AddWithValue("?", classGrade);

                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    if (r["LessonDate"] != DBNull.Value)
                    {
                        dates.Add(Convert.ToDateTime(r["LessonDate"]).Date);
                    }
                }
            }

            return dates.ToList();
        }

        public static void AddLessonDate(string classGrade, int subjectId, DateTime lessonDate, string topic = "Учебное занятие")
        {
            using var conn = GetOpenConnection();
            EnsureLessonsTableExists(conn);
            DateTime day = lessonDate.Date;

            string check = "SELECT COUNT(*) FROM [Lessons] WHERE [ClassGrade] = ? AND [SubjectID] = ? AND DateValue([LessonDate]) = DateValue(?)";
            using (var checkCmd = new OleDbCommand(check, conn))
            {
                checkCmd.Parameters.Add("?", OleDbType.VarWChar, 50).Value = classGrade;
                checkCmd.Parameters.Add("?", OleDbType.Integer).Value = subjectId;
                checkCmd.Parameters.Add("?", OleDbType.Date).Value = day;
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count == 0)
                {
                    string insert = "INSERT INTO [Lessons] ([ClassGrade], [SubjectID], [LessonDate], [Topic]) VALUES (?, ?, ?, ?)";
                    using var insertCmd = new OleDbCommand(insert, conn);
                    insertCmd.Parameters.Add("?", OleDbType.VarWChar, 50).Value = classGrade;
                    insertCmd.Parameters.Add("?", OleDbType.Integer).Value = subjectId;
                    insertCmd.Parameters.Add("?", OleDbType.Date).Value = day;
                    insertCmd.Parameters.Add("?", OleDbType.VarWChar, 255).Value = string.IsNullOrWhiteSpace(topic) ? "Учебное занятие" : topic;
                    insertCmd.ExecuteNonQuery();
                }
            }
            SyncDatabaseCopies();
        }

        public static void DeleteLessonDate(string classGrade, int subjectId, DateTime lessonDate)
        {
            using var conn = GetOpenConnection();
            EnsureLessonsTableExists(conn);
            DateTime day = lessonDate.Date;

            // Delete from Lessons
            string delLessons = "DELETE FROM [Lessons] WHERE [ClassGrade] = ? AND [SubjectID] = ? AND DateValue([LessonDate]) = DateValue(?)";
            using (var cmd = new OleDbCommand(delLessons, conn))
            {
                cmd.Parameters.Add("?", OleDbType.VarWChar, 50).Value = classGrade;
                cmd.Parameters.Add("?", OleDbType.Integer).Value = subjectId;
                cmd.Parameters.Add("?", OleDbType.Date).Value = day;
                cmd.ExecuteNonQuery();
            }

            // Also delete any grades for students of this class on this date
            var studentIds = GetStudentsByClass(classGrade).Select(s => s.StudentID).ToList();
            if (studentIds.Count > 0)
            {
                string idList = string.Join(",", studentIds);
                string delGrades = $"DELETE FROM [Grades] WHERE [SubjectID] = ? AND DateValue([LessonDate]) = DateValue(?) AND [StudentID] IN ({idList})";
                using var cmd = new OleDbCommand(delGrades, conn);
                cmd.Parameters.Add("?", OleDbType.Integer).Value = subjectId;
                cmd.Parameters.Add("?", OleDbType.Date).Value = day;
                cmd.ExecuteNonQuery();
            }
            SyncDatabaseCopies();
        }

        public static Dictionary<(int StudentID, DateTime Date), (int GradeID, int GradeValue, string Comment)> GetGradesMapForClass(string classGrade, int subjectId)
        {
            var map = new Dictionary<(int, DateTime), (int, int, string)>();
            using var conn = GetOpenConnection();
            string query = @"SELECT g.[GradeID], g.[StudentID], g.[LessonDate], g.[GradeValue], g.[Comment]
                             FROM [Grades] g
                             INNER JOIN [Students] s ON g.[StudentID] = s.[StudentID]
                             WHERE g.[SubjectID] = ? ";
            if (!string.IsNullOrEmpty(classGrade))
                query += " AND s.[ClassGrade] = ? ";

            using var cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("?", subjectId);
            if (!string.IsNullOrEmpty(classGrade))
                cmd.Parameters.AddWithValue("?", classGrade);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                int gId = Convert.ToInt32(r["GradeID"]);
                int sId = Convert.ToInt32(r["StudentID"]);
                DateTime dt = Convert.ToDateTime(r["LessonDate"]).Date;
                int val = Convert.ToInt32(r["GradeValue"]);
                string com = r["Comment"]?.ToString() ?? string.Empty;
                map[(sId, dt)] = (gId, val, com);
            }
            return map;
        }

        public static void SaveOrUpdateGrade(int studentId, int subjectId, DateTime date, int? gradeValue, string comment = "")
        {
            using var conn = GetOpenConnection();
            DateTime day = date.Date;

            int existingGradeId = -1;
            string checkQuery = "SELECT [GradeID] FROM [Grades] WHERE [StudentID] = ? AND [SubjectID] = ? AND DateValue([LessonDate]) = DateValue(?)";
            using (var checkCmd = new OleDbCommand(checkQuery, conn))
            {
                checkCmd.Parameters.Add("?", OleDbType.Integer).Value = studentId;
                checkCmd.Parameters.Add("?", OleDbType.Integer).Value = subjectId;
                checkCmd.Parameters.Add("?", OleDbType.Date).Value = day;
                var result = checkCmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    existingGradeId = Convert.ToInt32(result);
                }
            }

            if (gradeValue.HasValue && gradeValue.Value >= 2 && gradeValue.Value <= 5)
            {
                if (existingGradeId > 0)
                {
                    string updateQuery = "UPDATE [Grades] SET [GradeValue] = ?, [Comment] = ? WHERE [GradeID] = ?";
                    using var updateCmd = new OleDbCommand(updateQuery, conn);
                    updateCmd.Parameters.Add("?", OleDbType.Integer).Value = gradeValue.Value;
                    updateCmd.Parameters.Add("?", OleDbType.VarWChar, 255).Value = comment ?? string.Empty;
                    updateCmd.Parameters.Add("?", OleDbType.Integer).Value = existingGradeId;
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    string insertQuery = "INSERT INTO [Grades] ([StudentID], [SubjectID], [LessonDate], [GradeValue], [Comment]) VALUES (?, ?, ?, ?, ?)";
                    using var insertCmd = new OleDbCommand(insertQuery, conn);
                    insertCmd.Parameters.Add("?", OleDbType.Integer).Value = studentId;
                    insertCmd.Parameters.Add("?", OleDbType.Integer).Value = subjectId;
                    insertCmd.Parameters.Add("?", OleDbType.Date).Value = day;
                    insertCmd.Parameters.Add("?", OleDbType.Integer).Value = gradeValue.Value;
                    insertCmd.Parameters.Add("?", OleDbType.VarWChar, 255).Value = comment ?? string.Empty;
                    insertCmd.ExecuteNonQuery();
                }

                EnsureLessonExists(conn, studentId, subjectId, day);
            }
            else
            {
                if (existingGradeId > 0)
                {
                    string deleteQuery = "DELETE FROM [Grades] WHERE [GradeID] = ?";
                    using var deleteCmd = new OleDbCommand(deleteQuery, conn);
                    deleteCmd.Parameters.Add("?", OleDbType.Integer).Value = existingGradeId;
                    deleteCmd.ExecuteNonQuery();
                }
            }
            SyncDatabaseCopies();
        }

        private static void EnsureLessonExists(OleDbConnection conn, int studentId, int subjectId, DateTime date)
        {
            try
            {
                EnsureLessonsTableExists(conn);
                string classGrade = "";
                using (var getCmd = new OleDbCommand("SELECT [ClassGrade] FROM [Students] WHERE [StudentID] = ?", conn))
                {
                    getCmd.Parameters.Add("?", OleDbType.Integer).Value = studentId;
                    var obj = getCmd.ExecuteScalar();
                    if (obj != null) classGrade = obj.ToString() ?? "";
                }

                if (!string.IsNullOrEmpty(classGrade))
                {
                    string check = "SELECT COUNT(*) FROM [Lessons] WHERE [ClassGrade] = ? AND [SubjectID] = ? AND DateValue([LessonDate]) = DateValue(?)";
                    using var checkCmd = new OleDbCommand(check, conn);
                    checkCmd.Parameters.Add("?", OleDbType.VarWChar, 50).Value = classGrade;
                    checkCmd.Parameters.Add("?", OleDbType.Integer).Value = subjectId;
                    checkCmd.Parameters.Add("?", OleDbType.Date).Value = date.Date;
                    int cnt = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (cnt == 0)
                    {
                        string ins = "INSERT INTO [Lessons] ([ClassGrade], [SubjectID], [LessonDate], [Topic]) VALUES (?, ?, ?, ?)";
                        using var insCmd = new OleDbCommand(ins, conn);
                        insCmd.Parameters.Add("?", OleDbType.VarWChar, 50).Value = classGrade;
                        insCmd.Parameters.Add("?", OleDbType.Integer).Value = subjectId;
                        insCmd.Parameters.Add("?", OleDbType.Date).Value = date.Date;
                        insCmd.Parameters.Add("?", OleDbType.VarWChar, 255).Value = "Учебное занятие";
                        insCmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        public static Student? FindStudentByUser(User user)
        {
            if (user == null || user.Role != "Ученик") return null;
            var students = GetStudentsList();
            string userClean = user.FullName.Replace("(Ученик)", "").Trim();

            foreach (var s in students)
            {
                if (s.FullName.StartsWith(userClean, StringComparison.OrdinalIgnoreCase) ||
                    userClean.StartsWith(s.FullName, StringComparison.OrdinalIgnoreCase))
                {
                    return s;
                }
            }

            return students.FirstOrDefault();
        }

        // ==================== COMPUTATIONAL EXPERIMENT ====================

        public class StudentStats
        {
            public int StudentID { get; set; }
            public string FullName { get; set; } = string.Empty;
            public string ClassGrade { get; set; } = string.Empty;
            public int GradesCount { get; set; }
            public double AverageGrade { get; set; }
            public string Category { get; set; } = string.Empty;
        }

        public class SubjectStats
        {
            public string SubjectName { get; set; } = string.Empty;
            public int GradesCount { get; set; }
            public double AverageGrade { get; set; }
        }

        public class ExperimentResult
        {
            public int TotalGrades { get; set; }
            public double OverallAverage { get; set; }
            public int Count5 { get; set; }
            public int Count4 { get; set; }
            public int Count3 { get; set; }
            public int Count2 { get; set; }
            public double QualityPercent { get; set; } // % of 4 and 5
            public double SuccessPercent { get; set; } // % without 2
            public List<StudentStats> StudentRankings { get; set; } = new();
            public List<SubjectStats> SubjectRankings { get; set; } = new();
        }

        public static ExperimentResult CalculateExperiment()
        {
            var result = new ExperimentResult();
            using var conn = GetOpenConnection();

            // 1. Overall grades stats
            string queryGrades = "SELECT [GradeValue] FROM [Grades]";
            using (var cmd = new OleDbCommand(queryGrades, conn))
            using (var reader = cmd.ExecuteReader())
            {
                int sum = 0;
                while (reader.Read())
                {
                    int val = Convert.ToInt32(reader["GradeValue"]);
                    result.TotalGrades++;
                    sum += val;
                    if (val == 5) result.Count5++;
                    else if (val == 4) result.Count4++;
                    else if (val == 3) result.Count3++;
                    else if (val == 2) result.Count2++;
                }
                if (result.TotalGrades > 0)
                {
                    result.OverallAverage = Math.Round((double)sum / result.TotalGrades, 2);
                    result.QualityPercent = Math.Round((double)(result.Count5 + result.Count4) / result.TotalGrades * 100.0, 1);
                    result.SuccessPercent = Math.Round((double)(result.TotalGrades - result.Count2) / result.TotalGrades * 100.0, 1);
                }
            }

            // 2. Student performance calculation
            string queryStudents = @"SELECT 
                                        s.[StudentID],
                                        s.[FullName],
                                        s.[ClassGrade],
                                        COUNT(g.[GradeID]) AS [Cnt],
                                        AVG(g.[GradeValue]) AS [AvgVal],
                                        MIN(g.[GradeValue]) AS [MinVal]
                                     FROM [Students] s
                                     LEFT JOIN [Grades] g ON s.[StudentID] = g.[StudentID]
                                     GROUP BY s.[StudentID], s.[FullName], s.[ClassGrade]
                                     ORDER BY AVG(g.[GradeValue]) DESC, s.[FullName]";

            using (var cmd = new OleDbCommand(queryStudents, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int cnt = reader["Cnt"] != DBNull.Value ? Convert.ToInt32(reader["Cnt"]) : 0;
                    double avg = reader["AvgVal"] != DBNull.Value ? Math.Round(Convert.ToDouble(reader["AvgVal"]), 2) : 0.0;
                    int min = reader["MinVal"] != DBNull.Value ? Convert.ToInt32(reader["MinVal"]) : 0;

                    string category = "Нет оценок";
                    if (cnt > 0)
                    {
                        if (min == 5) category = "Отличник (5.0)";
                        else if (min >= 4) category = "Хорошист (4-5)";
                        else if (min >= 3) category = "Успевающий (3-5)";
                        else category = "Требуется помощь (есть 2)";
                    }

                    result.StudentRankings.Add(new StudentStats
                    {
                        StudentID = Convert.ToInt32(reader["StudentID"]),
                        FullName = reader["FullName"].ToString() ?? string.Empty,
                        ClassGrade = reader["ClassGrade"].ToString() ?? string.Empty,
                        GradesCount = cnt,
                        AverageGrade = avg,
                        Category = category
                    });
                }
            }

            // 3. Subject stats
            string querySubjects = @"SELECT 
                                        sub.[SubjectName],
                                        COUNT(g.[GradeID]) AS [Cnt],
                                        AVG(g.[GradeValue]) AS [AvgVal]
                                     FROM [Subjects] sub
                                     LEFT JOIN [Grades] g ON sub.[SubjectID] = g.[SubjectID]
                                     GROUP BY sub.[SubjectName]
                                     ORDER BY AVG(g.[GradeValue]) DESC, sub.[SubjectName]";

            using (var cmd = new OleDbCommand(querySubjects, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int cnt = reader["Cnt"] != DBNull.Value ? Convert.ToInt32(reader["Cnt"]) : 0;
                    double avg = reader["AvgVal"] != DBNull.Value ? Math.Round(Convert.ToDouble(reader["AvgVal"]), 2) : 0.0;

                    result.SubjectRankings.Add(new SubjectStats
                    {
                        SubjectName = reader["SubjectName"].ToString() ?? string.Empty,
                        GradesCount = cnt,
                        AverageGrade = avg
                    });
                }
            }

            return result;
        }
    }
}
