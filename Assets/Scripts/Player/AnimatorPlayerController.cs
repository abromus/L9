public static class AnimatorPlayerController
{
    public static class Params
    {
        public const string Damage = nameof(Damage);
        public const string Cure = nameof(Cure);
    }

    public static class States
    {
        public const string Idle = nameof(Idle);
        public const string Damaged = nameof(Damaged);
        public const string Cured = nameof(Cured);
    }
}