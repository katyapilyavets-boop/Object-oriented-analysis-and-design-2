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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.worldSelector = new System.Windows.Forms.ComboBox();
            this.worldLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.worldDescLabel = new System.Windows.Forms.Label();
            this.generateBtn = new System.Windows.Forms.Button();
            this.clearBtn = new System.Windows.Forms.Button();
            this.outputLog = new System.Windows.Forms.RichTextBox();
            this.picEnemy = new System.Windows.Forms.PictureBox();
            this.picWeapon = new System.Windows.Forms.PictureBox();
            this.picPotion = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picEnemy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWeapon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPotion)).BeginInit();
            this.SuspendLayout();

            // worldSelector
            this.worldSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.worldSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.worldSelector.FormattingEnabled = true;
            this.worldSelector.Items.AddRange(new object[] { "Фэнтези", "Киберпанк" });
            this.worldSelector.Location = new System.Drawing.Point(124, 54);
            this.worldSelector.Name = "worldSelector";
            this.worldSelector.Size = new System.Drawing.Size(265, 28);
            this.worldSelector.TabIndex = 0;
            this.worldSelector.SelectedIndexChanged += new System.EventHandler(this.WorldSelector_SelectedIndexChanged);

            // worldLabel
            this.worldLabel.AutoSize = true;
            this.worldLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.worldLabel.Location = new System.Drawing.Point(185, 20);
            this.worldLabel.Name = "worldLabel";
            this.worldLabel.Size = new System.Drawing.Size(148, 20);
            this.worldLabel.TabIndex = 1;
            this.worldLabel.Text = "Выберите мир:";

            // statusLabel
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.statusLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.statusLabel.Location = new System.Drawing.Point(16, 86);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(115, 18);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "Текущий мир: —";

            // worldDescLabel
            this.worldDescLabel.AutoSize = true;
            this.worldDescLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.worldDescLabel.ForeColor = System.Drawing.Color.DimGray;
            this.worldDescLabel.Location = new System.Drawing.Point(16, 111);
            this.worldDescLabel.MaximumSize = new System.Drawing.Size(533, 0);
            this.worldDescLabel.Name = "worldDescLabel";
            this.worldDescLabel.Size = new System.Drawing.Size(93, 18);
            this.worldDescLabel.TabIndex = 3;
            this.worldDescLabel.Text = "Описание: —";

            // generateBtn
            this.generateBtn.BackColor = System.Drawing.Color.SteelBlue;
            this.generateBtn.FlatAppearance.BorderSize = 0;
            this.generateBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.generateBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.generateBtn.ForeColor = System.Drawing.Color.White;
            this.generateBtn.Location = new System.Drawing.Point(19, 175);
            this.generateBtn.Name = "generateBtn";
            this.generateBtn.Size = new System.Drawing.Size(200, 43);
            this.generateBtn.TabIndex = 10;
            this.generateBtn.Text = "Генерировать";
            this.generateBtn.UseVisualStyleBackColor = false;
            this.generateBtn.Click += new System.EventHandler(this.GenerateBtn_Click);

            // clearBtn
            this.clearBtn.BackColor = System.Drawing.Color.SlateGray;
            this.clearBtn.FlatAppearance.BorderSize = 0;
            this.clearBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.clearBtn.ForeColor = System.Drawing.Color.White;
            this.clearBtn.Location = new System.Drawing.Point(333, 175);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(133, 43);
            this.clearBtn.TabIndex = 11;
            this.clearBtn.Text = "Очистить";
            this.clearBtn.UseVisualStyleBackColor = false;
            this.clearBtn.Click += new System.EventHandler(this.ClearBtn_Click);

            // 🔥 PictureBox для картинок
            // picEnemy
            this.picEnemy.Location = new System.Drawing.Point(19, 240);
            this.picEnemy.Name = "picEnemy";
            this.picEnemy.Size = new System.Drawing.Size(120, 120);
            this.picEnemy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEnemy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picEnemy.TabStop = false;
            // picWeapon
            this.picWeapon.Location = new System.Drawing.Point(173, 240);
            this.picWeapon.Name = "picWeapon";
            this.picWeapon.Size = new System.Drawing.Size(120, 120);
            this.picWeapon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picWeapon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picWeapon.TabStop = false;
            // picPotion
            this.picPotion.Location = new System.Drawing.Point(327, 240);
            this.picPotion.Name = "picPotion";
            this.picPotion.Size = new System.Drawing.Size(120, 120);
            this.picPotion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPotion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPotion.TabStop = false;

            // outputLog (сдвинут вниз под картинки)
            this.outputLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.outputLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.outputLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.outputLog.ForeColor = System.Drawing.Color.LightGreen;
            this.outputLog.Location = new System.Drawing.Point(19, 370);
            this.outputLog.Name = "outputLog";
            this.outputLog.ReadOnly = true;
            this.outputLog.Size = new System.Drawing.Size(447, 130);
            this.outputLog.TabIndex = 12;
            this.outputLog.Text = "";

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(487, 522);
            this.Controls.Add(this.outputLog);
            this.Controls.Add(this.picPotion);
            this.Controls.Add(this.picWeapon);
            this.Controls.Add(this.picEnemy);
            this.Controls.Add(this.clearBtn);
            this.Controls.Add(this.generateBtn);
            this.Controls.Add(this.worldDescLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.worldLabel);
            this.Controls.Add(this.worldSelector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🎮 Game Factory";

            ((System.ComponentModel.ISupportInitialize)(this.picEnemy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWeapon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPotion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox worldSelector;
        private System.Windows.Forms.Label worldLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label worldDescLabel;
        private System.Windows.Forms.Button generateBtn;
        private System.Windows.Forms.Button clearBtn;
        private System.Windows.Forms.RichTextBox outputLog;
        private System.Windows.Forms.PictureBox picEnemy;
        private System.Windows.Forms.PictureBox picWeapon;
        private System.Windows.Forms.PictureBox picPotion;
    }
}