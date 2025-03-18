using System;

namespace _23DaysLeft.Utils
{
    public static class Extensions
    {
        public static string GetName(this SceneType scene)
        {
            return scene switch
            {
                SceneType.Title   => "TitleTest",
                SceneType.Loading => "LoadingScene",
                SceneType.Main    => "MonsterTest",
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

        public static string GetClipName(this SoundType soundType)
        {
            return soundType switch
            {
                SoundType.MonsterHit   => "MonsterHit",
                SoundType.LoadingBGM   => "CampFire",
                SoundType.AnimalDead   => "AnimalDead",
                SoundType.SkeletonDead => "SkeletonDead",
                SoundType.GolemDead    => "GolemDead",
                SoundType.BossDead     => "BossDead",
                _                      => throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null)
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

    public enum SoundType
    {
        LoadingBGM,
        MonsterHit,
        AnimalDead,
        SkeletonDead,
        GolemDead,
        BossDead,
    }

    #endregion
}