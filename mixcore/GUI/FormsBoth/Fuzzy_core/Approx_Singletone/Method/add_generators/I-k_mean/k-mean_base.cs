using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone;

namespace Fuzzy_system.Approx_Singletone.add_generators.I_k_mean
{
    class k_mean_base
    {
        protected a_samples_set learn_table;
        public a_samples_set Learn_table { get { return learn_table; } }
        public List<List<double>> U_matrix { get { return u_Matrix; } }
        protected List<List<double>> u_Matrix = new List<List<double>>();
        protected List<List<double>> Distance_Matrix_d = new List<List<double>>();
        protected List<List<double>> Centroid_cordinate_s = new List<List<double>>();
        public List<List<double>> Centroid_cordinate_S { get { return Centroid_cordinate_s; } }
        public int Max_iterate { get; set; }
        public double Needed_precision { get; set; }
        protected int count_clusters;
        public int Count_clusters { get { return count_clusters; } }
        protected double nebulisation_factor;
        public double Nebulisation_factor { get { return nebulisation_factor; } }

        protected double round_by_zero_or_high_value(double d)
        {
            if (Math.Abs(d) < Math.Pow(10, -17))
            {
                return 0;
            }
            else
            {
                if (d > Math.Pow(10, 50))
                { return Math.Pow(10, 50); }
                else { return d; }
            }


        }


        protected List<List<double>> Clone(List<List<double>> Source)
        {
            List<List<double>> Result = new List<List<double>>();
            for (int i = 0; i < Source.Count; i++)
            {
                List<double> temp_List = Source[i].ToList();
                Result.Add(temp_List);
            }
            return Result;
        }


        protected virtual void init_U_matrix()
        {
            int count_samples = learn_table.Count_Samples;



            int mod;
            int count_point_to_cluster = Math.DivRem(learn_table.Count_Samples, Count_clusters, out mod);
            for (int i = 0; i < count_clusters; i++)
            {
                u_Matrix.Add(new List<double>());


                for (int j = 0; j < count_samples; j++)
                {
                    if ((i * count_point_to_cluster <= j) && (j < (i + 1) * count_point_to_cluster))
                    {
                        u_Matrix[i].Add(1.0);
                    }
                    else { u_Matrix[i].Add(0.0); }

                }

            }
            for (int i = mod; i > 0; i--)
            {
                u_Matrix[count_clusters - 1][count_samples - i] = 1.0;
            }

        }


        protected void init_Centroid_cordinate_s()
        {

            for (int i = 0; i < count_clusters; i++)
            {
                Centroid_cordinate_s.Add(new List<double>());
                for (int j = 0; j < learn_table.Count_Vars; j++)
                {
                    Centroid_cordinate_s[i].Add(0.0);
                }
            }
        }


        protected void init_Distance_Matrix_d()
        {

            for (int i = 0; i < count_clusters; i++)
            {
                Distance_Matrix_d.Add(new List<double>());
                for (int j = 0; j < learn_table.Count_Samples; j++)
                {
                    Distance_Matrix_d[i].Add(double.PositiveInfinity);
                }
            }
        }


        protected virtual void Centroid_cordinate_S_Calc()
        {

            for (int i = 0; i < count_clusters; i++)
            {
                for (int j = 0; j < learn_table.Count_Vars; j++)
                {
                    double nominator = 0;
                    double denominator = 0;
                    for (int e = 0; e < learn_table.Count_Samples; e++)
                    {
                        nominator += Math.Pow(u_Matrix[i][e], nebulisation_factor) * learn_table.Data_Rows[e].Input_Attribute_Value[j];
                        denominator += Math.Pow(u_Matrix[i][e], nebulisation_factor);
                    }
                    Centroid_cordinate_s[i][j] = nominator / denominator;
                }
            }




        }


                protected void update_u_Matrix() // Optimized variant of update
                {
                    for (int e = 0; e < learn_table.Count_Samples; e++)
                    {
                        double nominator = 0;
                        for (int k = 0; k < count_clusters; k++)
                        {
                            nominator += Distance_Matrix_d[k][e];
                        }

                        double sum = 0;
                        for (int i = 0; i < count_clusters; i++)
                        {
                            u_Matrix[i][e] = round_by_zero_or_high_value(Math.Pow(nominator / Distance_Matrix_d[i][e], 2 / (nebulisation_factor - 1)));

                            sum += u_Matrix[i][e];
                        }

                        for (int i = 0; i < count_clusters; i++)
                        {

                            if (Distance_Matrix_d[i][e] == 0)
                            { u_Matrix[i][e] = 1; }
                            else
                            {
                                if (Distance_Matrix_d[i][e] < 0)
                                { u_Matrix[i][e] = 0; }
                                else
                                {
                                    u_Matrix[i][e] = u_Matrix[i][e] / sum;
                                }

                            }

                        }
                    }
                


                }

       
          




        protected virtual void calc_Distance()
        {

            for (int i = 0; i < count_clusters; i++)
            {
                for (int e = 0; e < learn_table.Count_Samples; e++)
                {
                    double result = 0.0;
                    for (int j = 0; j < learn_table.Count_Vars; j++)
                    {
                        result += Math.Pow(Centroid_cordinate_s[i][j] - learn_table.Data_Rows[e].Input_Attribute_Value[j], 2);
                    }
                    Distance_Matrix_d[i][e] = round_by_zero_or_high_value(Math.Pow(result, 0.5));

                }
            }
        }

        public k_mean_base(a_samples_set Learn_table, int Max_iter, double precision_needed, int needed_count_clusters, double nebula)
        {
            learn_table = Learn_table;
            Max_iterate = Max_iter;
            Needed_precision = precision_needed;
            count_clusters = needed_count_clusters;
            nebulisation_factor = nebula;
        }


        public double objective_func()
        {
            double result = 0;


            for (int i = 0; i < count_clusters; i++)
            {
                for (int j = 0; j < learn_table.Count_Samples; j++)
                {
                    result += Math.Pow(u_Matrix[i][j], nebulisation_factor) *
                       Math.Pow(Distance_Matrix_d[i][j], 2);
                }
            }

            return result;
        }


        public void Calc()
        {
            init_U_matrix();
            init_Centroid_cordinate_s();
            init_Distance_Matrix_d();

            int current_iteration = 0;
            double previous_distance = double.PositiveInfinity;
            double last_beetween_distance = double.PositiveInfinity;
            List<List<double>> backup_Centroid;
            List<List<double>> backup_Distance;
            List<List<double>> backup_u_Matrix;

            do
            {
                backup_Centroid = Clone(Centroid_cordinate_s);
                Centroid_cordinate_S_Calc();

                backup_Distance = Clone(Distance_Matrix_d);
                calc_Distance();
                double current_distance = objective_func();
                backup_u_Matrix = Clone(u_Matrix);
                update_u_Matrix();

                if (Math.Abs(current_distance - previous_distance) < Needed_precision)
                {
                    break;
                }


                if ((double.IsInfinity(current_distance)) || (double.IsNaN(current_distance)))
                {
                    Centroid_cordinate_s = backup_Centroid;
                    Distance_Matrix_d = backup_Distance;
                    u_Matrix = backup_u_Matrix;

                    break;
                }
                last_beetween_distance = Math.Abs(current_distance - previous_distance);
                current_iteration++;
                previous_distance = current_distance;

            } while (current_iteration < Max_iterate);
        }

    }
}
