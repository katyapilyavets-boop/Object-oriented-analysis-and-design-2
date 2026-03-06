namespace GameWithoutPattern
{
    partial class Form1
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
            this.worldSelector = new System.Windows.Forms.ComboBox();
            this.generateBtn = new System.Windows.Forms.Button();
            this.clearBtn = new System.Windows.Forms.Button();
            this.outputLog = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.worldDescLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // worldSelector
            // 
            this.worldSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.worldSelector.Items.AddRange(new object[] {
            "Фэнтези",
            "Киберпанк"});
            this.worldSelector.Location = new System.Drawing.Point(12, 38);
            this.worldSelector.Name = "worldSelector";
            this.worldSelector.Size = new System.Drawing.Size(200, 21);
            this.worldSelector.TabIndex = 0;
            this.worldSelector.SelectedIndexChanged += new System.EventHandler(this.WorldSelector_SelectedIndexChanged);
            // 
            // generateBtn
            // 
            this.generateBtn.Location = new System.Drawing.Point(12, 78);
            this.generateBtn.Name = "generateBtn";
            this.generateBtn.Size = new System.Drawing.Size(200, 35);
            this.generateBtn.TabIndex = 1;
            this.generateBtn.Text = "🎲 Генерировать мир";
            this.generateBtn.UseVisualStyleBackColor = true;
            this.generateBtn.Click += new System.EventHandler(this.GenerateBtn_Click);
            // 
            // clearBtn
            // 
            this.clearBtn.Location = new System.Drawing.Point(12, 119);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(200, 25);
            this.clearBtn.TabIndex = 2;
            this.clearBtn.Text = "🗑️ Очистить лог";
            this.clearBtn.UseVisualStyleBackColor = true;
            this.clearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // outputLog
            // 
            this.outputLog.Location = new System.Drawing.Point(12, 178);
            this.outputLog.Multiline = true;
            this.outputLog.Name = "outputLog";
            this.outputLog.ReadOnly = true;
            this.outputLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputLog.Size = new System.Drawing.Size(560, 300);
            this.outputLog.TabIndex = 3;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusLabel.Location = new System.Drawing.Point(230, 15);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(42, 16);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "Мир: ";
            // 
            // worldDescLabel
            // 
            this.worldDescLabel.AutoSize = true;
            this.worldDescLabel.ForeColor = System.Drawing.Color.Gray;
            this.worldDescLabel.Location = new System.Drawing.Point(230, 41);
            this.worldDescLabel.Name = "worldDescLabel";
            this.worldDescLabel.Size = new System.Drawing.Size(107, 13);
            this.worldDescLabel.TabIndex = 5;
            this.worldDescLabel.Text = "Описание мира...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "🎮 Выбор мира игры:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 491);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.worldDescLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.outputLog);
            this.Controls.Add(this.clearBtn);
            this.Controls.Add(this.generateBtn);
            this.Controls.Add(this.worldSelector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🎮 Генератор игровых миров (без паттерна)";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ComboBox worldSelector;
        private System.Windows.Forms.Button generateBtn;
        private System.Windows.Forms.Button clearBtn;
        private System.Windows.Forms.TextBox outputLog;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label worldDescLabel;
        private System.Windows.Forms.Label label1;
    }
}