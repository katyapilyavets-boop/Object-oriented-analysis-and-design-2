using System;
using System.Drawing;      
using System.Windows.Forms;

namespace GameWithoutPattern
{
    public class Enemy
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; } 
    }

    public class Weapon
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; } 
    }

    public class Potion
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; }  
    }

    public partial class Form1 : Form
    {
        private string currentWorld;

        public Form1()
        {
            InitializeComponent();
            worldSelector.SelectedIndex = 0;
            currentWorld = "Фэнтези";
            UpdateWorldUI();
            Log("🎮 Добро пожаловать!\nВыберите мир и нажмите «Генерировать».\n");
        }

        private void WorldSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (worldSelector.SelectedItem == null) return;
            currentWorld = worldSelector.SelectedItem.ToString();
            UpdateWorldUI();
            outputLog.Clear();

           
            ClearImages();
        }

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            Log($"\n{'═',50}");
            Log($"МИР: {currentWorld}");
            Log(new string('─', 50));

            Enemy enemy;
            Weapon weapon;
            Potion potion;

            if (currentWorld == "Фэнтези")
            {
                enemy = new Enemy
                {
                    Name = "Древний дракон",
                    Description = "Огромный ящер с чешуёй цвета заката. Дышит огнём.",
                    Image = lab1wp.Properties.Resources.fantasy_enemy  
                };

                weapon = new Weapon
                {
                    Name = "Стальной меч",
                    Description = "Кованый клинок с рунами силы.",
                    Image = lab1wp.Properties.Resources.fantasy_weapon  
                };

                potion = new Potion
                {
                    Name = "Зелье маны",
                    Description = "Синяя жидкость с магическим свечением.",
                    Image = lab1wp.Properties.Resources.fantasy_potion  
                };
            }
            else if (currentWorld == "Киберпанк")
            {
                enemy = new Enemy
                {
                    Name = "Кибер-солдат",
                    Description = "Человек с боевыми имплантами.",
                    Image = lab1wp.Properties.Resources.cyber_enemy  
                };

                weapon = new Weapon
                {
                    Name = "Плазменная винтовка",
                    Description = "Стреляет сгустками плазмы.",
                    Image = lab1wp.Properties.Resources.cyber_weapon  
                };

                potion = new Potion
                {
                    Name = "Энергетик Вольт",
                    Description = "Радиоактивный напиток для бодрости.",
                    Image = lab1wp.Properties.Resources.cyber_potion 
                };
            }
            else
            {
                Log("❌ Неизвестный мир!");
                return;
            }

            // 🔥 Отображаем картинки в PictureBox
            if (picEnemy != null && enemy.Image != null)
                picEnemy.Image = new Bitmap(enemy.Image);
            if (picWeapon != null && weapon.Image != null)
                picWeapon.Image = new Bitmap(weapon.Image);
            if (picPotion != null && potion.Image != null)
                picPotion.Image = new Bitmap(potion.Image);

            Log($"\n ВРАГ");
            Log($"  {enemy.Name}");
            Log($"  {enemy.Description}");

            Log($"\nОРУЖИЕ");
            Log($"  {weapon.Name}");
            Log($"  {weapon.Description}");

            Log($"\nЗЕЛЬЕ");
            Log($"  {potion.Name}");
            Log($"  {potion.Description}");

            Log($"\n{'═',50}");
            Log("Готово!\n");
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            outputLog.Clear();
        }

        private void UpdateWorldUI()
        {
            string description;
            if (currentWorld == "Фэнтези")
                description = "Мир магии и драконов";
            else if (currentWorld == "Киберпанк")
                description = "Мир высоких технологий";
            else
                description = "Неизвестный мир";

            statusLabel.Text = "Мир: " + currentWorld;
            worldDescLabel.Text = description;
        }

        private void Log(string message)
        {
            if (outputLog.InvokeRequired)
                outputLog.Invoke(new Action(() => outputLog.AppendText(message + Environment.NewLine)));
            else
                outputLog.AppendText(message + Environment.NewLine);
        }
        private void ClearImages()
        {
            if (picEnemy?.Image != null) { picEnemy.Image.Dispose(); picEnemy.Image = null; }
            if (picWeapon?.Image != null) { picWeapon.Image.Dispose(); picWeapon.Image = null; }
            if (picPotion?.Image != null) { picPotion.Image.Dispose(); picPotion.Image = null; }
        }

        
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ClearImages();
            base.OnFormClosed(e);
        }
    }
}