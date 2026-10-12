using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp1
{
    internal class AddStudentForm:Form
    {
        private readonly TextBox txtName = new() { Left = 170, Top = 20, Width = 360 };
        private readonly TextBox txtSpeciality = new() { Left = 170, Top = 65, Width = 360 };
        private readonly TextBox txtGroup = new() { Left = 170, Top = 110, Width = 360 };
        private readonly Button btnOk = new() { Text = "OK", Left = 150, Top = 160, Width = 120, Height = 35 };
        private readonly Button btnCancel = new() { Text = "Отмена", Left = 290, Top = 160, Width = 120, Height = 35 };

        public string StudentName => txtName.Text.Trim();
        public string Speciality => txtSpeciality.Text.Trim();
        public string Group => txtGroup.Text.Trim();

        public AddStudentForm()
        {
            Text = "Добавить студента";
            ClientSize = new Size(540, 220);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            Controls.Add(new Label { Text = "ФИО", Left = 20, Top = 23, Width = 120 });
            Controls.Add(new Label { Text = "Специальность", Left = 20, Top = 68, Width = 150 });
            Controls.Add(new Label { Text = "Группа", Left = 20, Top = 113, Width = 120 });
            Controls.AddRange(new Control[] { txtName, txtSpeciality, txtGroup, btnOk, btnCancel });

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            btnCancel.DialogResult = DialogResult.Cancel;

            btnOk.Click += (s, e) =>
            {
                if (StudentName == "" || Speciality == "" || Group == "")
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }
                DialogResult = DialogResult.OK;
            };
        }
    }
}
