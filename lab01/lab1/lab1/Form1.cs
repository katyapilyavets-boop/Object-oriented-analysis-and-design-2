using System;
using System.Windows.Forms;

namespace GameFactoryDemo
{

    public interface Enemy
    {
        string GetName();
        string GetDescription();
    }

    public interface Weapon
    {
        string GetName();
        string GetDescription();
    }

    public interface Potion
    {
        string GetName();
        string GetDescription();
    }


    public class FantasyEnemy : Enemy
    {
        public string GetName() => "Древний дракон";
        public string GetDescription() => "Огромный ящер с чешуёй цвета заката. Дышит огнём.";
    }

    public class FantasyWeapon : Weapon
    {
        public string GetName() => "Стальной меч";
        public string GetDescription() => "Кованый клинок с рунами силы.";
    }

    public class FantasyPotion : Potion
    {
        public string GetName() => "Зелье маны";
        public string GetDescription() => "Синяя жидкость с магическим свечением.";
    }


    public class CyberPankEnemy : Enemy
    {
        public string GetName() => "Кибер-солдат";
        public string GetDescription() => "Человек с боевыми имплантами.";
    }

    public class CyberPankWeapon : Weapon
    {
        public string GetName() => "Плазменная винтовка";
        public string GetDescription() => "Стреляет сгустками плазмы.";
    }

    public class CyberPankPotion : Potion
    {
        public string GetName() => "Энергетик Вольт";
        public string GetDescription() => "Радиоактивный напиток для бодрости.";
    }


    public interface GameFactory
    {
        Enemy createEnemy();
        Weapon createWeapon();
        Potion createPotion();
        string GetWorldName();
        string GetWorldDescription();
    }

    public class FantasyGameFactory : GameFactory
    {
        public Enemy createEnemy() => new FantasyEnemy();
        public Weapon createWeapon() => new FantasyWeapon();
        public Potion createPotion() => new FantasyPotion();
        public string GetWorldName() => "Фэнтези";
        public string GetWorldDescription() => "Мир магии и драконов";
    }

    public class CyberPankGameFactory : GameFactory
    {
        public Enemy createEnemy() => new CyberPankEnemy();
        public Weapon createWeapon() => new CyberPankWeapon();
        public Potion createPotion() => new CyberPankPotion();
        public string GetWorldName() => "Киберпанк";
        public string GetWorldDescription() => "Мир высоких технологий";
    }


    public partial class Form1 : Form
    {
        private GameFactory currentFactory;

        public Form1()
        {
            InitializeComponent();
            worldSelector.SelectedIndex = 0;
            currentFactory = new FantasyGameFactory();
            UpdateWorldUI();
            Log("🎮 Добро пожаловать!\nВыберите мир и нажмите «Генерировать».\n");
        }

        private void WorldSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (worldSelector.SelectedItem == null) return;
            string selected = worldSelector.SelectedItem.ToString();

            if (selected == "Фэнтези")
                currentFactory = new FantasyGameFactory();
            else if (selected == "Киберпанк")
                currentFactory = new CyberPankGameFactory();

            UpdateWorldUI();
            outputLog.Clear();
        }

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            if (currentFactory == null) return;

            Log($"\n{'═',50}");
            Log($"МИР: {currentFactory.GetWorldName()}");
            Log(new string('─', 50));

            Enemy enemy = currentFactory.createEnemy();
            Weapon weapon = currentFactory.createWeapon();
            Potion potion = currentFactory.createPotion();

            Log($"\n ВРАГ");
            Log($"  {enemy.GetName()}");
            Log($"  {enemy.GetDescription()}");

            Log($"\nОРУЖИЕ");
            Log($"  {weapon.GetName()}");
            Log($"  {weapon.GetDescription()}");

            Log($"\nЗЕЛЬЕ");
            Log($"  {potion.GetName()}");
            Log($"  {potion.GetDescription()}");

            Log($"\n{'═',50}");
            Log("Готово!\n");
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            outputLog.Clear();
        }

        private void UpdateWorldUI()
        {
            statusLabel.Text = "Мир: " + currentFactory.GetWorldName();
            worldDescLabel.Text = currentFactory.GetWorldDescription();
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