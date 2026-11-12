namespace SchoolDiary.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnExitApp = new System.Windows.Forms.Button();
            this.btnSwitchUser = new System.Windows.Forms.Button();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGrades = new System.Windows.Forms.TabPage();
            this.dgvGrades = new System.Windows.Forms.DataGridView();
            this.pnlGradesBottom = new System.Windows.Forms.Panel();
            this.lblGradesCount = new System.Windows.Forms.Label();
            this.lblJournalHint = new System.Windows.Forms.Label();
            this.pnlGradesFilter = new System.Windows.Forms.Panel();
            this.btnRefreshGrades = new System.Windows.Forms.Button();
            this.btnDeleteLessonDate = new System.Windows.Forms.Button();
            this.btnAddLessonDate = new System.Windows.Forms.Button();
            this.cmbFilterSubject = new System.Windows.Forms.ComboBox();
            this.lblFilterSubject = new System.Windows.Forms.Label();
            this.cmbFilterClass = new System.Windows.Forms.ComboBox();
            this.lblFilterClass = new System.Windows.Forms.Label();
            this.contextMenuGrade = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuGrade5 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGrade4 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGrade3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGrade2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGradeSep = new System.Windows.Forms.ToolStripSeparator();
            this.menuGradeClear = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGradeComment = new System.Windows.Forms.ToolStripMenuItem();
            this.tabStudents = new System.Windows.Forms.TabPage();
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.pnlStudentsBottom = new System.Windows.Forms.Panel();
            this.lblStudentsCount = new System.Windows.Forms.Label();
            this.btnDeleteStudent = new System.Windows.Forms.Button();
            this.btnEditStudent = new System.Windows.Forms.Button();
            this.btnAddStudent = new System.Windows.Forms.Button();
            this.pnlStudentsSearch = new System.Windows.Forms.Panel();
            this.btnRefreshStudents = new System.Windows.Forms.Button();
            this.txtSearchStudent = new System.Windows.Forms.TextBox();
            this.lblSearchStudent = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabGrades.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrades)).BeginInit();
            this.pnlGradesBottom.SuspendLayout();
            this.pnlGradesFilter.SuspendLayout();
            this.contextMenuGrade.SuspendLayout();
            this.tabStudents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.pnlStudentsBottom.SuspendLayout();
            this.pnlStudentsSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.btnExitApp);
            this.pnlTop.Controls.Add(this.btnSwitchUser);
            this.pnlTop.Controls.Add(this.lblUserInfo);
            this.pnlTop.Controls.Add(this.lblAppTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1000, 52);
            this.pnlTop.TabIndex = 0;
            this.pnlTop.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTop_Paint);
            // 
            // btnExitApp
            // 
            this.btnExitApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExitApp.BackColor = System.Drawing.Color.White;
            this.btnExitApp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExitApp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnExitApp.ForeColor = System.Drawing.Color.Black;
            this.btnExitApp.Location = new System.Drawing.Point(895, 10);
            this.btnExitApp.Name = "btnExitApp";
            this.btnExitApp.Size = new System.Drawing.Size(95, 32);
            this.btnExitApp.TabIndex = 3;
            this.btnExitApp.Text = "Выход";
            this.btnExitApp.UseVisualStyleBackColor = true;
            this.btnExitApp.Click += new System.EventHandler(this.btnExitApp_Click);
            // 
            // btnSwitchUser
            // 
            this.btnSwitchUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSwitchUser.BackColor = System.Drawing.Color.White;
            this.btnSwitchUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSwitchUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSwitchUser.ForeColor = System.Drawing.Color.Black;
            this.btnSwitchUser.Location = new System.Drawing.Point(745, 10);
            this.btnSwitchUser.Name = "btnSwitchUser";
            this.btnSwitchUser.Size = new System.Drawing.Size(140, 32);
            this.btnSwitchUser.TabIndex = 2;
            this.btnSwitchUser.Text = "Сменить вход";
            this.btnSwitchUser.UseVisualStyleBackColor = true;
            this.btnSwitchUser.Click += new System.EventHandler(this.btnSwitchUser_Click);
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUserInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.lblUserInfo.Location = new System.Drawing.Point(380, 15);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(350, 23);
            this.lblUserInfo.TabIndex = 1;
            this.lblUserInfo.Text = "Пользователь: Загрузка...";
            this.lblUserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAppTitle.ForeColor = System.Drawing.Color.Black;
            this.lblAppTitle.Location = new System.Drawing.Point(15, 13);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(265, 28);
            this.lblAppTitle.TabIndex = 0;
            this.lblAppTitle.Text = "Электронный дневник v2.0";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGrades);
            this.tabControl.Controls.Add(this.tabStudents);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tabControl.Location = new System.Drawing.Point(0, 52);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1000, 628);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabGrades
            // 
            this.tabGrades.Controls.Add(this.dgvGrades);
            this.tabGrades.Controls.Add(this.pnlGradesBottom);
            this.tabGrades.Controls.Add(this.pnlGradesFilter);
            this.tabGrades.Location = new System.Drawing.Point(4, 30);
            this.tabGrades.Name = "tabGrades";
            this.tabGrades.Padding = new System.Windows.Forms.Padding(3);
            this.tabGrades.Size = new System.Drawing.Size(992, 594);
            this.tabGrades.TabIndex = 0;
            this.tabGrades.Text = " Классный журнал ";
            this.tabGrades.UseVisualStyleBackColor = true;
            // 
            // dgvGrades
            // 
            this.dgvGrades.AllowUserToAddRows = false;
            this.dgvGrades.AllowUserToDeleteRows = false;
            this.dgvGrades.BackgroundColor = System.Drawing.Color.White;
            this.dgvGrades.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGrades.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGrades.ColumnHeadersHeight = 36;
            this.dgvGrades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGrades.ContextMenuStrip = this.contextMenuGrade;
            this.dgvGrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGrades.EnableHeadersVisualStyles = false;
            this.dgvGrades.Location = new System.Drawing.Point(3, 53);
            this.dgvGrades.MultiSelect = false;
            this.dgvGrades.Name = "dgvGrades";
            this.dgvGrades.RowHeadersVisible = false;
            this.dgvGrades.RowHeadersWidth = 51;
            this.dgvGrades.RowTemplate.Height = 30;
            this.dgvGrades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvGrades.ShowCellToolTips = true;
            this.dgvGrades.Size = new System.Drawing.Size(986, 491);
            this.dgvGrades.TabIndex = 1;
            this.dgvGrades.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGrades_CellEndEdit);
            this.dgvGrades.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvGrades_CellFormatting);
            this.dgvGrades.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGrades_CellMouseDown);
            this.dgvGrades.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvGrades_KeyDown);
            // 
            // pnlGradesBottom
            // 
            this.pnlGradesBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.pnlGradesBottom.Controls.Add(this.lblGradesCount);
            this.pnlGradesBottom.Controls.Add(this.lblJournalHint);
            this.pnlGradesBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlGradesBottom.Location = new System.Drawing.Point(3, 544);
            this.pnlGradesBottom.Name = "pnlGradesBottom";
            this.pnlGradesBottom.Size = new System.Drawing.Size(986, 47);
            this.pnlGradesBottom.TabIndex = 2;
            // 
            // lblGradesCount
            // 
            this.lblGradesCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGradesCount.AutoSize = true;
            this.lblGradesCount.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblGradesCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblGradesCount.Location = new System.Drawing.Point(540, 13);
            this.lblGradesCount.Name = "lblGradesCount";
            this.lblGradesCount.Size = new System.Drawing.Size(430, 21);
            this.lblGradesCount.TabIndex = 1;
            this.lblGradesCount.Text = "Учеников: 0 | Занятий: 0 | Средний балл класса: 0.00";
            this.lblGradesCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblJournalHint
            // 
            this.lblJournalHint.AutoSize = true;
            this.lblJournalHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblJournalHint.Location = new System.Drawing.Point(12, 13);
            this.lblJournalHint.Name = "lblJournalHint";
            this.lblJournalHint.Size = new System.Drawing.Size(490, 21);
            this.lblJournalHint.TabIndex = 0;
            this.lblJournalHint.Text = "Выберите ячейку и введите 2-5 (Del — очистить). Правый клик — меню.";
            // 
            // pnlGradesFilter
            // 
            this.pnlGradesFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.pnlGradesFilter.Controls.Add(this.btnRefreshGrades);
            this.pnlGradesFilter.Controls.Add(this.btnDeleteLessonDate);
            this.pnlGradesFilter.Controls.Add(this.btnAddLessonDate);
            this.pnlGradesFilter.Controls.Add(this.cmbFilterSubject);
            this.pnlGradesFilter.Controls.Add(this.lblFilterSubject);
            this.pnlGradesFilter.Controls.Add(this.cmbFilterClass);
            this.pnlGradesFilter.Controls.Add(this.lblFilterClass);
            this.pnlGradesFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGradesFilter.Location = new System.Drawing.Point(3, 3);
            this.pnlGradesFilter.Name = "pnlGradesFilter";
            this.pnlGradesFilter.Size = new System.Drawing.Size(986, 50);
            this.pnlGradesFilter.TabIndex = 0;
            // 
            // btnRefreshGrades
            // 
            this.btnRefreshGrades.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshGrades.BackColor = System.Drawing.Color.White;
            this.btnRefreshGrades.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshGrades.ForeColor = System.Drawing.Color.Black;
            this.btnRefreshGrades.Location = new System.Drawing.Point(865, 9);
            this.btnRefreshGrades.Name = "btnRefreshGrades";
            this.btnRefreshGrades.Size = new System.Drawing.Size(110, 32);
            this.btnRefreshGrades.TabIndex = 6;
            this.btnRefreshGrades.Text = "Обновить";
            this.btnRefreshGrades.UseVisualStyleBackColor = true;
            this.btnRefreshGrades.Click += new System.EventHandler(this.btnRefreshGrades_Click);
            // 
            // btnDeleteLessonDate
            // 
            this.btnDeleteLessonDate.BackColor = System.Drawing.Color.White;
            this.btnDeleteLessonDate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteLessonDate.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteLessonDate.Location = new System.Drawing.Point(715, 9);
            this.btnDeleteLessonDate.Name = "btnDeleteLessonDate";
            this.btnDeleteLessonDate.Size = new System.Drawing.Size(130, 32);
            this.btnDeleteLessonDate.TabIndex = 5;
            this.btnDeleteLessonDate.Text = "Удалить дату";
            this.btnDeleteLessonDate.UseVisualStyleBackColor = true;
            this.btnDeleteLessonDate.Click += new System.EventHandler(this.btnDeleteLessonDate_Click);
            // 
            // btnAddLessonDate
            // 
            this.btnAddLessonDate.BackColor = System.Drawing.Color.White;
            this.btnAddLessonDate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddLessonDate.ForeColor = System.Drawing.Color.Black;
            this.btnAddLessonDate.Location = new System.Drawing.Point(555, 9);
            this.btnAddLessonDate.Name = "btnAddLessonDate";
            this.btnAddLessonDate.Size = new System.Drawing.Size(150, 32);
            this.btnAddLessonDate.TabIndex = 4;
            this.btnAddLessonDate.Text = "+ Добавить дату";
            this.btnAddLessonDate.UseVisualStyleBackColor = true;
            this.btnAddLessonDate.Click += new System.EventHandler(this.btnAddLessonDate_Click);
            // 
            // cmbFilterSubject
            // 
            this.cmbFilterSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterSubject.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbFilterSubject.FormattingEnabled = true;
            this.cmbFilterSubject.Location = new System.Drawing.Point(280, 11);
            this.cmbFilterSubject.Name = "cmbFilterSubject";
            this.cmbFilterSubject.Size = new System.Drawing.Size(260, 29);
            this.cmbFilterSubject.TabIndex = 3;
            this.cmbFilterSubject.SelectedIndexChanged += new System.EventHandler(this.cmbFilterSubject_SelectedIndexChanged);
            // 
            // lblFilterSubject
            // 
            this.lblFilterSubject.AutoSize = true;
            this.lblFilterSubject.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblFilterSubject.ForeColor = System.Drawing.Color.Black;
            this.lblFilterSubject.Location = new System.Drawing.Point(205, 14);
            this.lblFilterSubject.Name = "lblFilterSubject";
            this.lblFilterSubject.Size = new System.Drawing.Size(76, 21);
            this.lblFilterSubject.TabIndex = 2;
            this.lblFilterSubject.Text = "Предмет:";
            // 
            // cmbFilterClass
            // 
            this.cmbFilterClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterClass.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbFilterClass.FormattingEnabled = true;
            this.cmbFilterClass.Location = new System.Drawing.Point(68, 11);
            this.cmbFilterClass.Name = "cmbFilterClass";
            this.cmbFilterClass.Size = new System.Drawing.Size(120, 29);
            this.cmbFilterClass.TabIndex = 1;
            this.cmbFilterClass.SelectedIndexChanged += new System.EventHandler(this.cmbFilterClass_SelectedIndexChanged);
            // 
            // lblFilterClass
            // 
            this.lblFilterClass.AutoSize = true;
            this.lblFilterClass.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblFilterClass.ForeColor = System.Drawing.Color.Black;
            this.lblFilterClass.Location = new System.Drawing.Point(12, 14);
            this.lblFilterClass.Name = "lblFilterClass";
            this.lblFilterClass.Size = new System.Drawing.Size(54, 21);
            this.lblFilterClass.TabIndex = 0;
            this.lblFilterClass.Text = "Класс:";
            // 
            // contextMenuGrade
            // 
            this.contextMenuGrade.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuGrade.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuGrade5,
            this.menuGrade4,
            this.menuGrade3,
            this.menuGrade2,
            this.menuGradeSep,
            this.menuGradeClear,
            this.menuGradeComment});
            this.contextMenuGrade.Name = "contextMenuGrade";
            this.contextMenuGrade.Size = new System.Drawing.Size(271, 190);
            // 
            // menuGrade5
            // 
            this.menuGrade5.Name = "menuGrade5";
            this.menuGrade5.Size = new System.Drawing.Size(270, 26);
            this.menuGrade5.Tag = "5";
            this.menuGrade5.Text = "Оценка 5 (Отлично)";
            this.menuGrade5.Click += new System.EventHandler(this.menuGrade_Click);
            // 
            // menuGrade4
            // 
            this.menuGrade4.Name = "menuGrade4";
            this.menuGrade4.Size = new System.Drawing.Size(270, 26);
            this.menuGrade4.Tag = "4";
            this.menuGrade4.Text = "Оценка 4 (Хорошо)";
            this.menuGrade4.Click += new System.EventHandler(this.menuGrade_Click);
            // 
            // menuGrade3
            // 
            this.menuGrade3.Name = "menuGrade3";
            this.menuGrade3.Size = new System.Drawing.Size(270, 26);
            this.menuGrade3.Tag = "3";
            this.menuGrade3.Text = "Оценка 3 (Удовлетворительно)";
            this.menuGrade3.Click += new System.EventHandler(this.menuGrade_Click);
            // 
            // menuGrade2
            // 
            this.menuGrade2.Name = "menuGrade2";
            this.menuGrade2.Size = new System.Drawing.Size(270, 26);
            this.menuGrade2.Tag = "2";
            this.menuGrade2.Text = "Оценка 2 (Неудовлетворительно)";
            this.menuGrade2.Click += new System.EventHandler(this.menuGrade_Click);
            // 
            // menuGradeSep
            // 
            this.menuGradeSep.Name = "menuGradeSep";
            this.menuGradeSep.Size = new System.Drawing.Size(267, 6);
            // 
            // menuGradeClear
            // 
            this.menuGradeClear.Name = "menuGradeClear";
            this.menuGradeClear.Size = new System.Drawing.Size(270, 26);
            this.menuGradeClear.Text = "Очистить ячейку (Del)";
            this.menuGradeClear.Click += new System.EventHandler(this.menuGradeClear_Click);
            // 
            // menuGradeComment
            // 
            this.menuGradeComment.Name = "menuGradeComment";
            this.menuGradeComment.Size = new System.Drawing.Size(270, 26);
            this.menuGradeComment.Text = "Добавить примечание...";
            this.menuGradeComment.Click += new System.EventHandler(this.menuGradeComment_Click);
            // 
            // tabStudents
            // 
            this.tabStudents.Controls.Add(this.dgvStudents);
            this.tabStudents.Controls.Add(this.pnlStudentsBottom);
            this.tabStudents.Controls.Add(this.pnlStudentsSearch);
            this.tabStudents.Location = new System.Drawing.Point(4, 30);
            this.tabStudents.Name = "tabStudents";
            this.tabStudents.Padding = new System.Windows.Forms.Padding(3);
            this.tabStudents.Size = new System.Drawing.Size(992, 594);
            this.tabStudents.TabIndex = 1;
            this.tabStudents.Text = " Список учеников ";
            this.tabStudents.UseVisualStyleBackColor = true;
            // 
            // dgvStudents
            // 
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AllowUserToDeleteRows = false;
            this.dgvStudents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStudents.BackgroundColor = System.Drawing.Color.White;
            this.dgvStudents.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStudents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvStudents.ColumnHeadersHeight = 34;
            this.dgvStudents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStudents.EnableHeadersVisualStyles = false;
            this.dgvStudents.Location = new System.Drawing.Point(3, 53);
            this.dgvStudents.MultiSelect = false;
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.RowHeadersVisible = false;
            this.dgvStudents.RowHeadersWidth = 51;
            this.dgvStudents.RowTemplate.Height = 30;
            this.dgvStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudents.Size = new System.Drawing.Size(986, 491);
            this.dgvStudents.TabIndex = 1;
            this.dgvStudents.DoubleClick += new System.EventHandler(this.btnEditStudent_Click);
            // 
            // pnlStudentsBottom
            // 
            this.pnlStudentsBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.pnlStudentsBottom.Controls.Add(this.lblStudentsCount);
            this.pnlStudentsBottom.Controls.Add(this.btnDeleteStudent);
            this.pnlStudentsBottom.Controls.Add(this.btnEditStudent);
            this.pnlStudentsBottom.Controls.Add(this.btnAddStudent);
            this.pnlStudentsBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStudentsBottom.Location = new System.Drawing.Point(3, 544);
            this.pnlStudentsBottom.Name = "pnlStudentsBottom";
            this.pnlStudentsBottom.Size = new System.Drawing.Size(986, 47);
            this.pnlStudentsBottom.TabIndex = 2;
            // 
            // lblStudentsCount
            // 
            this.lblStudentsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStudentsCount.AutoSize = true;
            this.lblStudentsCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblStudentsCount.Location = new System.Drawing.Point(790, 14);
            this.lblStudentsCount.Name = "lblStudentsCount";
            this.lblStudentsCount.Size = new System.Drawing.Size(135, 21);
            this.lblStudentsCount.TabIndex = 3;
            this.lblStudentsCount.Text = "Всего учеников: 0";
            // 
            // btnDeleteStudent
            // 
            this.btnDeleteStudent.BackColor = System.Drawing.Color.White;
            this.btnDeleteStudent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteStudent.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDeleteStudent.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteStudent.Location = new System.Drawing.Point(365, 7);
            this.btnDeleteStudent.Name = "btnDeleteStudent";
            this.btnDeleteStudent.Size = new System.Drawing.Size(155, 34);
            this.btnDeleteStudent.TabIndex = 2;
            this.btnDeleteStudent.Text = "Удалить из БД";
            this.btnDeleteStudent.UseVisualStyleBackColor = true;
            this.btnDeleteStudent.Click += new System.EventHandler(this.btnDeleteStudent_Click);
            // 
            // btnEditStudent
            // 
            this.btnEditStudent.BackColor = System.Drawing.Color.White;
            this.btnEditStudent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditStudent.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnEditStudent.ForeColor = System.Drawing.Color.Black;
            this.btnEditStudent.Location = new System.Drawing.Point(195, 7);
            this.btnEditStudent.Name = "btnEditStudent";
            this.btnEditStudent.Size = new System.Drawing.Size(155, 34);
            this.btnEditStudent.TabIndex = 1;
            this.btnEditStudent.Text = "Редактировать";
            this.btnEditStudent.UseVisualStyleBackColor = true;
            this.btnEditStudent.Click += new System.EventHandler(this.btnEditStudent_Click);
            // 
            // btnAddStudent
            // 
            this.btnAddStudent.BackColor = System.Drawing.Color.White;
            this.btnAddStudent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddStudent.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAddStudent.ForeColor = System.Drawing.Color.Black;
            this.btnAddStudent.Location = new System.Drawing.Point(12, 7);
            this.btnAddStudent.Name = "btnAddStudent";
            this.btnAddStudent.Size = new System.Drawing.Size(165, 34);
            this.btnAddStudent.TabIndex = 0;
            this.btnAddStudent.Text = "+ Новый ученик";
            this.btnAddStudent.UseVisualStyleBackColor = true;
            this.btnAddStudent.Click += new System.EventHandler(this.btnAddStudent_Click);
            // 
            // pnlStudentsSearch
            // 
            this.pnlStudentsSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.pnlStudentsSearch.Controls.Add(this.btnRefreshStudents);
            this.pnlStudentsSearch.Controls.Add(this.txtSearchStudent);
            this.pnlStudentsSearch.Controls.Add(this.lblSearchStudent);
            this.pnlStudentsSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStudentsSearch.Location = new System.Drawing.Point(3, 3);
            this.pnlStudentsSearch.Name = "pnlStudentsSearch";
            this.pnlStudentsSearch.Size = new System.Drawing.Size(986, 50);
            this.pnlStudentsSearch.TabIndex = 0;
            // 
            // btnRefreshStudents
            // 
            this.btnRefreshStudents.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshStudents.BackColor = System.Drawing.Color.White;
            this.btnRefreshStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshStudents.ForeColor = System.Drawing.Color.Black;
            this.btnRefreshStudents.Location = new System.Drawing.Point(825, 9);
            this.btnRefreshStudents.Name = "btnRefreshStudents";
            this.btnRefreshStudents.Size = new System.Drawing.Size(150, 32);
            this.btnRefreshStudents.TabIndex = 2;
            this.btnRefreshStudents.Text = "Сброс и обновление";
            this.btnRefreshStudents.UseVisualStyleBackColor = true;
            this.btnRefreshStudents.Click += new System.EventHandler(this.btnRefreshStudents_Click);
            // 
            // txtSearchStudent
            // 
            this.txtSearchStudent.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSearchStudent.Location = new System.Drawing.Point(145, 11);
            this.txtSearchStudent.Name = "txtSearchStudent";
            this.txtSearchStudent.PlaceholderText = "Введите часть ФИО или класс для фильтрации...";
            this.txtSearchStudent.Size = new System.Drawing.Size(380, 29);
            this.txtSearchStudent.TabIndex = 1;
            this.txtSearchStudent.TextChanged += new System.EventHandler(this.txtSearchStudent_TextChanged);
            // 
            // lblSearchStudent
            // 
            this.lblSearchStudent.AutoSize = true;
            this.lblSearchStudent.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSearchStudent.ForeColor = System.Drawing.Color.Black;
            this.lblSearchStudent.Location = new System.Drawing.Point(15, 14);
            this.lblSearchStudent.Name = "lblSearchStudent";
            this.lblSearchStudent.Size = new System.Drawing.Size(121, 21);
            this.lblSearchStudent.TabIndex = 0;
            this.lblSearchStudent.Text = "Поиск ученика:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1000, 680);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlTop);
            this.MinimumSize = new System.Drawing.Size(850, 550);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Электронный дневник v2.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabGrades.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrades)).EndInit();
            this.pnlGradesBottom.ResumeLayout(false);
            this.pnlGradesBottom.PerformLayout();
            this.pnlGradesFilter.ResumeLayout(false);
            this.pnlGradesFilter.PerformLayout();
            this.contextMenuGrade.ResumeLayout(false);
            this.tabStudents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.pnlStudentsBottom.ResumeLayout(false);
            this.pnlStudentsBottom.PerformLayout();
            this.pnlStudentsSearch.ResumeLayout(false);
            this.pnlStudentsSearch.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Button btnSwitchUser;
        private System.Windows.Forms.Button btnExitApp;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGrades;
        private System.Windows.Forms.Panel pnlGradesFilter;
        private System.Windows.Forms.Label lblFilterClass;
        private System.Windows.Forms.ComboBox cmbFilterClass;
        private System.Windows.Forms.Label lblFilterSubject;
        private System.Windows.Forms.ComboBox cmbFilterSubject;
        private System.Windows.Forms.Button btnAddLessonDate;
        private System.Windows.Forms.Button btnDeleteLessonDate;
        private System.Windows.Forms.Button btnRefreshGrades;
        private System.Windows.Forms.DataGridView dgvGrades;
        private System.Windows.Forms.Panel pnlGradesBottom;
        private System.Windows.Forms.Label lblJournalHint;
        private System.Windows.Forms.Label lblGradesCount;
        private System.Windows.Forms.ContextMenuStrip contextMenuGrade;
        private System.Windows.Forms.ToolStripMenuItem menuGrade5;
        private System.Windows.Forms.ToolStripMenuItem menuGrade4;
        private System.Windows.Forms.ToolStripMenuItem menuGrade3;
        private System.Windows.Forms.ToolStripMenuItem menuGrade2;
        private System.Windows.Forms.ToolStripSeparator menuGradeSep;
        private System.Windows.Forms.ToolStripMenuItem menuGradeClear;
        private System.Windows.Forms.ToolStripMenuItem menuGradeComment;
        private System.Windows.Forms.TabPage tabStudents;
        private System.Windows.Forms.Panel pnlStudentsSearch;
        private System.Windows.Forms.Label lblSearchStudent;
        private System.Windows.Forms.TextBox txtSearchStudent;
        private System.Windows.Forms.Button btnRefreshStudents;
        private System.Windows.Forms.DataGridView dgvStudents;
        private System.Windows.Forms.Panel pnlStudentsBottom;
        private System.Windows.Forms.Button btnAddStudent;
        private System.Windows.Forms.Button btnEditStudent;
        private System.Windows.Forms.Button btnDeleteStudent;
        private System.Windows.Forms.Label lblStudentsCount;
    }
}
