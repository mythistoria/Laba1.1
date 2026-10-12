namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonAdd = new Button();
            buttonDelete = new Button();
            buttonHistogram = new Button();
            dgvStudents = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvStudents).BeginInit();
            SuspendLayout();
            // 
            // buttonAdd
            // 
            buttonAdd.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 204);
            buttonAdd.Location = new Point(40, 24);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(300, 96);
            buttonAdd.TabIndex = 0;
            buttonAdd.Text = "Добавить студента";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 204);
            buttonDelete.Location = new Point(401, 24);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(300, 96);
            buttonDelete.TabIndex = 1;
            buttonDelete.Text = "Удалить студента";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // buttonHistogram
            // 
            buttonHistogram.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 204);
            buttonHistogram.Location = new Point(765, 24);
            buttonHistogram.Name = "buttonHistogram";
            buttonHistogram.Size = new Size(300, 96);
            buttonHistogram.TabIndex = 2;
            buttonHistogram.Text = "Гистограмма";
            buttonHistogram.UseVisualStyleBackColor = true;
            buttonHistogram.Click += buttonHistogram_Click;
            // 
            // dgvStudents
            // 
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStudents.Dock = DockStyle.Bottom;
            dgvStudents.Location = new Point(0, 169);
            dgvStudents.MultiSelect = false;
            dgvStudents.Name = "dgvStudents";
            dgvStudents.ReadOnly = true;
            dgvStudents.RowHeadersWidth = 62;
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.Size = new Size(1104, 466);
            dgvStudents.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1104, 635);
            Controls.Add(dgvStudents);
            Controls.Add(buttonHistogram);
            Controls.Add(buttonDelete);
            Controls.Add(buttonAdd);
            Name = "Form1";
            Text = "Деканат";
            ((System.ComponentModel.ISupportInitialize)dgvStudents).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button buttonAdd;
        private Button buttonDelete;
        private Button buttonHistogram;
        private DataGridView dgvStudents;
    }
}
