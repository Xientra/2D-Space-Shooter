# 2D-Space-Shooter

# SoundList:

========Player========
-> Ship / General
	OnHit (some clang against the hull)
	Moving?
	OnDeathExplosion
	OnCoolDownReset?

-> Weapons
	ChainGun
	GrenadeLauncher & ShrapnellLauncher (x)
	Helix [1 (x), 2 (x), 3 (x)]
	Homing (x?)
	LaserSword?
	MissileLauncher (x)
	ShotGun (x)
	Standart (x)
	(Sniper)

-> Bullets & Explosionsn (Question: Should there be a audio feedback on hit? i think yes)
	GrenadeExplosion
	Q? HelixBullet (Child and Main?)
	Q? Homingbullet
	Q? LaserSword
	Missile Moving?
	MissileExplosion
	Q? ShotgunBullet
	ShrapnellExplosion
	Q? (No?) ShrapnellBullet
	(Q? SniperBullet)
	Q? Standart

	{
	Explosion_Big
	Explosion_Small
	Explosion_Impact
	}

========PickUps========
-> PowerUps (Both collecting and active (and on existing?))
	DamageUp
	FireRate
	HealthUp
	Invincibility
	Regeneration
	SloMo

-> Credits
	PickUpSound (different pitch for different values?)


========Enemies========
-> OnShoot (Should they make a sound on firing? I think not acually (but i'll see)) 
	   (Should they make a sound when hitting the player? (yes?))

	AlienLaserBullet
	AilenBulletBig
	AlienBulletSimple
	AilenBulletSlow

	ExpBullet
	ExpBulletSimple & Split
	ExpBulletSmall
	ExpExplosiveContainer
	ExpExplosiveContainerExplosion
	
	WireArtilleryBullet
	WireBulletlightning
		

========UI========
-> Buttons



