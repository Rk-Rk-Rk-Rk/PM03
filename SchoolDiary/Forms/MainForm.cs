using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SchoolDiary.Models;

namespace SchoolDiary.Forms
{
    public partial class MainForm : Form
    {
        private User _currentUser = new User
        {
            UserID = 1,
            Username = "admin",
            FullName = "Администратор системы",
            Role = "Администратор"
        };

        private Student? _currentStudent;
        private DataTable? _studentsTable;
        private bool _isLoading = true;

        // Matrix journal cache
        private List<DateTime> _currentLessonDates = new List<DateTime>();
        private List<Student> _currentClassStudents = new List<Student>();
        private Dictionary<(int StudentID, DateTime Date), (int GradeID, int GradeValue, string Comment)> _gradesMap = new();

        /// <summary>
        /// Конструктор по умолчанию (необходим для визуального дизайнера Visual Studio)
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            pnlTop.Resize += (s, e) => pnlTop.Invalidate();
            dgvGrades.Leave += (s, e) => dgvGrades.EndEdit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            dgvGrades.EndEdit();
            base.OnFormClosing(e);
        }

        /// <summary>
        /// Основной конструктор с передачей авторизованного пользователя
        /// </summary>
        public MainForm(User? currentUser) : this()
        {
            if (currentUser != null)
            {
                _currentUser = currentUser;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                _isLoading = true;
                ApplyUserContext();
                LoadFilterCombos();
                _isLoading = false;

                LoadJournalGrid();
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при инициализации программы:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void tabControl_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabGrades)
            {
                LoadJournalGrid();
            }
            else if (tabControl.SelectedTab == tabStudents)
            {
                FormatStudentsColumns();
            }
        }

        private void ApplyUserContext()
        {
            lblUserInfo.Text = $"Пользователь: {_currentUser.FullName} | Роль: {_currentUser.Role}";

            bool isStudent = string.Equals(_currentUser.Role, "Ученик", StringComparison.OrdinalIgnoreCase);

            if (isStudent)
            {
                _currentStudent = DatabaseHelper.FindStudentByUser(_currentUser);

                btnAddLessonDate.Visible = false;
                btnDeleteLessonDate.Visible = false;
                cmbFilterClass.Enabled = false;

                dgvGrades.ReadOnly = true;
                dgvGrades.ContextMenuStrip = null;
                lblJournalHint.Text = "Режим просмотра журнала: изменения может вносить только учитель.";

                btnAddStudent.Visible = false;
                btnEditStudent.Visible = false;
                btnDeleteStudent.Visible = false;
            }
            else
            {
                _currentStudent = null;

                btnAddLessonDate.Visible = true;
                btnDeleteLessonDate.Visible = true;
                cmbFilterClass.Enabled = true;

                dgvGrades.ReadOnly = false;
                dgvGrades.ContextMenuStrip = contextMenuGrade;
                lblJournalHint.Text = "Выберите ячейку и введите 2-5 (Del — очистить). Правый клик — меню.";

                btnAddStudent.Visible = true;
                btnEditStudent.Visible = true;
                btnDeleteStudent.Visible = true;
            }
        }

