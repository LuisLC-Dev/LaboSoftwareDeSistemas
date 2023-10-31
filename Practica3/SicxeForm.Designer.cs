namespace Practica3
{
    partial class SicxeForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileButton = new System.Windows.Forms.Button();
            this.codeTB = new System.Windows.Forms.RichTextBox();
            this.analizeCodeButton = new System.Windows.Forms.Button();
            this.codeDetailsDGV = new System.Windows.Forms.DataGridView();
            this.Linea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Format = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProgramCounter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Instruction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Op = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.erroresP1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DirMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.objCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.erroresPaso2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabSimDGV = new System.Windows.Forms.DataGridView();
            this.Simbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dirección = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.errorsTB = new System.Windows.Forms.RichTextBox();
            this.programSizeLBL2 = new System.Windows.Forms.Label();
            this.programSizeLBL = new System.Windows.Forms.Label();
            this.registerDGV = new System.Windows.Forms.DataGridView();
            this.Tipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Content = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.codeDetailsDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabSimDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.registerDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(12, 12);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(75, 23);
            this.openFileButton.TabIndex = 0;
            this.openFileButton.Text = "Abrir Archivo";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // codeTB
            // 
            this.codeTB.Location = new System.Drawing.Point(12, 41);
            this.codeTB.Name = "codeTB";
            this.codeTB.Size = new System.Drawing.Size(224, 289);
            this.codeTB.TabIndex = 1;
            this.codeTB.Text = "";
            // 
            // analizeCodeButton
            // 
            this.analizeCodeButton.Location = new System.Drawing.Point(279, 12);
            this.analizeCodeButton.Name = "analizeCodeButton";
            this.analizeCodeButton.Size = new System.Drawing.Size(75, 23);
            this.analizeCodeButton.TabIndex = 2;
            this.analizeCodeButton.Text = "Analizar";
            this.analizeCodeButton.UseVisualStyleBackColor = true;
            this.analizeCodeButton.Click += new System.EventHandler(this.analizeCodeButton_Click);
            // 
            // codeDetailsDGV
            // 
            this.codeDetailsDGV.AllowUserToAddRows = false;
            this.codeDetailsDGV.AllowUserToDeleteRows = false;
            this.codeDetailsDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Linea,
            this.Format,
            this.ProgramCounter,
            this.ID,
            this.Instruction,
            this.Op,
            this.erroresP1,
            this.DirMode,
            this.objCode,
            this.erroresPaso2});
            this.codeDetailsDGV.Location = new System.Drawing.Point(242, 41);
            this.codeDetailsDGV.Name = "codeDetailsDGV";
            this.codeDetailsDGV.ReadOnly = true;
            this.codeDetailsDGV.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.codeDetailsDGV.Size = new System.Drawing.Size(735, 592);
            this.codeDetailsDGV.TabIndex = 3;
            this.codeDetailsDGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.codeDetailsDGV_CellContentClick);
            // 
            // Linea
            // 
            this.Linea.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Linea.HeaderText = "Línea";
            this.Linea.Name = "Linea";
            this.Linea.ReadOnly = true;
            this.Linea.Width = 60;
            // 
            // Format
            // 
            this.Format.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Format.HeaderText = "Formato";
            this.Format.Name = "Format";
            this.Format.ReadOnly = true;
            this.Format.Width = 70;
            // 
            // ProgramCounter
            // 
            this.ProgramCounter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ProgramCounter.HeaderText = "CP";
            this.ProgramCounter.Name = "ProgramCounter";
            this.ProgramCounter.ReadOnly = true;
            this.ProgramCounter.Width = 46;
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ID.HeaderText = "Etiqueta";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 71;
            // 
            // Instruction
            // 
            this.Instruction.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Instruction.HeaderText = "Inst/Dir";
            this.Instruction.Name = "Instruction";
            this.Instruction.ReadOnly = true;
            this.Instruction.Width = 67;
            // 
            // Op
            // 
            this.Op.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Op.HeaderText = "Operando";
            this.Op.Name = "Op";
            this.Op.ReadOnly = true;
            this.Op.Width = 79;
            // 
            // erroresP1
            // 
            this.erroresP1.HeaderText = "Errores Paso1";
            this.erroresP1.Name = "erroresP1";
            this.erroresP1.ReadOnly = true;
            // 
            // DirMode
            // 
            this.DirMode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.DirMode.HeaderText = "ModoDir";
            this.DirMode.Name = "DirMode";
            this.DirMode.ReadOnly = true;
            this.DirMode.Width = 72;
            // 
            // objCode
            // 
            this.objCode.HeaderText = "CódigoObjeto";
            this.objCode.Name = "objCode";
            this.objCode.ReadOnly = true;
            // 
            // erroresPaso2
            // 
            this.erroresPaso2.HeaderText = "ErroresPaso2";
            this.erroresPaso2.Name = "erroresPaso2";
            this.erroresPaso2.ReadOnly = true;
            // 
            // tabSimDGV
            // 
            this.tabSimDGV.AllowUserToAddRows = false;
            this.tabSimDGV.AllowUserToDeleteRows = false;
            this.tabSimDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tabSimDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Simbol,
            this.Dirección});
            this.tabSimDGV.Location = new System.Drawing.Point(983, 41);
            this.tabSimDGV.Name = "tabSimDGV";
            this.tabSimDGV.ReadOnly = true;
            this.tabSimDGV.Size = new System.Drawing.Size(304, 289);
            this.tabSimDGV.TabIndex = 4;
            // 
            // Simbol
            // 
            this.Simbol.HeaderText = "Símbolo";
            this.Simbol.Name = "Simbol";
            this.Simbol.ReadOnly = true;
            // 
            // Dirección
            // 
            this.Dirección.HeaderText = "Direction";
            this.Dirección.Name = "Dirección";
            this.Dirección.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1107, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "TABSIM";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 355);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "ERRORES";
            // 
            // errorsTB
            // 
            this.errorsTB.Location = new System.Drawing.Point(12, 371);
            this.errorsTB.Name = "errorsTB";
            this.errorsTB.Size = new System.Drawing.Size(224, 262);
            this.errorsTB.TabIndex = 8;
            this.errorsTB.Text = "";
            // 
            // programSizeLBL2
            // 
            this.programSizeLBL2.AutoSize = true;
            this.programSizeLBL2.Location = new System.Drawing.Point(942, 9);
            this.programSizeLBL2.Name = "programSizeLBL2";
            this.programSizeLBL2.Size = new System.Drawing.Size(35, 13);
            this.programSizeLBL2.TabIndex = 10;
            this.programSizeLBL2.Text = "label3";
            // 
            // programSizeLBL
            // 
            this.programSizeLBL.AutoSize = true;
            this.programSizeLBL.Location = new System.Drawing.Point(823, 9);
            this.programSizeLBL.Name = "programSizeLBL";
            this.programSizeLBL.Size = new System.Drawing.Size(113, 13);
            this.programSizeLBL.TabIndex = 9;
            this.programSizeLBL.Text = "Tamaño del programa:";
            // 
            // registerDGV
            // 
            this.registerDGV.AllowUserToAddRows = false;
            this.registerDGV.AllowUserToDeleteRows = false;
            this.registerDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.registerDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Tipo,
            this.Content});
            this.registerDGV.Location = new System.Drawing.Point(983, 371);
            this.registerDGV.Name = "registerDGV";
            this.registerDGV.ReadOnly = true;
            this.registerDGV.Size = new System.Drawing.Size(304, 262);
            this.registerDGV.TabIndex = 11;
            // 
            // Tipo
            // 
            this.Tipo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Tipo.HeaderText = "Tipo";
            this.Tipo.Name = "Tipo";
            this.Tipo.ReadOnly = true;
            this.Tipo.Width = 53;
            // 
            // Content
            // 
            this.Content.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Content.HeaderText = "Contenido";
            this.Content.Name = "Content";
            this.Content.ReadOnly = true;
            this.Content.Width = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1107, 355);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Registros:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(93, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Guardar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(171, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Nuevo";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // SicxeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1299, 645);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.registerDGV);
            this.Controls.Add(this.programSizeLBL2);
            this.Controls.Add(this.programSizeLBL);
            this.Controls.Add(this.errorsTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabSimDGV);
            this.Controls.Add(this.codeDetailsDGV);
            this.Controls.Add(this.analizeCodeButton);
            this.Controls.Add(this.codeTB);
            this.Controls.Add(this.openFileButton);
            this.Name = "SicxeForm";
            this.Text = "AnalizadorSICXE";
            ((System.ComponentModel.ISupportInitialize)(this.codeDetailsDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabSimDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.registerDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.RichTextBox codeTB;
        private System.Windows.Forms.Button analizeCodeButton;
        private System.Windows.Forms.DataGridView codeDetailsDGV;
        private System.Windows.Forms.DataGridView tabSimDGV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox errorsTB;
        private System.Windows.Forms.Label programSizeLBL2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Simbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dirección;
        private System.Windows.Forms.Label programSizeLBL;
        private System.Windows.Forms.DataGridViewTextBoxColumn Linea;
        private System.Windows.Forms.DataGridViewTextBoxColumn Format;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProgramCounter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Instruction;
        private System.Windows.Forms.DataGridViewTextBoxColumn Op;
        private System.Windows.Forms.DataGridViewTextBoxColumn erroresP1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DirMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn objCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn erroresPaso2;
        private System.Windows.Forms.DataGridView registerDGV;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Content;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

