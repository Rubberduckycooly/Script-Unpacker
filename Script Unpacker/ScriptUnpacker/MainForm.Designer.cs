namespace ScriptUnpacker
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SelectDataFolderButton = new System.Windows.Forms.Button();
            this.RSDKverBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.UnpackButton = new System.Windows.Forms.Button();
            this.BytecodeButton = new System.Windows.Forms.Button();
            this.UnpackBCFileButton = new System.Windows.Forms.Button();
            this.UseHexCB = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // SelectDataFolderButton
            // 
            this.SelectDataFolderButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SelectDataFolderButton.Location = new System.Drawing.Point(20, 93);
            this.SelectDataFolderButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SelectDataFolderButton.Name = "SelectDataFolderButton";
            this.SelectDataFolderButton.Size = new System.Drawing.Size(145, 44);
            this.SelectDataFolderButton.TabIndex = 0;
            this.SelectDataFolderButton.Text = "Select Data Folder";
            this.SelectDataFolderButton.UseVisualStyleBackColor = true;
            this.SelectDataFolderButton.Click += new System.EventHandler(this.SelectDataFolderButton_Click);
            // 
            // RSDKverBox
            // 
            this.RSDKverBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RSDKverBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RSDKverBox.FormattingEnabled = true;
            this.RSDKverBox.Items.AddRange(new object[] {
            "RSDKv2 (Sonic CD)",
            "RSDKvB (Sonic 1/2)",
            "RSDKvRS (Retro-Sonic (2007))"});
            this.RSDKverBox.Location = new System.Drawing.Point(19, 38);
            this.RSDKverBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RSDKverBox.Name = "RSDKverBox";
            this.RSDKverBox.Size = new System.Drawing.Size(146, 21);
            this.RSDKverBox.TabIndex = 1;
            this.RSDKverBox.SelectedIndexChanged += new System.EventHandler(this.RSDKverBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "RSDK Version";
            // 
            // UnpackButton
            // 
            this.UnpackButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.UnpackButton.Location = new System.Drawing.Point(22, 193);
            this.UnpackButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.UnpackButton.Name = "UnpackButton";
            this.UnpackButton.Size = new System.Drawing.Size(145, 44);
            this.UnpackButton.TabIndex = 3;
            this.UnpackButton.Text = "Unpack!";
            this.UnpackButton.UseVisualStyleBackColor = true;
            this.UnpackButton.Click += new System.EventHandler(this.UnpackButton_Click);
            // 
            // BytecodeButton
            // 
            this.BytecodeButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BytecodeButton.Location = new System.Drawing.Point(224, 93);
            this.BytecodeButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BytecodeButton.Name = "BytecodeButton";
            this.BytecodeButton.Size = new System.Drawing.Size(145, 44);
            this.BytecodeButton.TabIndex = 4;
            this.BytecodeButton.Text = "Select Bytecode File";
            this.BytecodeButton.UseVisualStyleBackColor = true;
            this.BytecodeButton.Click += new System.EventHandler(this.BytecodeButton_Click);
            // 
            // UnpackBCFileButton
            // 
            this.UnpackBCFileButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.UnpackBCFileButton.Location = new System.Drawing.Point(224, 193);
            this.UnpackBCFileButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.UnpackBCFileButton.Name = "UnpackBCFileButton";
            this.UnpackBCFileButton.Size = new System.Drawing.Size(145, 44);
            this.UnpackBCFileButton.TabIndex = 5;
            this.UnpackBCFileButton.Text = "Unpack File!";
            this.UnpackBCFileButton.UseVisualStyleBackColor = true;
            this.UnpackBCFileButton.Click += new System.EventHandler(this.UnpackBCFileButton_Click);
            // 
            // UseHexCB
            // 
            this.UseHexCB.AutoSize = true;
            this.UseHexCB.Location = new System.Drawing.Point(224, 38);
            this.UseHexCB.Name = "UseHexCB";
            this.UseHexCB.Size = new System.Drawing.Size(102, 17);
            this.UseHexCB.TabIndex = 6;
            this.UseHexCB.Text = "Use Hex Values";
            this.UseHexCB.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(398, 266);
            this.Controls.Add(this.UseHexCB);
            this.Controls.Add(this.UnpackBCFileButton);
            this.Controls.Add(this.BytecodeButton);
            this.Controls.Add(this.UnpackButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RSDKverBox);
            this.Controls.Add(this.SelectDataFolderButton);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainForm";
            this.Text = "Script Unpacker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SelectDataFolderButton;
        private System.Windows.Forms.ComboBox RSDKverBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button UnpackButton;
        private System.Windows.Forms.Button BytecodeButton;
        private System.Windows.Forms.Button UnpackBCFileButton;
        private System.Windows.Forms.CheckBox UseHexCB;
    }
}

