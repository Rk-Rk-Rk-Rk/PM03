using System;
using System.Windows.Forms;

namespace SchoolDiary.Forms
{
    public partial class AddLessonDateForm : Form
    {
        public DateTime SelectedDate => dtpLessonDate.Value.Date;
        public string Topic => txtTopic.Text.Trim();

        public AddLessonDateForm(DateTime? defaultDate = null)
        {
            InitializeComponent();
            dtpLessonDate.Value = defaultDate ?? DateTime.Today;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
