using Engine.Core;

Scene firstLevel = new Scene("Level 1");

GameObject player = new GameObject("Player");
player.Transform.X = 100f;
player.Transform.Y = 50f;

GameObject enemy = new GameObject("Enemy");
enemy.Transform.X = 300f;
enemy.Transform.Y = 50f;

firstLevel.Add(player);
firstLevel.Add(enemy);

EngineRuntime engineRuntime = new EngineRuntime(firstLevel);

player.AddComponent(x => new PlayerMovement(x));
Console.WriteLine(player.Transform.X);
engineRuntime.Run(60);
Console.WriteLine(player.Transform.X);