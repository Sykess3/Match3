namespace _Project.Code.Core.Models
{
    public static class CommonExtensions
    {
        private static System.Random _random = new System.Random();
        public static bool RandomBoolean(this System.Random random)
        {
            var nextDouble = _random.NextDouble();
            return (nextDouble > 0.5);
        }
    }
}