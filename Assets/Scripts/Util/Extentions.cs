using System;

public static class Extensions
{
    public static string ToKey(this Creatures creatures)
    {
        return creatures switch
        {
            Creatures.Colobus => "Colobus",
            Creatures.Squid   => "Squid",
            Creatures.Taipan  => "Taipan",
            _                 => throw new ArgumentOutOfRangeException(nameof(creatures), creatures, null)
        };
    }
}
