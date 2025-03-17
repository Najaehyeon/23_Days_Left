using _23DaysLeft.Managers;
using _23DaysLeft.Monsters;
using System;

namespace _23DaysLeft.Utils
{
    public static class Extensions
    {
        public static string GetName(this SceneType scene)
        {
            return scene switch
            {
                SceneType.Title   => "TitleScene",
                SceneType.Loading => "LoadingScene",
                SceneType.Main    => "MainScene",
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
    }
}