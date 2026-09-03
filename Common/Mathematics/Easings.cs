using System.Runtime.CompilerServices;

namespace QuestBooks.Common.Mathematics;

public static class Easings
{
    public static class Quadratic
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float In(float t) => t * t;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Out(float t) => 1f - In(1f - t);
    }

    public static class Cubic
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float In(float t) => t * t * t;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Out(float t) => 1f - In(1f - t);
    }
}