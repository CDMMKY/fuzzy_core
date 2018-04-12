namespace FuzzySystem.FuzzyAbstract.Hybride
{

    public abstract class FuzzyHybrideBase
    {
        protected int maximumSize = 200;
        object lockerStorage = new object();


        public enum goodness
        {
            best = 0,
            random = 1
        }

        public enum islandStrategy
        {
            One = 0,
            All = 1
        }
    }
}
