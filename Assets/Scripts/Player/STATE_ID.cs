using System;

namespace Player
{
	public enum STATE_ID
	{
		Edit = 1,
		Run,
		Jump,
		AirAttackAction,
		SpringJump,
		DashRing,
		AfterSpinAttack,
		Fall,
		Damage,
		FallingDead,
		Dead,
		RunLoop,
		ChangePhantom,
		ReturnFromPhantom,
		PhantomLaser,
		PhantomLaserBoss,
		PhantomDrill,
		PhantomDrillBoss,
		PhantomAsteroid,
		PhantomAsteroidBoss,
		ReachCannon,
		LaunchCannon,
		Hold,
		TrickJump,
		Stumble,
		DoubleJump,
		ThirdJump,
		LastChance,
		Non = -1
	}
}
