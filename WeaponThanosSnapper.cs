using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;

namespace WeaponThanosSnapper
{
    [MinimumApiVersion(171)]
    public partial class WeaponThanosSnapper : BasePlugin
    {
        public override string ModuleName => "WeaponThanosSnapper";
        public override string ModuleVersion => $"1.0 - {new DateTime(Builtin.CompileTime, DateTimeKind.Utc)}";
        public override string ModuleAuthor => "DEAFPS https://github.com/DEAFPS/";
        public override string ModuleDescription => "A Plugin to remove dropped weapons";

        public bool timerStarted = false;
        public CounterStrikeSharp.API.Modules.Timers.Timer weaponTimer = null;

        public override void Load(bool hotReload)
        {
            RegisterEventHandler<EventRoundStart>((@event, info) =>
            {
                StartWeaponTimer();
                return HookResult.Continue;
            });

            Console.WriteLine("[WeaponThanosSnapper] Plugin Loaded");
        }

        public void StartWeaponTimer()
        {
            if(weaponTimer != null)
            {
                weaponTimer.Kill();
                Console.WriteLine("[WeaponThanosSnapper] Old Weapon Timer Removed!");
            }
            
            weaponTimer = AddTimer(1.0f, RemoveWeaponsOnTheGround);
            Console.WriteLine("[WeaponThanosSnapper] Weapon Timer started!");
        }

        public static void RemoveWeaponsOnTheGround()
        {
            var entities = Utilities.FindAllEntitiesByDesignerName<CCSWeaponBaseGun>("weapon_");

            foreach (var entity in entities)
            {
                if (!entity.IsValid)
                {
                    continue;
                }

                if (entity.State != CSWeaponState_t.WEAPON_NOT_CARRIED)
                {
                    continue;
                }

                if (entity.DesignerName.StartsWith("weapon_") == false)
                {
                    continue;
                }

                entity.Remove();
            }
        }
    }
}
