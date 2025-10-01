class Program
{
    static void Main(string[] args)
    {
        Random rnd = new Random();

        PrintSlow("Добро пожаловать, воин!\nНазови себя:", 50);
        string playerName = Console.ReadLine();

        string[] weaponNames = { "Фламберг", "Экскалибур", "Арболет", "Полуторный меч", "Магический посох" };
        string[] enemyNames = { "Варвар", "Орк", "Нежить", "Злой колдун", "Тролль" };
        string[] aidNames = { "Мини аптечка", "Средняя аптечка", "Большая аптечка" };
        int[] aidValues = { 5, 20, 45 };

        // Генерация оружия игрока
        Weapon playerWeapon = new Weapon(weaponNames[rnd.Next(weaponNames.Length)], rnd.Next(10, 50), rnd.Next(5, 15));
        int aidIndex = rnd.Next(aidNames.Length);
        Aid playerAid = new Aid(aidNames[aidIndex], aidValues[aidIndex]);

        Player player = new Player(playerName, 100, playerWeapon, playerAid);

        PrintSlow($"\nВаше имя {player.Name}!", 50);
        PrintSlow($"Вам был ниспослан {playerWeapon.Name} ({playerWeapon.Damage}), а также {playerAid.Name} ({playerAid.HealAmount}hp).", 50);
        PrintSlow($"У вас {player.Health}hp.\n", 50);

        while (player.IsAlive)
        {
            // Генерация врага
            Weapon enemyWeapon = new Weapon(weaponNames[rnd.Next(weaponNames.Length)], rnd.Next(10, 50), rnd.Next(5, 15));
            Enemy enemy = new Enemy(enemyNames[rnd.Next(enemyNames.Length)], rnd.Next(40, 80), enemyWeapon);

            PrintSlow($"{player.Name} встречает врага {enemy.Name} ({enemy.Health}hp), у врага оружие {enemyWeapon.Name} ({enemyWeapon.Damage})", 50);

            while (enemy.IsAlive && player.IsAlive)
            {
                PrintSlow("\nЧто вы будете делать?", 50);
                Console.WriteLine("1. Ударить");
                Console.WriteLine("2. Пропустить ход");
                Console.WriteLine("3. Использовать аптечку");
                Console.Write("> ");
                string wariant = Console.ReadLine();

                switch (wariant)
                {
                    case "1":
                        player.Attack(enemy);
                        if (enemy.IsAlive)
                            enemy.Attack(player);
                        break;
                    case "2":
                        PrintSlow($"{player.Name} пропустил ход.", 50);
                        if (enemy.IsAlive)
                            enemy.Attack(player);
                        break;
                    case "3":
                        player.UseAid();
                        if (enemy.IsAlive)
                            enemy.Attack(player);
                        break;
                    default:
                        PrintSlow("Неверный выбор!", 20);
                        break;
                }
            }

            if (!player.IsAlive)
            {
                PrintSlow("Игра окончена! Вы погибли...", 80);
                break;
            }
            else
            {
                player.Score += 10;
                PrintSlow($"\nВы победили врага {enemy.Name}! Ваши очки: {player.Score}\n", 60);
            }
        }
    }

    static void PrintSlow(string message, int delay)
    {
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(delay);
        }
        Console.WriteLine();
    }

    class Player
    {
        public string Name { get; }
        public int Health { get; private set; }
        public int MaxHealth { get; }
        public Aid Aid { get; }
        public Weapon Weapon { get; }
        public int Score { get; set; }

        public bool IsAlive => Health > 0;

        public Player(string name, int health, Weapon weapon, Aid aid)
        {
            Name = name;
            Health = health;
            MaxHealth = health;
            Weapon = weapon;
            Aid = aid;
            Score = 0;
        }

        public void Attack(Enemy enemy)
        {
            PrintSlow($"{Name} ударил противника {enemy.Name}", 40);
            enemy.TakeDamage(Weapon.Damage);
            PrintSlow($"У противника {enemy.Health}hp, у вас {Health}hp", 40);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }

        public void UseAid()
        {
            if (Health < MaxHealth)
            {
                Health += Aid.HealAmount;
                if (Health > MaxHealth) Health = MaxHealth;
                PrintSlow($"{Name} использовал аптечку. У вас {Health}hp", 40);
            }
            else
            {
                PrintSlow("У вас полное здоровье, аптечка не нужна!", 40);
            }
        }
    }

    class Enemy
    {
        public string Name { get; }
        public int Health { get; private set; }
        public int MaxHealth { get; }
        public Weapon Weapon { get; }

        public bool IsAlive => Health > 0;

        public Enemy(string name, int health, Weapon weapon)
        {
            Name = name;
            Health = health;
            MaxHealth = health;
            Weapon = weapon;
        }

        public void Attack(Player player)
        {
            PrintSlow($"Противник {Name} ударил вас!", 40);
            player.TakeDamage(Weapon.Damage);
            PrintSlow($"У противника {Health}hp, у вас {player.Health}hp", 40);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }
    }

    class Weapon
    {
        public string Name { get; }
        public int Damage { get; }
        public int Durability { get; private set; }

        public Weapon(string name, int damage, int durability)
        {
            Name = name;
            Damage = damage;
            Durability = durability;
        }
    }

    class Aid
    {
        public string Name { get; }
        public int HealAmount { get; }

        public Aid(string name, int healAmount)
        {
            Name = name;
            HealAmount = healAmount;
        }
    }
}