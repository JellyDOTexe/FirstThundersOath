using System;
using System.Collections.Generic;

namespace FirstThundersOath
{
    // First Thunder's Oath, the Meadows set. Each item clones a vanilla (or Therzie Warfare) weapon for its moveset,
    // sounds and skill, then sets its own damage. Target: a little better than Meadows gear, below the bronze tier
    // (players can go straight to the Black Forest, or farm Eikthyr a few times to be better prepared).
    // Every piece adds a small lightning bonus: Eikthyr's storm.
    internal sealed class WeaponDef
    {
        public string Prefab, CloneFrom, Fallback, Asset, Name, Description;
        public float HeadFraction = 0.3f;           // top part of the model used for the swing trail
        public bool FlipBlade;                      // rotate 180 degrees around the haft if the blade faces the wrong way (shield: turn it right side up)
        public float ShieldRoll;                    // degrees to turn a shield's face in its own plane (round shields have no long axis to line up)
        public bool IsShield;                       // round shield: fitted to the vanilla shield by size and facing, no haft
        public string WarfareJuggernaut;            // register with Therzie Warfare's Juggernaut (Piercer) effect, like its own war pikes
        public bool FlipHead;                       // the head goes on the end nearer the hand (vanilla spears are held near the middle)
        public bool ThrownModel;                    // the thrown projectile (spear) shows our model too
        public float StaminaFactor = 1f;            // attack stamina multiplier (two-handers: 0.8)
        public Action<ItemDrop.ItemData.SharedData> Stats;
        public (string item, int amount, int perLevel)[] Recipe;
        public int StationLevel = 2;
    }

    internal static class Weapons
    {
        private const int Lightning = 4, LightningPerLevel = 1;

        // Vanilla numbers for reference (dumped from the game 2026-11-13):
        //   Club blunt 12 | Bronze Mace blunt 35 | Bronze Sword slash 35 | Flint Axe slash 20 chop 30 | Bronze Axe slash 40
        //   Bone War Pike pierce 38 | Stagbreaker blunt 20 + pierce 5 | Bronze Atgeir pierce 45 | Wood Shield block 6
        //   Bronze Buckler block 16 | Flint Knife 5/5 | Copper Knife 12/12 | Flint Spear pierce 20 | Bronze Spear pierce 35
        private static (string, int, int)[] Std(int wood, int flint, string hide, int hideAmount) => new[]
        {
            ("Wood", wood, wood / 2), ("Flint", flint, flint / 2), (hide, hideAmount, 1), ("HardAntler", 1, 0)
        };

        private static void Storm(ItemDrop.ItemData.SharedData s)
        {
            s.m_damages.m_lightning = Lightning;
            s.m_damagesPerLevel.m_lightning = LightningPerLevel;
        }

