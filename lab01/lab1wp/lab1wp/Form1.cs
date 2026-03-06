using System;
using System.Windows.Forms;

namespace GameWithoutPattern
{

    public class Enemy
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Weapon
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Potion
    {
        public string Name { get; set; }
        public string Description { get; set; }
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
                    Description = "Огромный ящер с чешуёй цвета заката. Дышит огнём."
                };

                weapon = new Weapon
                {
                    Name = "Стальной меч",
                    Description = "Кованый клинок с рунами силы."
                };

                potion = new Potion
                {
                    Name = "Зелье маны",
                    Description = "Синяя жидкость с магическим свечением."
                };
            }
            else if (currentWorld == "Киберпанк")
            {
                enemy = new Enemy
                {
                    Name = "Кибер-солдат",
                    Description = "Человек с боевыми имплантами."
                };

                weapon = new Weapon
                {
                    Name = "Плазменная винтовка",
                    Description = "Стреляет сгустками плазмы."
                };

                potion = new Potion
                {
                    Name = "Энергетик Вольт",
                    Description = "Радиоактивный напиток для бодрости."
                };
            }
            else
            {
                Log("❌ Неизвестный мир!");
                return;
            }


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
    }
}