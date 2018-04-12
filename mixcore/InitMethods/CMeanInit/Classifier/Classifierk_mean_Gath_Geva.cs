using System;
using Matrix_component.MatrixN;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.PittsburghClassifier.add_generators.I_k_mean
{
    class Classifierk_mean_Gath_Geva : Classifierk_mean_base
    {

        public Classifierk_mean_Gath_Geva(SampleSet Learn_table, int Max_iter, double precision_needed, int needed_count_clusters, double nebula)
            : base(Learn_table, Max_iter, precision_needed, needed_count_clusters, nebula)
        {
           
        }





        protected override void init_U_matrix()
        {
            Classifierk_mean_base k_l = new Classifierk_mean_base(learn_table, Max_iterate, Needed_precision, count_clusters, nebulisation_factor);
            k_l.Calc();
            u_Matrix = k_l.U_matrix;
        }
      

      
        protected override void calc_Distance()
        {
            for (int i =0; i<count_clusters;i++)
            {
                double Pi = calc_Probability_Pi(i);
                Matrix Ai= calc_fuzzy_covariance_Matrix_A(i);
                double determinant_Ai=Ai.Determinant();
                Matrix Reverse_of_Ai= Ai.Inverse();
                for (int e=0;e<learn_table.CountSamples;e++)
            {
                    Matrix x_v = Matrix_distance_beetween_x_v(i, e);
                    Matrix x_v_T = x_v.Transpose();
                    Matrix Result = x_v_T * Reverse_of_Ai * x_v;
                    double result_of_matrix = Result.GetElement(0,0);

                    double final_result = Math.Pow(determinant_Ai,1/nebulisation_factor)/Pi;
                    final_result *= Math.Exp(0.5 * result_of_matrix);
                  Distance_Matrix_d[i][e] = round_by_zero_or_high_value(final_result);
                 
            }
            }
        }




        protected Matrix calc_fuzzy_covariance_Matrix_A(int number_of_cluster)
        {
            Matrix A = new Matrix(learn_table.CountVars, learn_table.CountVars);

            double denominate = 0;
            for (int e = 0; e < learn_table.CountSamples; e++)
            {
                denominate += Math.Pow(u_Matrix[number_of_cluster][e], nebulisation_factor);
            }

            for (int e = 0; e < learn_table.CountSamples; e++)
            {
                Matrix x_v = Matrix_distance_beetween_x_v(number_of_cluster, e);
                Matrix x_v_T = x_v.Transpose();
                Matrix nominate = x_v * x_v_T;
                nominate = nominate.Multiply(Math.Pow(u_Matrix[number_of_cluster][e], nebulisation_factor));

                A += nominate;
            }
            A = A.Multiply(1 / denominate);
            return A;
        }






        protected Matrix Matrix_distance_beetween_x_v(int cluster_number, int point_count)
        {
            Matrix x_v = new Matrix(learn_table.CountVars, 1);
            for (int j = 0; j < learn_table.CountVars; j++)
            {
                x_v.SetElement(j, 0, Centroid_cordinate_s[cluster_number][j] - learn_table.DataRows[point_count].InputAttributeValue[j]);
            }
            return x_v;
        }







        protected double calc_Probability_Pi(int number_of_cluster)
        {
            double nominator = 0;
            for (int e = 0; e < learn_table.CountSamples; e++)
            {
                nominator += Math.Pow(u_Matrix[number_of_cluster][e], nebulisation_factor);
            }
            return nominator / learn_table.CountSamples;
        }




    }
}
