using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace DamageUI
{
    public class Plugin : RocketPlugin<Config>
    {
        public static Plugin Instance;

        protected override void Load()
        {
            base.Load();
            Instance = this;

            Logger.Log("----------------------------");
            Logger.Log("- Plugin created: https://discord.gg/PaDvuGPSyK");
            Logger.Log("- tg: t.me/pyqpeckin");
            Logger.Log("----------------------------");

            DamageTool.damagePlayerRequested += DamageTool_damagePlayerRequested;
            DamageTool.damageZombieRequested += DamageTool_damageZombieRequested;
            DamageTool.damageAnimalRequested += DamageTool_damageAnimalRequested;
            ResourceManager.onDamageResourceRequested += Damage_Resource;
            BarricadeManager.onDamageBarricadeRequested += Damage_Barricade;
            StructureManager.onDamageStructureRequested += Damage_Structure;
        }

        private void Damage_Structure(CSteamID instigatorSteamID, Transform structureTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            try
            {
                UnturnedPlayer uKiller = UnturnedPlayer.FromCSteamID(instigatorSteamID);
                ushort damage = pendingTotalDamage;
                EffectManager.sendUIEffect(Configuration.Instance.EffectID, getKey, uKiller.SteamPlayer().transportConnection, false, string.Format(Configuration.Instance.color.Structure, damage));
            }
            catch { }
        }

        private void Damage_Barricade(CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            try
            {
                UnturnedPlayer uKiller = UnturnedPlayer.FromCSteamID(instigatorSteamID);
                ushort damage = pendingTotalDamage;
                EffectManager.sendUIEffect(Configuration.Instance.EffectID, getKey, uKiller.SteamPlayer().transportConnection, false, string.Format(Configuration.Instance.color.Barricade, damage));
            }
            catch { }
        }

        private void Damage_Resource(CSteamID instigatorSteamID, Transform objectTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            try
            {
                UnturnedPlayer uKiller = UnturnedPlayer.FromCSteamID(instigatorSteamID);
                ushort damage = pendingTotalDamage;

                EffectManager.sendUIEffect(Configuration.Instance.EffectID, getKey, uKiller.SteamPlayer().transportConnection, false, string.Format(Configuration.Instance.color.Resource, damage));
            }
            catch { }
        }

        private void DamageTool_damageAnimalRequested(ref DamageAnimalParameters parameters, ref bool shouldAllow)
        {
            UnturnedPlayer uKiller = UnturnedPlayer.FromPlayer((Player)parameters.instigator);
            byte damage = (byte)Mathf.Min(255, Mathf.FloorToInt(parameters.damage)); // damage armor

            EffectManager.sendUIEffect(Configuration.Instance.EffectID, getKey, uKiller.SteamPlayer().transportConnection, false, string.Format(Configuration.Instance.color.Animal, damage));
        }

        private void DamageTool_damageZombieRequested(ref DamageZombieParameters parameters, ref bool shouldAllow)
        {
            UnturnedPlayer uKiller = UnturnedPlayer.FromPlayer((Player)parameters.instigator);
            float zombieArmor = DamageTool.getZombieArmor(parameters.limb, parameters.zombie); // ger armor
            byte damage = (byte)Mathf.Min(255, Mathf.FloorToInt(parameters.damage * zombieArmor)); // damage armor

            EffectManager.sendUIEffect(Configuration.Instance.EffectID, getKey, uKiller.SteamPlayer().transportConnection, false, string.Format(Configuration.Instance.color.Zombie, damage));
        }

        private void DamageTool_damagePlayerRequested(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
            try
            {
                UnturnedPlayer uKiller = UnturnedPlayer.FromCSteamID(parameters.killer); // get killer
                float playerArmor = DamageTool.getPlayerArmor(parameters.limb, parameters.player); // ger armor
                byte damage = (byte)Mathf.Min(255, Mathf.FloorToInt(parameters.damage * playerArmor)); // damage armor

                ELimb limb = parameters.limb;
                if (limb == ELimb.SKULL)
                    EffectManager.sendUIEffect(Configuration.Instance.EffectID, getKey, uKiller.SteamPlayer().transportConnection, false, string.Format(Configuration.Instance.color.Player_Skulll, damage));
                else EffectManager.sendUIEffect(Configuration.Instance.EffectID, getKey, uKiller.SteamPlayer().transportConnection, false, string.Format(Configuration.Instance.color.Player_Body, damage));
            }
            catch { }
        }
        public short getKey 
        {
            get
            {
                System.Random rand = new System.Random();
                return (short)rand.Next(short.MaxValue);
            }
        }

        protected override void Unload()
        {
            Logger.Log("----------------------------");
            Logger.Log("- Plugin created: https://discord.gg/PaDvuGPSyK");
            Logger.Log("- tg: t.me/pyqpeckin");
            Logger.Log("----------------------------");

            DamageTool.damagePlayerRequested += DamageTool_damagePlayerRequested;
            DamageTool.damageZombieRequested += DamageTool_damageZombieRequested;
            DamageTool.damageAnimalRequested += DamageTool_damageAnimalRequested;
            ResourceManager.onDamageResourceRequested += Damage_Resource;
            BarricadeManager.onDamageBarricadeRequested += Damage_Barricade;
            StructureManager.onDamageStructureRequested += Damage_Structure;

            Instance = null;
            base.Unload();
        }
    }
}
