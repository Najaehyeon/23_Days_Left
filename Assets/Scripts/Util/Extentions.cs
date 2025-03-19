using System;

namespace _23DaysLeft.Utils
{
    public static class Extensions
    {
        public static string GetName(this SceneType scene)
        {
            return scene switch
            {
                SceneType.Title   => "TitleMenu",
                SceneType.Loading => "LoadingScene",
                SceneType.Main    => "PlayerScene",
                _                 => throw new ArgumentOutOfRangeException(nameof(scene), scene, null)
            };
        }

        public static string ToName(this Creatures creatures)
        {
            return creatures switch
            {
                Creatures.Colobus         => "콜로부스",
                Creatures.Squid           => "스퀴드",
                Creatures.Taipan          => "타이판",
                Creatures.Skeleton_Easy   => "스켈레톤",
                Creatures.Skeleton_Normal => "강한 스켈레톤",
                Creatures.Skeleton_Hard   => "화난 스켈레톤",
                Creatures.Golem           => "골렘",
                Creatures.PinkGolem       => "핑크골렘",
                Creatures.GreenGolem      => "그린골렘",
                Creatures.PurpleGolem     => "퍼플골렘",
                Creatures.RedGolem        => "레드골렘",
                Creatures.GiantGolem      => "1차 보스",
                Creatures.GreenGiantGolem => "2차 보스",
                Creatures.RedGiantGolem   => "3차 보스",
                _                         => throw new ArgumentOutOfRangeException(nameof(creatures), creatures, null)
            };
        }

        public static string GetClipName(this SoundTypeEnum soundTypeEnum)
        {
            return soundTypeEnum switch
            {
                SoundTypeEnum.MonsterHit   => "MonsterHit",
                SoundTypeEnum.LoadingBGM   => "CampFire",
                SoundTypeEnum.AnimalDead   => "AnimalDead",
                SoundTypeEnum.SkeletonDead => "SkeletonDead",
                SoundTypeEnum.GolemDead    => "GolemDead",
                SoundTypeEnum.BossDead     => "BossDead",
                SoundTypeEnum.Punch1       => "Punch Sound 1",
                SoundTypeEnum.Punch2       => "Punch Sound 2",
                SoundTypeEnum.StoneMiningSound1 => "stone mining sound 1",
                SoundTypeEnum.StoneMiningSound2 => "stone mining sound 2",
                SoundTypeEnum.StoneMiningSound5 => "stone mining sound 5",
                SoundTypeEnum.SwordSoundEffects1 => "Sword SoundEffectts 1",
                SoundTypeEnum.SwordSoundEffects2 => "Sword SoundEffectts 2",
                SoundTypeEnum.WalkSoundEffects1 => "Walk SoundEffectts 1",
                SoundTypeEnum.WalkSoundEffects2 => "Walk SoundEffectts 2",
                SoundTypeEnum.WoodSoundEffects1 => "Wood SoundEffectts 1",
                SoundTypeEnum.WoodSoundEffects2 => "Wood SoundEffectts 2",
                SoundTypeEnum.BossJumpAttack => "JumpAttack Sound",
                SoundTypeEnum.BossKickAttack => "KickAttack Sound",
                _                      => throw new ArgumentOutOfRangeException(nameof(soundTypeEnum), soundTypeEnum, null)
            };
        }
    }

    #region Enum

    public enum SceneType
    {
        Title,
        Loading,
        Main,
    }

    public enum Creatures
    {
        Colobus,
        Squid,
        Taipan,
        Skeleton_Easy,
        Skeleton_Normal,
        Skeleton_Hard,
        Golem,
        PinkGolem,
        GreenGolem,
        PurpleGolem,
        RedGolem,
        GiantGolem,
        GreenGiantGolem,
        RedGiantGolem,
    }

    public enum SoundTypeEnum
    {
        LoadingBGM,
        MonsterHit,
        AnimalDead,
        SkeletonDead,
        GolemDead,
        BossDead,
        Punch1,
        Punch2,
        StoneMiningSound1,
        StoneMiningSound2,
        StoneMiningSound5,
        SwordSoundEffects1,
        SwordSoundEffects2,
        WalkSoundEffects1,
        WalkSoundEffects2,
        WoodSoundEffects1,
        WoodSoundEffects2,
        BossJumpAttack,
        BossKickAttack,
    }

    #endregion
}