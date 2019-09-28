using UnityEngine;
using System.Collections;

public class SkillsInfo {

    public const float Slime_HandDamage = 15f;

    public const float Slime_JumpAttackDamage = 50f;

    public const float Slime_PulseDamage = 50f;

    public const float Slime_ForceOrbDamage = 100f;
    public const float Slime_ForceOrbSpeed = 5f;

    public const float Slime_FireTurret_ParticleDamage = 20f;
    public const float Slime_FireTurret_ThrowSpeed = 10f;
    public const float Slime_FireTurret_LifeTime = 15f;

    public const float Slime_FireBolt_Damage = 20f;
    public const float Slime_FireBolt_LifeTime = 5f;
    public const float Slime_FireBolt_Speed = 20f;
    public const int Slime_FireBolt_Loops = 10;
    public const float Slime_FireBolt_BurnLength = 5f;

    public const float Slime_Meteor_Damage = 40f;
    public const float Slime_MeteorShower_Diameter = 15f;
    public const float Slime_MeteorShower_Lifetime = 10f;
    public const float Slime_MeteorShower_Interval = 0.03f;

    public const float Slime_FireCircle_BurnDuration = 10f;
    public const float Slime_FireCircle_LifeTime = 5f;

    public const float Slime_Steam_Damage = 1f;

    public const float Slime_PortalProjectile_Damage = 200f;
    public const float Slime_PortalProjectile_Speed = 10f;
    public const float Slime_PortalProjectile_Interval = 0.5f;
    public const float Slime_PortalProjectile_LifeTime = 5f;
    public const float Slime_PortalBeam_Damage = 50f;

    public const float Slime_Mine_AliveDuration = 30f;
    public const float Slime_RedMine_Damage = 69f;
    public const float Slime_BlueMine_SlowDuration = 5f;

    public const float Slime_Spikes_Damage = 50f;

    // PLAYER SKILLS //
    public const float Player_Orb_Damage = 1f;
    public const float Player_DamageOrb_ModifierAdditive = 0.02f;
    public const float Player_DefenceOrb_ModifierAdditive = 0.02f;
    public const float Player_SpeedOrb_ModifierAdditive = 0.02f;

    public const float Player_ChannelHeat_Cooldown = 1f;
    public const float Player_ChannelHeat_Damage = 30f;
    public const float Player_ChannelHeat_CollapseSpeed = 12f;
    public const float Player_ChannelHeat_TimeTillFullChannel = 4f;

    public const float Player_Compress_Speed = 10f;
    public const float Player_Compress_Cooldown = 3f;

    public const float Player_Manipulate_Cooldown = 1f;
    public const float Player_Manipulate_Interval = 4f;

    public const float Player_Rift_Cooldown = 10f;
    public const float Player_Rift_RadiusScale = 1f;
    public const float Player_Rift_Damage = 10f;
    public const float Player_Rift_DamageInterval = 1.5f;
    public const float Player_Rift_LifeTime = 4f;
    public const float Player_Rift_SlowDuration = 5f;
    public const float Player_Rift_DoubleOrbDuration = 4f;

    public const float Player_Collapse_Cooldown = 10f;

    public const float Player_Stun_Cooldown = 20f;
    public const float Player_Stun_Duration = 4f;
    public const float Player_Stun_RadiusScale = 1f;

    public const float Player_Masochism_Cooldown = 20f;
    public const float Player_Masochism_Duration = 4f;

    public const float Player_Teleport_Cooldown = 0f;

    public const float Player_StellarBolt_Damage = 30f;
    public const float Player_StellarBolt_Cooldown = 3f;
    public const float Player_StellarBolt_Speed = 20f;
    public const float Player_StellarBolt_LifeTime = 10f;

    public const float Player_Transcendence_Cooldown = 0f;
    public const float Player_Transcendence_BuffDuration = 20f;

    public const float Player_Heal_Cooldown = 0f;
    public const float Player_Heal_HealAmount = 25f;

    public const float Player_Transference_Speed = 15f;
    public const float Player_Transference_CooldownAfterShoot = 3f;
    public const float Player_Transference_CooldownAfterHit = 0f;
    public const float Player_Transference_CooldownAfterCancel = 0f;

    public const float Player_Barrage_Speed = 20f;
    public const float Player_Barrage_Cooldown = 1f;
    public const float Player_Barrage_Damage = 10f;
    public const float Player_Barrage_ProjectileLife = 10f;


    //Debuffs

    public const float Debuff_TranscendenceDamage_DamageModifierAdditive = 0.5f;
    public const float Debuff_TranscendenceDefense_DefenceModifierAdditive = 0.5f;
    public const float Debuff_TranscendenceControl_SpeedModifierAdditive = 0.5f;

    public const float Debuff_Burn_Damage = 10f;
    public const float Debuff_Burn_Interval = 1f;



    // uhhm something else?

    public const float Player_ClickPointer_ActiveTime = 1f;

    // static

    public static float Player_DamageModifier = 1f;
    public static float Player_DefenceModifier = 1f;
    public static float Player_SpeedModifier = 1f;
}
