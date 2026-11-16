using BL;


namespace WinFormsApp1
{
    /// <summary>
    /// Главная форма: таблица студентов, кнопки, выбор реализации доступа к данным.
    /// </summary>
    public partial class Form1 : Form
    {
        private BusinessLogic? logic;

        // выбор реализации доступа к данным: EF Core или Dapper
        private readonly ComboBox cmbMode = new() { Left = 40, Top = 128, Width = 220 };
        private readonly Label lblMode = new() { Left = 270, Top = 134, Width = 420, Text = "Реализация доступа к данным" };

        /// <summary>
        /// Создаёт форму, добавляет список выбора EF Core/Dapper и подключается к базе.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            cmbMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMode.Items.Add("EF Core");
            cmbMode.Items.Add("Dapper");
            Controls.Add(cmbMode);
            Controls.Add(lblMode);

            cmbMode.SelectedIndex = 0;
            cmbMode.SelectedIndexChanged += (s, e) => Connect();
            Connect();
        }

        /// <summary>
        /// Создаёт BusinessLogic для выбранного режима и обновляет таблицу. При ошибке подключения показывает сообщение.
        /// </summary>
        private void Connect()
        {
            try
            {
                logic = new BusinessLogic(cmbMode.SelectedIndex == 1 ? StorageMode.Dapper : StorageMode.EfCore);
                RefreshTable();
            }
            catch (Exception ex)
            {
                logic = null;
                dgvStudents.Rows.Clear();
                MessageBox.Show(
                    "Не удалось подключиться к базе данных:\n\n" + (ex.InnerException ?? ex).Message,
                    "Ошибка БД");
            }
        }
        /// <summary>
        /// Перечитывает студентов из логики и заполняет таблицу.
        /// </summary>
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
            if (logic == null) return;
            string[][] studentsData = logic.GetStudents();
            foreach (var studentRow in studentsData)
            {
                dgvStudents.Rows.Add(studentRow);
            }
            dgvStudents.ClearSelection();
        }
        /// <summary>
        /// Открывает диалог добавления и передаёт введённые данные в бизнес-логику.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (logic == null) return;
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

        /// <summary>
        /// Удаляет студента, выбранного в таблице.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (logic == null) return;
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show("Выберите студента в таблице");
                return;
            }

            logic.DeleteStudent(dgvStudents.CurrentRow.Index);
            RefreshTable();
        }

        /// <summary>
        /// Открывает окно гистограммы по специальностям.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void buttonHistogram_Click(object sender, EventArgs e)
        {
            if (logic == null) return;

            using var form = new HistogramForm(logic.GetHistogram());
            form.ShowDialog(this);
        }
    }
}
