namespace FuzzySystem.FuzzyAbstract
{
    public static class FuzzySystemRelisedList
    {
        #region Fuzzy system Types
        public enum TypeSystem
        {
            Singletone = 0,
            PittsburghClassifier = 1,
            TakagiSugenoApproximate = 2
        }




        public static object[] AllTypesFuzzySystem
        {
            get
            {
                return new object[] { "Аппроксиматор Синглтон" , //Singletone = 0
        "Классификатор Питтсбургский", //PittsburgClassifier =1
        "Такаги-Сугено" // TakagiSugenoApproximate =2
        };

            }


        }

        #endregion
    }
}
