using BL;


namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly BusinessLogic logic = new BusinessLogic();

        public Form1()
        {
            InitializeComponent();
            logic.AddTestStudents();
            RefreshTable();
        }
        private void RefreshTable()
        {
            if (dgvStudents.Columns.Count == 0)
            {
                dgvStudents.ColumnCount = 3;
                dgvStudents.Columns[0].Name = "ФИО";
                dgvStudents.Columns[1].Name = "Специальность";
                dgvStudents.Columns[2].Name = "Группа";
                dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            dgvStudents.Rows.Clear();
            string[][] studentsData = logic.GetStudents();
            foreach (var studentRow in studentsData)
            {
                dgvStudents.Rows.Add(studentRow);
            }
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