        internal static readonly List<WeaponDef> All = new List<WeaponDef>
        {
            new WeaponDef
            {
                // Bronze Mace for the full mace moveset (the Club has no secondary attack)
                Prefab = "MaceFirstThunder", CloneFrom = "MaceBronze", Asset = "meadows_mace", HeadFraction = 0.22f,
                Name = "Firstfall",
                Description = "The first blow taught the grass to remember thunder.",
                Stats = s => { s.m_damages.m_blunt = 22; Storm(s); },
                Recipe = Std(8, 6, "LeatherScraps", 3),
            },
            new WeaponDef
            {
                Prefab = "SwordFirstThunder", CloneFrom = "SwordBronze", Asset = "meadows_sword", HeadFraction = 0.75f,
                Name = "Stormfurrow",
                Description = "They planted flint along the grain and reaped the storm.",
                Stats = s => { s.m_damages.m_slash = 24; Storm(s); },
                Recipe = Std(6, 10, "LeatherScraps", 3),
            },
            new WeaponDef
            {
                // chop and tool tier stay the Flint Axe's: it fells trees like a Meadows axe
                Prefab = "AxeFirstThunder", CloneFrom = "AxeFlint", Asset = "meadows_axe", HeadFraction = 0.3f, FlipBlade = true,
                Name = "Oakwake",
                Description = "The grove gave timber to those who silenced its thunder.",
                Stats = s => { s.m_damages.m_slash = 26; Storm(s); },
                Recipe = Std(6, 8, "LeatherScraps", 3),
            },
            new WeaponDef
            {
                // Warfare's Bone War Pike gives the war pike moveset; without Warfare it falls back to the Bronze Atgeir
                Prefab = "PikeFirstThunder", CloneFrom = "WarpikeBone_TW", Fallback = "AtgeirBronze", WarfareJuggernaut = "yes",
                Asset = "meadows_pike", HeadFraction = 0.22f, StaminaFactor = 0.8f,
                Name = "Altarward",
                Description = "At the standing stones, even the storm kept its distance.",
                Stats = s => { s.m_damages.m_pierce = 30; Storm(s); },
                Recipe = Std(12, 10, "DeerHide", 4),
            },
            new WeaponDef
            {
                // Stagbreaker is the first sledge in the game; its swing already ends in a burst
                Prefab = "SledgeFirstThunder", CloneFrom = "SledgeStagbreaker", Asset = "meadows_sledge", HeadFraction = 0.23f, StaminaFactor = 0.8f,
                Name = "Stormcleft",
                Description = "They bound the oak the lightning could not finish.",
                Stats = s => { s.m_damages.m_blunt = 26; s.m_damages.m_pierce = 5; Storm(s); },
                Recipe = Std(20, 6, "DeerHide", 4),
            },
            new WeaponDef
            {
                Prefab = "AtgeirFirstThunder", CloneFrom = "AtgeirBronze", Asset = "meadows_atgeir", HeadFraction = 0.27f, StaminaFactor = 0.8f,
                Name = "Meadow Reaper",
                Description = "Its hook gathered the foe; its edge opened the dawn.",
                Stats = s => { s.m_damages.m_pierce = 32; Storm(s); },
                Recipe = Std(12, 10, "DeerHide", 4),
            },
            new WeaponDef
            {
                Prefab = "ShieldFirstThunder", CloneFrom = "ShieldWood", Asset = "meadows_shield", IsShield = true, ShieldRoll = 90f,
                Name = "Dawnring",
                Description = "Behind split birch, the first hearth weathered the storm.",
                Stats = s => { s.m_blockPower = 10; s.m_blockPowerPerLevel = 6; },   // Wood Shield 6 (+6), Bronze Buckler 16
                Recipe = new[] { ("Wood", 10, 5), ("Resin", 4, 2), ("DeerHide", 4, 2), ("HardAntler", 1, 0) },
            },
            new WeaponDef
            {
                Prefab = "KnifeFirstThunder", CloneFrom = "KnifeFlint", Asset = "meadows_dagger", HeadFraction = 0.55f,
                Name = "Fawnfang",
                Description = "A small edge kept the last watch before sunrise.",
                Stats = s => { s.m_damages.m_slash = 8; s.m_damages.m_pierce = 8; Storm(s); },
                Recipe = Std(2, 6, "LeatherScraps", 2),
            },
            new WeaponDef
            {
                Prefab = "SpearFirstThunder", CloneFrom = "SpearFlint", Asset = "meadows_spear", HeadFraction = 0.15f, ThrownModel = true, FlipHead = true,
                Name = "Rainrunner",
                Description = "It flew once, and the thunder fell quiet.",
                Stats = s => { s.m_damages.m_pierce = 26; Storm(s); },
                Recipe = Std(6, 10, "LeatherScraps", 2),
            },
            new WeaponDef
            {
                // the vanilla Crude Bow is a rigid mesh (its limbs never bend), so ours swaps in like any other weapon
                Prefab = "BowFirstThunder", CloneFrom = "Bow", Asset = "meadows_bow", HeadFraction = 0.5f, FlipBlade = true,
                Name = "Stillbranch",
                Description = "The quiet bend held the memory of the storm.",
                Stats = s => { s.m_damages.m_pierce = 26; Storm(s); },   // Crude Bow 22, Finewood Bow 32
                Recipe = new[] { ("Wood", 10, 5), ("LeatherScraps", 6, 3), ("DeerHide", 2, 1), ("HardAntler", 1, 0) },
            },
        };
    }
}