        private void btnSwitchUser_Click(object sender, EventArgs e)
        {
            dgvGrades.EndEdit();
            this.Hide();
            using var loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK && loginForm.AuthenticatedUser != null)
            {
                _currentUser = loginForm.AuthenticatedUser;
                _isLoading = true;
                ApplyUserContext();
                LoadFilterCombos();
                _isLoading = false;
                LoadJournalGrid();
                LoadStudents();
                this.Show();
            }
            else
            {
                this.Show();
            }
        }

        private void btnExitApp_Click(object? sender, EventArgs e)
        {
            dgvGrades.EndEdit();
            Application.Exit();
        }

        // ==================== HELPER COLUMN FORMATTING ====================

        private static void SetColumnWidthIfExists(DataGridView dgv, string colName, int width, string? headerText = null)
        {
            if (dgv == null || dgv.Columns == null || dgv.Columns.Count == 0) return;
            if (dgv.Columns.Contains(colName))
            {
                var col = dgv.Columns[colName];
                if (col != null)
                {
                    if (width > 0) col.Width = width;
                    if (headerText != null) col.HeaderText = headerText;
                }
            }
        }

        private void FormatStudentsColumns()
        {
            SetColumnWidthIfExists(dgvStudents, "Код", 60);
            SetColumnWidthIfExists(dgvStudents, "ФИО ученика", 320);
            SetColumnWidthIfExists(dgvStudents, "Класс", 100);
            SetColumnWidthIfExists(dgvStudents, "Телефон", 180);
        }

        // ==================== MATRIX JOURNAL LOGIC (v2.0) ====================

        private void LoadFilterCombos()
        {
            try
            {
                // 1. Classes filter
                var classes = DatabaseHelper.GetDistinctClasses();
                cmbFilterClass.Items.Clear();

                bool isStudent = string.Equals(_currentUser.Role, "Ученик", StringComparison.OrdinalIgnoreCase);
                if (isStudent && _currentStudent != null && !classes.Contains(_currentStudent.ClassGrade))
                {
                    if (!string.IsNullOrWhiteSpace(_currentStudent.ClassGrade))
                        classes.Add(_currentStudent.ClassGrade);
                }

                foreach (var cls in classes)
                {
                    cmbFilterClass.Items.Add(cls);
                }

                if (isStudent && _currentStudent != null)
                {
                    int idx = cmbFilterClass.Items.IndexOf(_currentStudent.ClassGrade);
                    cmbFilterClass.SelectedIndex = idx >= 0 ? idx : (cmbFilterClass.Items.Count > 0 ? 0 : -1);
                }
                else
                {
                    if (cmbFilterClass.Items.Count > 0)
                        cmbFilterClass.SelectedIndex = 0;
                }

                // 2. Subjects filter
                var subjects = DatabaseHelper.GetSubjectsList();
                cmbFilterSubject.Items.Clear();
                foreach (var s in subjects)
                {
                    cmbFilterSubject.Items.Add(new ComboBoxItem { Text = s.SubjectName, Value = s.SubjectID });
                }
                if (cmbFilterSubject.Items.Count > 0)
                    cmbFilterSubject.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при заполнении фильтров:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbFilterClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            LoadJournalGrid();
        }

        private void cmbFilterSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            LoadJournalGrid();
        }

        private void btnRefreshGrades_Click(object sender, EventArgs e)
        {
            LoadJournalGrid();
        }

        private void LoadJournalGrid()
        {
            try
            {
                dgvGrades.SuspendLayout();
                dgvGrades.Rows.Clear();
                dgvGrades.Columns.Clear();

                string selectedClass = cmbFilterClass.SelectedItem?.ToString() ?? string.Empty;
                int subjectId = 0;
                if (cmbFilterSubject.SelectedItem is ComboBoxItem itemSubject)
                    subjectId = itemSubject.Value;

                if (string.IsNullOrEmpty(selectedClass) || subjectId == 0)
                {
                    lblGradesCount.Text = "Выберите класс и предмет для отображения классного журнала";
                    dgvGrades.ResumeLayout();
                    return;
                }

                bool isStudent = string.Equals(_currentUser.Role, "Ученик", StringComparison.OrdinalIgnoreCase);

                // Fetch students, lesson dates and grades map from Database
                _currentClassStudents = DatabaseHelper.GetStudentsByClass(selectedClass);
                _currentLessonDates = DatabaseHelper.GetLessonDates(selectedClass, subjectId);
                _gradesMap = DatabaseHelper.GetGradesMapForClass(selectedClass, subjectId);

                // 1. Fixed Columns
                // Column 0: №
                var colNum = new DataGridViewTextBoxColumn
                {
                    Name = "colNum",
                    HeaderText = "№",
                    Width = 40,
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                colNum.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colNum.DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90);
                dgvGrades.Columns.Add(colNum);

                // Column 1: StudentID (hidden)
                var colStudentId = new DataGridViewTextBoxColumn
                {
                    Name = "colStudentId",
                    HeaderText = "StudentID",
                    Visible = false
                };
                dgvGrades.Columns.Add(colStudentId);

                // Column 2: ФИО ученика
                var colFullName = new DataGridViewTextBoxColumn
                {
                    Name = "colFullName",
                    HeaderText = "ФИО ученика",
                    Width = 260,
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                colFullName.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                colFullName.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
                dgvGrades.Columns.Add(colFullName);

                // Dynamic Date Columns
                foreach (var dt in _currentLessonDates)
                {
                    var colDate = new DataGridViewTextBoxColumn
                    {
                        Name = $"colDate_{dt:yyyyMMdd}",
                        HeaderText = dt.ToString("dd.MM"),
                        Tag = dt,
                        ToolTipText = $"Дата занятия: {dt:dd.MM.yyyy}",
                        Width = 60,
                        ReadOnly = isStudent,
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    colDate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    colDate.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    dgvGrades.Columns.Add(colDate);
                }

                // Average column
                var colAvg = new DataGridViewTextBoxColumn
                {
                    Name = "colAvg",
                    HeaderText = "Ср. балл",
                    Width = 85,
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                colAvg.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colAvg.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                colAvg.DefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
                dgvGrades.Columns.Add(colAvg);

                // 2. Populate Student Rows
                var allGrades = new List<int>();

                for (int i = 0; i < _currentClassStudents.Count; i++)
                {
                    var st = _currentClassStudents[i];
                    var rowValues = new object[_currentLessonDates.Count + 4];
                    rowValues[0] = (i + 1).ToString();
                    rowValues[1] = st.StudentID;
                    rowValues[2] = st.FullName;

                    var studentGrades = new List<int>();

                    for (int d = 0; d < _currentLessonDates.Count; d++)
                    {
                        var dt = _currentLessonDates[d];
                        if (_gradesMap.TryGetValue((st.StudentID, dt), out var gradeInfo) && gradeInfo.GradeValue >= 2 && gradeInfo.GradeValue <= 5)
                        {
                            rowValues[3 + d] = gradeInfo.GradeValue.ToString();
                            studentGrades.Add(gradeInfo.GradeValue);
                            allGrades.Add(gradeInfo.GradeValue);
                        }
                        else
                        {
                            rowValues[3 + d] = string.Empty;
                        }
                    }

                    // Average
                    if (studentGrades.Count > 0)
                    {
                        double avg = studentGrades.Average();
                        rowValues[rowValues.Length - 1] = avg.ToString("F2");
                    }
                    else
                    {
                        rowValues[rowValues.Length - 1] = "—";
                    }

                    int rowIndex = dgvGrades.Rows.Add(rowValues);
                    var row = dgvGrades.Rows[rowIndex];

                    // Set tooltips for comments on date cells
                    for (int d = 0; d < _currentLessonDates.Count; d++)
                    {
                        var dt = _currentLessonDates[d];
                        if (_gradesMap.TryGetValue((st.StudentID, dt), out var gInfo))
                        {
                            var cell = row.Cells[3 + d];
                            string tip = $"Дата: {dt:dd.MM.yyyy}\nОценка: {gInfo.GradeValue}";
                            if (!string.IsNullOrWhiteSpace(gInfo.Comment))
                                tip += $"\nПримечание: {gInfo.Comment}";
                            cell.ToolTipText = tip;
                        }
                    }

                    // Highlight current student's row if user is student
                    if (isStudent && _currentStudent != null && st.StudentID == _currentStudent.StudentID)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(242, 246, 252);
                        row.Cells["colFullName"].Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                    }
                }

                // 3. Update summary label
                int totalStudents = _currentClassStudents.Count;
                int totalLessons = _currentLessonDates.Count;
                string avgStr = allGrades.Count > 0 ? allGrades.Average().ToString("F2") : "0.00";
                lblGradesCount.Text = $"Учеников: {totalStudents} | Занятий: {totalLessons} | Оценок: {allGrades.Count} | Средний балл класса: {avgStr}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка построения классного журнала:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dgvGrades.ResumeLayout();
            }
        }

        private void dgvGrades_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e == null || e.CellStyle == null) return;
            if (e.ColumnIndex < 0 || e.ColumnIndex >= dgvGrades.Columns.Count) return;

            var col = dgvGrades.Columns[e.ColumnIndex];
            if (col.Tag is DateTime)
            {
                // Date grade cell
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }
            else if (col.Name == "colAvg")
            {
                e.CellStyle.ForeColor = Color.FromArgb(40, 40, 40);
                e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            }
        }

        private void dgvGrades_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var col = dgvGrades.Columns[e.ColumnIndex];
                if (col.Tag is DateTime)
                {
                    dgvGrades.CurrentCell = dgvGrades.Rows[e.RowIndex].Cells[e.ColumnIndex];
                }
            }
        }

        private void dgvGrades_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var col = dgvGrades.Columns[e.ColumnIndex];
            if (!(col.Tag is DateTime lessonDate)) return;

            var row = dgvGrades.Rows[e.RowIndex];
            int studentId = Convert.ToInt32(row.Cells["colStudentId"].Value);
            int subjectId = (cmbFilterSubject.SelectedItem as ComboBoxItem)?.Value ?? 0;
            if (subjectId == 0) return;

            object? rawVal = row.Cells[e.ColumnIndex].Value;
            string input = rawVal?.ToString()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                // Clear grade
                try
                {
                    DatabaseHelper.SaveOrUpdateGrade(studentId, subjectId, lessonDate, null);
                    _gradesMap.Remove((studentId, lessonDate));
                    row.Cells[e.ColumnIndex].Value = string.Empty;
                    row.Cells[e.ColumnIndex].ToolTipText = string.Empty;
                    RecalculateRowAverage(row);
                    RecalculateClassSummary();
                    lblJournalHint.Text = $"✔ Оценка удалена из БД Access ({DateTime.Now:HH:mm:ss})";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения оценки в БД:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (int.TryParse(input, out int gradeVal) && gradeVal >= 2 && gradeVal <= 5)
            {
                // Valid grade
                try
                {
                    string existingComment = "";
                    if (_gradesMap.TryGetValue((studentId, lessonDate), out var gInfo))
                        existingComment = gInfo.Comment;

                    DatabaseHelper.SaveOrUpdateGrade(studentId, subjectId, lessonDate, gradeVal, existingComment);
                    _gradesMap[(studentId, lessonDate)] = (0, gradeVal, existingComment);
                    row.Cells[e.ColumnIndex].Value = gradeVal.ToString();
                    row.Cells[e.ColumnIndex].ToolTipText = $"Дата: {lessonDate:dd.MM.yyyy}\nОценка: {gradeVal}" +
                        (!string.IsNullOrWhiteSpace(existingComment) ? $"\nПримечание: {existingComment}" : "");
                    RecalculateRowAverage(row);
                    RecalculateClassSummary();
                    lblJournalHint.Text = $"✔ Оценка '{gradeVal}' сохранена в файл базы данных Access ({DateTime.Now:HH:mm:ss})";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения оценки в БД:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Invalid grade
                MessageBox.Show("Оценка должна быть числом от 2 до 5.\nЧтобы удалить оценку, сотрите значение или нажмите Del.", "Некорректный ввод", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (_gradesMap.TryGetValue((studentId, lessonDate), out var oldInfo))
                    row.Cells[e.ColumnIndex].Value = oldInfo.GradeValue.ToString();
                else
                    row.Cells[e.ColumnIndex].Value = string.Empty;
            }
        }

        private void dgvGrades_KeyDown(object sender, KeyEventArgs e)
        {
            bool isStudent = string.Equals(_currentUser.Role, "Ученик", StringComparison.OrdinalIgnoreCase);
            if (isStudent || dgvGrades.CurrentCell == null) return;

            var cell = dgvGrades.CurrentCell;
            var col = dgvGrades.Columns[cell.ColumnIndex];
            if (!(col.Tag is DateTime lessonDate)) return;

            var row = dgvGrades.Rows[cell.RowIndex];
            int studentId = Convert.ToInt32(row.Cells["colStudentId"].Value);
            int subjectId = (cmbFilterSubject.SelectedItem as ComboBoxItem)?.Value ?? 0;
            if (subjectId == 0) return;

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                try
                {
                    DatabaseHelper.SaveOrUpdateGrade(studentId, subjectId, lessonDate, null);
                    _gradesMap.Remove((studentId, lessonDate));
                    cell.Value = string.Empty;
                    cell.ToolTipText = string.Empty;
                    RecalculateRowAverage(row);
                    RecalculateClassSummary();
                    lblJournalHint.Text = $"✔ Оценка удалена из базы данных Access ({DateTime.Now:HH:mm:ss})";
                    e.Handled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка очистки оценки:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if ((e.KeyCode >= Keys.D2 && e.KeyCode <= Keys.D5) || (e.KeyCode >= Keys.NumPad2 && e.KeyCode <= Keys.NumPad5))
            {
                int val = (e.KeyCode >= Keys.D2 && e.KeyCode <= Keys.D5)
                    ? (e.KeyCode - Keys.D0)
                    : (e.KeyCode - Keys.NumPad0);

                try
                {
                    string existingComment = "";
                    if (_gradesMap.TryGetValue((studentId, lessonDate), out var gInfo))
                        existingComment = gInfo.Comment;

                    DatabaseHelper.SaveOrUpdateGrade(studentId, subjectId, lessonDate, val, existingComment);
                    _gradesMap[(studentId, lessonDate)] = (0, val, existingComment);
                    cell.Value = val.ToString();
                    cell.ToolTipText = $"Дата: {lessonDate:dd.MM.yyyy}\nОценка: {val}" +
                        (!string.IsNullOrWhiteSpace(existingComment) ? $"\nПримечание: {existingComment}" : "");
                    RecalculateRowAverage(row);
                    RecalculateClassSummary();
                    lblJournalHint.Text = $"✔ Оценка '{val}' сохранена в базу данных Access ({DateTime.Now:HH:mm:ss})";
                    e.Handled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка выставления оценки:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void menuGrade_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && int.TryParse(item.Tag?.ToString(), out int val))
            {
                SetCurrentCellGrade(val);
            }
        }

        private void menuGradeClear_Click(object sender, EventArgs e)
        {
            SetCurrentCellGrade(null);
        }

        private void menuGradeComment_Click(object sender, EventArgs e)
        {
            if (dgvGrades.CurrentCell == null) return;
            var cell = dgvGrades.CurrentCell;
            var col = dgvGrades.Columns[cell.ColumnIndex];
            if (!(col.Tag is DateTime lessonDate)) return;

            var row = dgvGrades.Rows[cell.RowIndex];
            int studentId = Convert.ToInt32(row.Cells["colStudentId"].Value);
            int subjectId = (cmbFilterSubject.SelectedItem as ComboBoxItem)?.Value ?? 0;
            if (subjectId == 0) return;

            string currentComment = "";
            int? currentVal = null;
            if (_gradesMap.TryGetValue((studentId, lessonDate), out var gInfo))
            {
                currentComment = gInfo.Comment;
                currentVal = gInfo.GradeValue;
            }

            if (!currentVal.HasValue)
            {
                MessageBox.Show("Сначала выставите оценку за этот день, чтобы добавить примечание!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string? input = PromptInputBox("Примечание к оценке", "Введите примечание (например: контрольная, зачет, ответ у доски):", currentComment);
            if (input != null)
            {
                try
                {
                    DatabaseHelper.SaveOrUpdateGrade(studentId, subjectId, lessonDate, currentVal.Value, input.Trim());
                    _gradesMap[(studentId, lessonDate)] = (0, currentVal.Value, input.Trim());
                    cell.ToolTipText = $"Дата: {lessonDate:dd.MM.yyyy}\nОценка: {currentVal.Value}" +
                        (!string.IsNullOrWhiteSpace(input) ? $"\nПримечание: {input.Trim()}" : "");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения примечания:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SetCurrentCellGrade(int? gradeVal)
        {
            if (dgvGrades.CurrentCell == null) return;
            var cell = dgvGrades.CurrentCell;
            var col = dgvGrades.Columns[cell.ColumnIndex];
            if (!(col.Tag is DateTime lessonDate)) return;

            var row = dgvGrades.Rows[cell.RowIndex];
            int studentId = Convert.ToInt32(row.Cells["colStudentId"].Value);
            int subjectId = (cmbFilterSubject.SelectedItem as ComboBoxItem)?.Value ?? 0;
            if (subjectId == 0) return;

            try
            {
                if (gradeVal.HasValue)
                {
                    string existingComment = "";
                    if (_gradesMap.TryGetValue((studentId, lessonDate), out var gInfo))
                        existingComment = gInfo.Comment;

                    DatabaseHelper.SaveOrUpdateGrade(studentId, subjectId, lessonDate, gradeVal.Value, existingComment);
                    _gradesMap[(studentId, lessonDate)] = (0, gradeVal.Value, existingComment);
                    cell.Value = gradeVal.Value.ToString();
                    cell.ToolTipText = $"Дата: {lessonDate:dd.MM.yyyy}\nОценка: {gradeVal.Value}" +
                        (!string.IsNullOrWhiteSpace(existingComment) ? $"\nПримечание: {existingComment}" : "");
                    lblJournalHint.Text = $"✔ Оценка '{gradeVal.Value}' сохранена в базу данных Access ({DateTime.Now:HH:mm:ss})";
                }
                else
                {
                    DatabaseHelper.SaveOrUpdateGrade(studentId, subjectId, lessonDate, null);
                    _gradesMap.Remove((studentId, lessonDate));
                    cell.Value = string.Empty;
                    cell.ToolTipText = string.Empty;
                    lblJournalHint.Text = $"✔ Оценка удалена из базы данных Access ({DateTime.Now:HH:mm:ss})";
                }

                RecalculateRowAverage(row);
                RecalculateClassSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения оценки в БД:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RecalculateRowAverage(DataGridViewRow row)
        {
            var grades = new List<int>();
            for (int d = 0; d < _currentLessonDates.Count; d++)
            {
                var val = row.Cells[3 + d].Value?.ToString();
                if (int.TryParse(val, out int g) && g >= 2 && g <= 5)
                {
                    grades.Add(g);
                }
            }

            int avgColIndex = dgvGrades.Columns["colAvg"].Index;
            row.Cells[avgColIndex].Value = grades.Count > 0 ? grades.Average().ToString("F2") : "—";
        }

        private void RecalculateClassSummary()
        {
            var allGrades = new List<int>();
            foreach (DataGridViewRow row in dgvGrades.Rows)
            {
                for (int d = 0; d < _currentLessonDates.Count; d++)
                {
                    var val = row.Cells[3 + d].Value?.ToString();
                    if (int.TryParse(val, out int g) && g >= 2 && g <= 5)
                    {
                        allGrades.Add(g);
                    }
                }
            }

            int totalStudents = _currentClassStudents.Count;
            int totalLessons = _currentLessonDates.Count;
            string avgStr = allGrades.Count > 0 ? allGrades.Average().ToString("F2") : "0.00";
            lblGradesCount.Text = $"Учеников: {totalStudents} | Занятий: {totalLessons} | Оценок: {allGrades.Count} | Средний балл класса: {avgStr}";
        }

        private void btnAddLessonDate_Click(object sender, EventArgs e)
        {
            string selectedClass = cmbFilterClass.SelectedItem?.ToString() ?? string.Empty;
            int subjectId = (cmbFilterSubject.SelectedItem as ComboBoxItem)?.Value ?? 0;

            if (string.IsNullOrEmpty(selectedClass) || subjectId == 0)
            {
                MessageBox.Show("Сначала выберите класс и предмет для добавления даты занятия!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var form = new AddLessonDateForm();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    DatabaseHelper.AddLessonDate(selectedClass, subjectId, form.SelectedDate, form.Topic);
                    LoadJournalGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавления даты занятия:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteLessonDate_Click(object sender, EventArgs e)
        {
            string selectedClass = cmbFilterClass.SelectedItem?.ToString() ?? string.Empty;
            int subjectId = (cmbFilterSubject.SelectedItem as ComboBoxItem)?.Value ?? 0;

            if (string.IsNullOrEmpty(selectedClass) || subjectId == 0)
            {
                MessageBox.Show("Сначала выберите класс и предмет!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DateTime? targetDate = null;
            if (dgvGrades.CurrentCell != null)
            {
                var col = dgvGrades.Columns[dgvGrades.CurrentCell.ColumnIndex];
                if (col.Tag is DateTime d)
                {
                    targetDate = d;
                }
            }

            if (!targetDate.HasValue)
            {
                if (_currentLessonDates.Count == 0)
                {
                    MessageBox.Show("В выбранном предмете еще нет добавленных дат занятий.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                targetDate = _currentLessonDates.Last();
            }

            var confirm = MessageBox.Show(
                $"Вы действительно хотите удалить столбец даты '{targetDate.Value:dd.MM.yyyy}' для класса '{selectedClass}'?\n\nВсе оценки учеников за этот день будут удалены из базы данных!",
                "Подтверждение удаления даты занятия",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.DeleteLessonDate(selectedClass, subjectId, targetDate.Value);
                    LoadJournalGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления даты занятия:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static string? PromptInputBox(string title, string prompt, string defaultValue = "")
        {
            using var form = new Form
            {
                Width = 420,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            var lbl = new Label { Left = 20, Top = 15, Width = 360, Text = prompt };
            var txt = new TextBox { Left = 20, Top = 45, Width = 360, Text = defaultValue };
            var btnOk = new Button
            {
                Text = "OK",
                Left = 190,
                Width = 90,
                Top = 85,
                DialogResult = DialogResult.OK,
                BackColor = Color.White,
                ForeColor = Color.Black,
                UseVisualStyleBackColor = true
            };
            var btnCancel = new Button
            {
                Text = "Отмена",
                Left = 290,
                Width = 90,
                Top = 85,
                DialogResult = DialogResult.Cancel,
                BackColor = Color.White,
                ForeColor = Color.Black,
                UseVisualStyleBackColor = true
            };

            form.Controls.Add(lbl);
            form.Controls.Add(txt);
            form.Controls.Add(btnOk);
            form.Controls.Add(btnCancel);
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
        }

        // ==================== STUDENTS LOGIC ====================

        private void LoadStudents()
        {
            try
            {
                _studentsTable = DatabaseHelper.GetStudentsTable();
                dgvStudents.BindingContext = this.BindingContext;
                dgvStudents.DataSource = _studentsTable;

                FormatStudentsColumns();

                if (_studentsTable != null)
                    lblStudentsCount.Text = $"Всего учеников: {_studentsTable.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки списка учеников:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearchStudent_TextChanged(object sender, EventArgs e)
        {
            if (_studentsTable == null) return;
            string filter = txtSearchStudent.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(filter))
            {
                _studentsTable.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                _studentsTable.DefaultView.RowFilter = $"[ФИО ученика] LIKE '%{filter}%' OR [Класс] LIKE '%{filter}%'";
            }
            lblStudentsCount.Text = $"Найдено учеников: {_studentsTable.DefaultView.Count}";
        }

        private void btnRefreshStudents_Click(object sender, EventArgs e)
        {
            txtSearchStudent.Clear();
            LoadStudents();
            LoadFilterCombos();
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            using var form = new StudentEditForm();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                LoadStudents();
                LoadFilterCombos();
                LoadJournalGrid();
            }
        }

        private void btnEditStudent_Click(object sender, EventArgs e)
        {
            if (string.Equals(_currentUser.Role, "Ученик", StringComparison.OrdinalIgnoreCase))
                return;

            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите ученика для редактирования!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvStudents.SelectedRows[0];
            object? idVal = (dgvStudents.Columns.Contains("Код") && row.Cells["Код"] != null)
                ? row.Cells["Код"].Value
                : (row.Cells.Count > 0 ? row.Cells[0].Value : null);

            if (idVal == null || idVal == DBNull.Value) return;
            int studentId = Convert.ToInt32(idVal);

            string fullName = (dgvStudents.Columns.Contains("ФИО ученика") && row.Cells["ФИО ученика"] != null)
                ? (row.Cells["ФИО ученика"].Value?.ToString() ?? string.Empty)
                : string.Empty;
            string classGrade = (dgvStudents.Columns.Contains("Класс") && row.Cells["Класс"] != null)
                ? (row.Cells["Класс"].Value?.ToString() ?? string.Empty)
                : string.Empty;
            string phone = (dgvStudents.Columns.Contains("Телефон") && row.Cells["Телефон"] != null)
                ? (row.Cells["Телефон"].Value?.ToString() ?? string.Empty)
                : string.Empty;

            var student = new Student
            {
                StudentID = studentId,
                FullName = fullName,
                ClassGrade = classGrade,
                Phone = phone
            };

            using var form = new StudentEditForm(student);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                LoadStudents();
                LoadFilterCombos();
                LoadJournalGrid();
            }
        }

        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите ученика для удаления!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvStudents.SelectedRows[0];
            object? idVal = (dgvStudents.Columns.Contains("Код") && row.Cells["Код"] != null)
                ? row.Cells["Код"].Value
                : (row.Cells.Count > 0 ? row.Cells[0].Value : null);

            if (idVal == null || idVal == DBNull.Value) return;
            int studentId = Convert.ToInt32(idVal);

            string fullName = (dgvStudents.Columns.Contains("ФИО ученика") && row.Cells["ФИО ученика"] != null)
                ? (row.Cells["ФИО ученика"].Value?.ToString() ?? string.Empty)
                : string.Empty;

            var confirm = MessageBox.Show(
                $"Внимание! Удаление ученика '{fullName}' также удалит все выставленные ему оценки из базы данных.\n\nПродолжить?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.DeleteStudent(studentId);
                    LoadStudents();
                    LoadFilterCombos();
                    LoadJournalGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении ученика:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pnlTop_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(220, 220, 220));
            e.Graphics.DrawLine(pen, 0, pnlTop.Height - 1, pnlTop.Width, pnlTop.Height - 1);
        }

        private class ComboBoxItem
        {
            public string Text { get; set; } = string.Empty;
            public int Value { get; set; }
            public override string ToString() => Text;
        }
    }
}
