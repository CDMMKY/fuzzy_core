namespace ReCalcUFSForm
{
    public class abstract_RecombineUFS
    {
        protected string source;

        public string Source
        {
            get { return source; }  
        }


protected        double GIBNormal = 0;

        public double GIBNormalI
        {
            get { return GIBNormal; }

        }
        protected double GIBSumStraigth = 0;

        public double GIBSumStraigthI
        {
            get { return GIBSumStraigth; }
        }
        protected double GIBSumReverce = 0;

        public double GIBSumReverceI
        {
            get { return GIBSumReverce; }
        }
        protected double GISNormal = 0;

        public double GISNormalI
        {
            get { return GISNormal; }
        }
        protected double GISSumStraigth = 0;

        public double GISSumStraigthI
        {
            get { return GISSumStraigth; }
        }
        protected double GISSumReverce = 0;

        public double GISSumReverceI
        {
            get { return GISSumReverce; }

        }
        protected double GICNormal = 0;

        public double GICNormalI
        {
            get { return GICNormal; }

        }
        protected double GICSumStraigh = 0;

        public double GICSumStraighI
        {
            get { return GICSumStraigh; }
        }
        protected double GICSumReverce = 0;

        public double GICSumReverceI
        {
            get { return GICSumReverce; }
        }
        protected double LindisNormal = 0;

        public double LindisNormalI
        {
            get { return LindisNormal; }

        }
        protected double LindisSumStraigh = 0;

        public double LindisSumStraighI
        {
            get { return LindisSumStraigh; }

        }
        protected double LindisSumReverce = 0;

        public double LindisSumReverceI
        {
            get { return LindisSumReverce; }

        }
        protected double NormalIndex = 0;

        public double NormalIndexI
        {
            get { return NormalIndex; }

        }
        protected double SumsStraigthIndex = 0;

        public double SumsStraigthIndexI
        {
            get { return SumsStraigthIndex; }
        }
        protected double SumReverseIndex = 0;

        public double SumReverseIndexI
        {
            get { return SumReverseIndex; }
        }




        public abstract_RecombineUFS(string UFSPath)
        {
            source = UFSPath; 
        }
        public virtual void Work ()
        {
        }

    }
}
