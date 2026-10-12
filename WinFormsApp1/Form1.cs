using BL;
using Model;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly BusinessLogic logic = new BusinessLogic();
        public Form1()
        {
            InitializeComponent();
            RefreshTable();
        }
        private void RefreshTable()
        {
            dgvStudents.DataSource = null;
            dgvStudents.DataSource = logic.GetStudents().ToList();
            dgvStudents.ClearSelection();
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            using var form = new AddStudentForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                bool added = logic.AddStudent(form.StudentName, form.Speciality, form.Group);

                if (!added)
                {
                    MessageBox.Show("Заполните все поля корректно");
                    return;
                }
                RefreshTable();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show("Выберите студента в таблице");
                return;
            }

            logic.DeleteStudent(dgvStudents.CurrentRow.Index);
            RefreshTable();
        }

        private void buttonHistogram_Click(object sender, EventArgs e)
        {
            new HistogramForm(logic.GetHistogram()).Show();
        }
    }
}
