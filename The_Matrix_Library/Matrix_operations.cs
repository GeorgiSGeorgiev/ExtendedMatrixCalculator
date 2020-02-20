using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Extended_Matrix_Calculator;

namespace Matrix_Operations
{
    class Matrix_Library
    {
        static private int H; //aktualni radek (vyuziva se to behem prevadeni do RREF tvaru)
        static private int K; //aktualni sloupec (vyuziva se to behem prevadeni do RREF tvaru)
        static public double determinant_factor;
        static public int total_number_of_simulations = 0;
        static private Random rnd_generator = new Random();
        //static private Random rnd2 = new Random();
        static private Boolean Power_it_limit_reached = false;
        static public double[] First_Eigenvector;
        static private Boolean End_of_Line = false;
        static private Boolean stop_reading = false;
        static private int all_around_counter = 0;


        static private double Read_Number(System.IO.Stream file, ref int cursor)
        {
            double number = 0;
            Boolean minus = false;
            Boolean the_point = false;
            stop_reading = false;
            int decimal_places_counter = 0;
            cursor = file.ReadByte();
            while (!(cursor >= 48 && cursor <= 57) && !stop_reading)
            {
                if (cursor == 45)
                {
                    minus = true;
                }
                else if (cursor == 10 || cursor == 13)
                {
                    End_of_Line = true;
                }
                else if (cursor == -1)
                {
                    stop_reading = true;
                    number = -1;
                }
                cursor = file.ReadByte();
            }

            while (cursor >= 48 && cursor <= 57 || cursor == 44 || cursor == 46)
            {
                if (the_point)
                {
                    decimal_places_counter++;
                }
                if (cursor == 44 || cursor == 46)
                {
                    the_point = true;
                }
                else
                {
                    number = number * 10 + (cursor - 48);
                }
                cursor = file.ReadByte();
            }

            for (int i = 0; i < decimal_places_counter; i++)
            {
                number = number / 10;
            }

            if (minus == true)
            {
                number = number * (-1);
            }

            return number;
        }

        static public double[,] Read_File_and_Save_Matrix(System.IO.Stream Read_File, int Rows, int Columns)
        {
            double[,] the_result_matrix = new double[Rows, Columns];
            int cursor = 0;
            int i = 0, j = 0;
            double number = 0;
            Boolean Error = false;
            while (i != Rows)
            {
                number = Read_Number(Read_File, ref cursor);
                if (number == -1 && stop_reading)
                {
                    i = Rows;
                    Error = Writings.Show_Dimensions_missmatch_error(Error);
                }
                else
                {
                    the_result_matrix[i, j] = number;
                    if (j < Columns - 1)
                    {
                        if (End_of_Line)
                        {
                            End_of_Line = false;
                        }
                        j++;
                    }
                    else
                    {
                        if (!End_of_Line && j == Columns || End_of_Line && j != Columns)
                        {
                            Error = Writings.Show_Dimensions_missmatch_error(Error);
                        }
                        if (End_of_Line)
                        {
                            End_of_Line = false;
                        }

                        j = 0;
                        if (i < Rows)
                        {
                            i++;
                        }
                        else
                        {
                            Error = Writings.Show_Dimensions_missmatch_error(Error);
                        }
                    }
                }
            }
            number = Read_Number(Read_File, ref cursor);
            if (!stop_reading && !Error)
            {
                Error = Writings.Show_Dimensions_missmatch_error(Error);
            }
            if (Error)
            {
                the_result_matrix = null;
            }
            return the_result_matrix;
        }

        static public string Convert_MX_VR_to_string(int rows, int columns, double[,] the_matrix)
        {
            string result = "";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result += Convert.ToString(Math.Round(the_matrix[i, j], 2)) + " "; 
                }
                result += Environment.NewLine;
            }
            return result;
        }

        static public string Convert_MX_VR_to_string(int length, double[] the_vector)
        {
            string result = "";
            for (int i = 0; i < length; i++)
            {
                result += Convert.ToString(Math.Round(the_vector[i], 2)) + " ";
            }
            return result;
        }

        static public string Convert_MX_VR_to_string(int length, int[] the_vector)
        {
            string result = "";
            for (int i = 0; i < length; i++)
            {
                result += Convert.ToString(the_vector[i]) + " ";
            }
            return result;
        }

        static public string Convert_MX_VR_to_string(int length, Boolean[] the_vector)
        {
            string result = "";
            for (int i = 0; i < length; i++)
            {
                result += Convert.ToString(the_vector[i]) + " ";
            }
            return result;
        }

        static public void Write_List_Capacity_in_MessBox(string name, List<int> The_List)
        {
            string List_values = name + ": ";
            for (int i = 0; i < The_List.Count; i++)
            {
                List_values += " " + Convert.ToString(The_List); 
            }
            MessageBox.Show(List_values);
        }

        static public double[] copy_vector(int length, double[] original_vector)
        {
            double[] result = new double[length];
            if (original_vector != null)
            {
                for (int i = 0; i < length; i++)
                {
                    result[i] = original_vector[i];
                }
            }
            else
            {
                result = null;
            }
            return result;
        }


        static public double[,] copy_matrix(int rows, int columns, double[,] original_matrix)
        {
            double[,] result = new double[rows, columns];
            for (int i = 0; i< rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i, j] = original_matrix[i, j];
                }
            }
            return result;
        }

        static public double[,] Vector_to_1_row_matrix(int length, double[] vector)
        {
            double[,] Result = new double[1, length];
            for (int j = 0; j < length; j++)
            {
                Result[0, j] = vector[j];
            }
            return Result;
        }

        static public double[,] Vector_to_1_column_matrix(int length, double[] vector)
        {
            double[,] Result = new double[length, 1];
            for (int i = 0; i < length; i++)
            {
                Result[i, 0] = vector[i];
            }
            return Result;
        }

        static public double[] one_column_matrix_to_vector(int rows, double[,] matrix)
        {
            double[] Result = new double[rows];
            for (int i = 0; i < rows; i++)
            {
                Result[i] = matrix[i, 0];
            }
            return Result;
        }


        static public Boolean Zero_vector_detector(int length, double[] vector, int accuracy)
        {
            Boolean vector_is_zero = true;
            double temp_round = 0;
            for (int i = 0; i < length; i++)
            {
                temp_round = Matrix_Library.Round(vector[i], accuracy);
                if (temp_round != 0)
                {
                    vector_is_zero = false;
                }
            }
            return vector_is_zero;
        }


        static public Boolean Zero_matrix_detector(int rows, int columns, double[,] the_matrix, int accuracy)
        {
            Boolean zero_matrix = true;
            double temp_round = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    temp_round = Matrix_Library.Round(the_matrix[i, j], accuracy);
                    if (temp_round != 0)
                    {
                        zero_matrix = false;
                    }
                }
            }
            return zero_matrix;
        }


        //Equality
        static public Boolean Equal_vectors(int length, double[] vector1, double[] vector2)
        {
            double v1_rounded = 0, v2_rounded = 0;
            Boolean Equal = true;
            for (int i = 0; i < length; i++)
            {
                v1_rounded = Matrix_Library.Round(vector1[i], 0);
                v2_rounded = Matrix_Library.Round(vector2[i], 0);
                if (v1_rounded != v2_rounded)
                {
                    Equal = false;
                }
            }
            return Equal;
        }

        static public Boolean Equal_Matrices(int rows, int columns, double[,] Matrix1, double[,] Matrix2)
        {
            double v1_rounded = 0, v2_rounded = 0;
            Boolean Equal = true;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    v1_rounded = Matrix_Library.Round(Matrix1[i, j], 15);
                    v2_rounded = Matrix_Library.Round(Matrix2[i, j], 15);
                    if (v1_rounded != v2_rounded)
                    {
                        Equal = false;
                    }
                }
            }
            return Equal;
        }

        //Round double number
        static public double Round(double number, int accuracy)
        {
            double Result = 0;
            if (accuracy < 15)
            {
                Result = Math.Round(number, accuracy);
            }
            else
            {
                Result = number;
            }
            return Result;
        }


        //Round vector
        static public double[] Round(int length, double[] vector, int accuracy)
        {
            double[] Result = new double[length];
            if (accuracy < 15)
            {
                for (int i = 0; i < length; i++)
                {
                    Result[i] = Math.Round(vector[i], accuracy);
                }
            }
            else
            {
                Result = copy_vector(length, vector);
            }
            return Result;
        }

        //Round matrix
        static public double[,] Round(int rows, int columns, double[,] matrix, int accuracy)
        {
            double[,] Result = new double[rows, columns];
            if (accuracy < 15)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        Result[i, j] = Math.Round(matrix[i, j], accuracy);
                    }
                }
            }
            else
            {
                Result = copy_matrix(rows, columns, matrix);
            }

            return Result;
        }

        //Euclidean norm
        static public double Euclidean_norm(int length, double[] vector)
        {
            double result = 0;
            for (int i = 0; i < length; i++)
            {
                result += vector[i] * vector[i];
            }
            result = Math.Sqrt(result);
            return result;
        }

        //Swap Rows
        static public double[,] Swap_Rows(int number_of_rows, int number_of_columns, double[,] matrix, int row1, int row2)
        {
            double[] temp_row = new double[number_of_columns];
            for (int j = 0; j < number_of_columns; j++)
            {
                temp_row[j] = matrix[row1, j];
                matrix[row1, j] = matrix[row2, j];
                matrix[row2, j] = temp_row[j];
            }
            return matrix;
        }

        //Swap Columns
        static public double[,] Swap_Columns(int number_of_rows, int number_of_columns, double[,] matrix, int column1, int column2)
        {
            double[] temp_column = new double[number_of_rows];
            for (int i = 0; i < number_of_rows; i++)
            {
                temp_column[i] = matrix[i, column1];
                matrix[i, column1] = matrix[i, column2];
                matrix[i, column2] = temp_column[i];
            }
            return matrix;
        }

        //Sum two matrices
        static public double[,] Sum_Two_Matrices(int rows, int columns, double[,] matrix1, double[,] matrix2, int accuracy)
        {
            double[,] result = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
            result = Matrix_Library.Round(rows, columns, result, accuracy);
            return result;
        }

        static public double[,] Sum_Two_Matrices_EigenVectors_Edition(int rows, int columns, double[,] matrix1, double[,] matrix2)
        {
            double[,] result = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns - 1; j++)
                {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
            return result;
        }

        //Subtract two matrices
        static public double[,] Subtract_Two_Matrices(int rows, int columns, double[,] matrix1, double[,] matrix2, int accuracy)
        {
            double[,] result = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }
            result = Matrix_Library.Round(rows, columns, result, accuracy);
            return result;
        }


        //Matrix multiplication by scalar
        static public double[,] Multiplication_by_scalar(double scalar, int rows, int columns, double[,] matrix, int accuracy)
        {
            double[,] result = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i, j] = scalar * matrix[i, j];
                }
            }
            result = Matrix_Library.Round(rows, columns, result, accuracy);
            return result;
        }

        //Vector multiplication by scalar, inside usage
        static public double[] Multiplication_by_scalar(double scalar, int length, double[] vector)
        {
            double[] result = new double[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = scalar * vector[i];
            }
            return result;
        }


        //Vector vs Vector Multiplication 1, inside usage
        static public double Matrix_Multiplication(int length, double[] row_vector, double[] column_vector)
        {
            double result = 0;
            for (int i = 0; i < length; i++)
            {
                result += row_vector[i] * column_vector[i];
            }
            return result;
        }

        //Vector vs Vector Multiplication 2, inside usage
        static public double[,] Matrix_Multiplication(int col_length, int row_length, double[] colummn_vector, double[] row_vector)
        {
            double[,] result = new double[col_length, row_length];
            for (int i = 0; i < col_length; i++)
            {
                for (int j = 0; j < row_length; j++)
                {
                    result[i, j] = colummn_vector[i] * row_vector[j];
                }
            }
            return result;
        }

        //Matrix vs Vector Multiplication, inside usage
        static public double[] Matrix_Multiplication(int rows, int columns, double[,] matrix, int length, double[] vector)
        {
            double[] result = new double[rows];
            for (int i = 0; i < rows; i++)
            {
                result[i] = 0;
                for (int j = 0; j < columns; j++)
                {
                    result[i] = result[i] + matrix[i, j] * vector[j];
                }
            }
            return result;
        }


        //Multiply + Multiplication by scalar, accuracy needed
        static public double[,] Matrix_Multiplication(int rows1, int columns1, double[,] matrix1, int rows2, int columns2, double[,] matrix2, int accuracy)
        {
            Boolean scalar_multiplication = false;
            double scalar = 0;
            double[,] result;
            int res_rows = 0, res_columns = 0;

            if (rows1 == 1 && columns1 == 1 || rows2 == 1 && columns2 == 1)
            {
                scalar_multiplication = true;
            }
            if (scalar_multiplication)
            {
                if (rows1 == 1 && columns1 == 1)
                {
                    scalar = matrix1[0, 0];
                    res_rows = rows2;
                    res_columns = columns2;
                    result = new double[res_rows, res_columns];
                    result = copy_matrix(res_rows, res_columns, matrix2);
                }
                else
                {
                    scalar = matrix2[0, 0];
                    res_rows = rows1;
                    res_columns = columns1;
                    result = new double[res_rows, res_columns];
                    result = copy_matrix(res_rows, res_columns, matrix1);
                }

                for (int i = 0; i < res_rows; i++)
                {
                    for (int j = 0; j < res_columns; j++)
                    {
                        result[i, j] = scalar * result[i, j];
                    }
                }
            }
            else // normal 2 matrix multiplication 
            {
                res_rows = rows1;
                res_columns = columns2;
                result = new double[res_rows, res_columns];
                for (int i = 0; i < rows1; i++)
                {
                    for (int j = 0; j < columns2; j++)
                    {
                        result[i, j] = 0;
                        for (int k = 0; k < columns1; k++)
                        {
                            result[i, j] += matrix1[i, k] * matrix2[k, j];
                        }
                    }
                }
            }
            result = Matrix_Library.Round(res_rows, res_columns, result, accuracy);
            return result;
        }

        //Transpose
        static public double[,] Transposition(int rows, int columns, double[,] matrix, int accuracy)
        {
            double[,] result = new double[columns, rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }
            result = Matrix_Library.Round(columns, rows, result, accuracy);
            return result;
        }

        //Merge two matrices (or vectors) - LSM
        static public double[,] Merge_Matrices(int rows1, int columns1, double[,] matrix1, int columns2, double[,] matrix2)
        {
            int matrix2_columns = 0;
            double[,] result = new double[rows1, columns1 + columns2];
            for (int i = 0; i < rows1; i++)
            {
                for (int j = 0; j < columns1 + columns2; j++)
                {
                    if (j < columns1)
                    {
                        result[i, j] = matrix1[i, j];
                    }
                    else
                    {
                        result[i, j] = matrix2[i, matrix2_columns];
                        if (matrix2_columns < columns2)
                        {
                            matrix2_columns++;
                        }
                        else
                        {
                            MessageBox.Show(Writings.Error_In_Merging);
                        }
                    }
                }
                matrix2_columns = 0;
            }
            return result;
        }

        //Merge two matrices vertically, zmenil jsem trosicku pristup aby to bylo kratsi
        static public Tuple<int, int, double[,]> Merge_Matrices_Vertically(int rows1, int columns1, double[,] matrix1, int rows2, double[,] matrix2)
        {
            int matrix2_rows_i = 0;
            int result_rows = rows1 + rows2;
            double[,] result = new double[rows1 + rows2, columns1];
            for (int i = 0; i < result_rows; i++)
            {
                for (int j = 0; j < columns1; j++)
                {
                    if (i < rows1)
                    {
                        result[i, j] = matrix1[i, j];
                    }
                    else
                    {
                        matrix2_rows_i = i - rows1;
                        result[i, j] = matrix2[matrix2_rows_i, j];
                    }
                }
            }
            return Tuple.Create<int, int, double[,]>(result_rows, columns1, result);
        }


        //Max argument in one column (below pivot)
        static public int Find_Max_Absolute_Argument(int rows, int columns, double[,] the_matrix)
        {
            int index = H;
            double Max_Argument = Math.Abs(the_matrix[index, K]);
            int Max_Argument_Index = H;
            for (index = H; index < rows; index++)
            {
                if (Math.Abs(the_matrix[index, K]) > Max_Argument)
                {
                    Max_Argument = Math.Abs(the_matrix[index, K]);
                    Max_Argument_Index = index;
                }
            }
            return Max_Argument_Index;
        }


        //Find minimal argument in one column (below pivot) without counting the zeros
        static public int Find_Min_Without_0s(int rows, int columns, double[,] the_matrix)
        {
            int index;
            double Min_Argument = 0;
            int Min_Argument_Index = -1;
            bool end = false;
            for (index = H; index < rows && !end; index++) // najdi nenulovou slozku s vetsim nez H indexem v danem sloupci
            {
                if (the_matrix[index, K] != 0)
                {
                    end = true;
                    Min_Argument = the_matrix[index, K];
                    Min_Argument_Index = index;
                }
            }
            for (index = H; index < rows; index++)
            {
                if (the_matrix[index, K] < Min_Argument && the_matrix[index, K] != 0)
                {
                    Min_Argument = the_matrix[index, K];
                    Min_Argument_Index = index;
                }
            }
            return Min_Argument_Index;
        }


        //REF
        static public double[,] REF(int rows, int columns, double[,] the_matrix, int accuracy)
        {
            H = 0; K = 0;
            determinant_factor = 1;
            int i_min = 0;
            double coefficient = 0;
            bool end = false;

            while (H < rows && K < columns && !end)
            {
                end = true;
                for (int k = H; k < rows; k++)
                {
                    for (int l = K; l < columns; l++)
                    {
                        if (the_matrix[k, l] != 0)
                        {
                            end = false;
                        }
                    }
                }

                i_min = Find_Min_Without_0s(rows, columns, the_matrix);
                if (i_min == -1)
                {
                    K++; //preskoci nulove sloupce
                }
                else
                {
                    if (i_min != H)
                    {
                        the_matrix = Swap_Rows(rows, columns, the_matrix, i_min, H);
                        determinant_factor = (-1) * determinant_factor;
                    }

                    for (int i = H + 1; i < rows; i++) //odecteni z nizsich radku
                    {
                        coefficient = the_matrix[i, K] / the_matrix[H, K];
                        the_matrix[i, K] = 0; //radek pod se vynuluje
                        for (int j = K + 1; j < columns; j++)
                        {
                            the_matrix[i, j] = the_matrix[i, j] - the_matrix[H, j] * coefficient;
                        }
                    }
                    H++;
                    K++;
                }
            }
            the_matrix = Matrix_Library.Round(rows, columns, the_matrix, accuracy);
            return the_matrix;
        }


        //Special RREF double number rounding, interesting bug fix
        static public double RREF_num_rounding(double the_number)
        {
            double result = 0;
            if (Math.Round(the_number, 5) == Math.Round(the_number, 6))
            {
                result = Math.Round(the_number, 6);
            }
            else
            {
                result = the_number;
            }
            return result;
        }

        //RREF
        static public double[,] RREF(int rows, int columns, double[,] the_matrix, bool full_RREF, int accuracy)
        {
            H = 0; K = 0;
            int i_max = 0;
            double pivot_value = 0;
            double koeficient1 = 0;
            double koeficient2 = 0;
            int columns_RREF_mode;
            if (full_RREF)
            {
                columns_RREF_mode = columns;
            }
            else
            {
                columns_RREF_mode = columns - 1;
            }
            //potrebuji prestat s RREF upravami o 1 krok driv
            while (H < rows && K < columns_RREF_mode)
            {
                i_max = Find_Max_Absolute_Argument(rows, columns, the_matrix);
                pivot_value = the_matrix[i_max, K];
                if (pivot_value != 0 && pivot_value != 1) // vydelim radek hodnotou pivotu
                {
                    for (int j = K; j < columns; j++)
                    {
                        the_matrix[i_max, j] = the_matrix[i_max, j] / pivot_value;
                        the_matrix[i_max, j] = RREF_num_rounding(the_matrix[i_max, j]);
                    }
                }
                //MessageBox.Show(Convert.ToString(pivot_value));
                if (pivot_value == 0)
                {
                    K++; //preskoci nulove sloupce
                }
                else //odecitani z vyssich radku
                {
                    the_matrix = Swap_Rows(rows, columns, the_matrix, H, i_max);
                    if (H > 0)
                    {
                        for (int i2 = 0; i2 < (H); i2++)
                        {
                            koeficient2 = the_matrix[i2, K];
                            the_matrix[i2, K] = 0;
                            for (int j2 = K + 1; j2 < columns; j2++)
                            {
                                the_matrix[i2, j2] = the_matrix[i2, j2] - the_matrix[H, j2] * koeficient2;
                                the_matrix[i2, j2] = RREF_num_rounding(the_matrix[i2, j2]);
                            }
                        }
                    }
                    for (int i = H + 1; i < rows; i++) //odecteni z nizsich radku
                    {
                        koeficient1 = the_matrix[i, K] / the_matrix[H, K];
                        the_matrix[i, K] = 0; //radek pod se vynuluje
                        for (int j = K + 1; j < columns; j++)
                        {
                            the_matrix[i, j] = the_matrix[i, j] - the_matrix[H, j] * koeficient1;
                            the_matrix[i, j] = RREF_num_rounding(the_matrix[i, j]);
                        }
                    }
                    H++;
                    K++;
                }
                //MessageBox.Show("End of step: " + Environment.NewLine + Convert_MX_to_string(rows, columns, the_matrix));
            }
            the_matrix = Matrix_Library.Round(rows, columns, the_matrix, accuracy);
            return the_matrix;
        }


        //Matrix Rank finder
        static public int Matrix_Rank_Finder(int rows, int columns, double[,] the_matrix, int accuracy)
        {
            Boolean non_zero_row = false;
            int the_Rank = 0;
            double[,] copy_of_A = new double[rows, columns];
            double[,] temp_RREF_matrix = new double[rows, columns];
            copy_of_A = copy_matrix(rows, columns, the_matrix);

            temp_RREF_matrix = RREF(rows, columns, copy_of_A, true, accuracy);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns && !non_zero_row; j++)
                {
                    if (temp_RREF_matrix[i,j] != 0)
                    {
                        non_zero_row = true;
                    }
                }
                if (non_zero_row)
                {
                    the_Rank++;
                    non_zero_row = false;
                }
            }
            return the_Rank;
        }

        //Identity matrix creation
        static public double[,] create_identity_matrix(int rows, int columns)
        {
            double[,] result = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (i == j)
                    {
                        result[i, j] = 1;
                    }
                    else
                    {
                        result[i, j] = 0;
                    }
                }
            }
            return result;
        }


        //Extract bottom right part of the Matrix
        static public double[,] Extract_part_of_the_Matrix(int rows_total, int columns_total, double[,] whole_matrix, int beginning_row, int ending_row, int beginning_column, int ending_column)
        {
            int temp = 0;
            if (beginning_row > ending_row)
            {
                temp = beginning_row;
                beginning_row = ending_row;
                ending_row = temp;
            }
            if (beginning_column > ending_column)
            {
                temp = beginning_column;
                beginning_column = ending_column;
                ending_column = temp;
            }
            double[,] the_result = new double[ending_row - beginning_row + 1, ending_column - beginning_column + 1];
            try
            {
                for (int i = beginning_row; i <= ending_row; i++)
                {
                    for (int j = beginning_column; j <= ending_column; j++)
                    {
                        the_result[i - beginning_row, j - beginning_column] = whole_matrix[i, j];
                    }
                }
            }
            catch (Exception The_Exception)
            {
                MessageBox.Show(Writings.Cutting_error + The_Exception.Message);
            }

            return the_result;
        }


        //Inverse
        static public Tuple<Boolean, double[,]> Inverse(int rows, int columns, double[,] the_matrix, int accuracy)
        {
            Boolean Inverse_successful = true;
            //int result_cheker_rank = 0;
            double[,] Result_matrix = new double[rows, columns];
            double[,] Result_checker = new double[rows, columns];
            double[,] Identity_matrix = new double[rows, columns];
            double[,] the_temp_big_matrix = new double[rows, 2 * columns];
            Identity_matrix = Matrix_Library.create_identity_matrix(rows, columns);
            the_temp_big_matrix = Matrix_Library.Merge_Matrices(rows, columns, the_matrix, columns, Identity_matrix);
            the_temp_big_matrix = Matrix_Library.RREF(rows, 2 * columns, the_temp_big_matrix, true, accuracy);
            Result_matrix = Extract_part_of_the_Matrix(rows, 2 * columns, the_temp_big_matrix, 0, rows - 1, columns, 2 * columns - 1);
            Result_checker = Extract_part_of_the_Matrix(rows, 2 * columns, the_temp_big_matrix, 0, rows - 1, 0, columns - 1);

            Result_matrix = Matrix_Library.Round(rows, columns, Result_matrix, accuracy);
            Result_checker = Matrix_Library.Round(rows, columns, Result_checker, 3);
            //result_cheker_rank = Matrix_Library.Matrix_Rank_Finder(rows, columns, Result_checker, 3);
            /*if (result_cheker_rank == rows)
            {
                Inverse_successful = true;
            }*/
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (Result_checker[i, j] != 1 && i == j || Result_checker[i, j] != 0 && i != j)
                    {
                        Inverse_successful = false;
                    }
                }
            }
            return Tuple.Create(Inverse_successful, Result_matrix);
        }


        //Determinant
        static public Tuple<double, double[,]> Determinant(int rows, int columns, double[,] the_matrix, int accuracy)
        {
            double result = 1;
            double[,] the_new_mx = new double[rows, columns];
            int j = 0;
            the_new_mx = REF(rows, columns, the_matrix, accuracy);
            for (int i = 0; i < rows && j < columns; i++, j++)
            {
                result = result * the_new_mx[i, j];
            }
            result = result * determinant_factor;
            return Tuple.Create(result, the_new_mx);
        }

        //LSM, sort rows
        static public double[,] LSM_Sort(int number_of_samples, double[,] data)
        {
            Boolean swapped = true;
            double[,] temp = copy_matrix(number_of_samples, 2, data);
            while (swapped)
            {
                swapped = false;
                for (int i = 0; i < number_of_samples - 1; i++)
                {
                    if (temp[i, 0] > temp[i + 1, 0] || temp[i, 0] == temp[i + 1, 0] && temp[i, 1] > temp[i + 1, 1])
                    {
                        temp = Swap_Rows(number_of_samples, 2, temp, i, i + 1);
                        swapped = true;
                    }
                }
            }
            return temp;
        }

        //LSM, step 1: Prepare the matrix
        static public Tuple<double[,], double[,]> LSM_Prepare_matrix(int number_of_samples, int columns_according_to_mode, double[,] data)
        {
            int vector_columns = 1;
            double[,] the_result_matrix = new double[number_of_samples, columns_according_to_mode];
            double[,] the_result_vector = new double[number_of_samples, vector_columns];
            Boolean cube = false;

            if (columns_according_to_mode == 4)
            {
                cube = true;
            }
            else if (columns_according_to_mode != 3)
            {
                MessageBox.Show(Writings.LSM_Error);
            }

            for (int i = 0; i < number_of_samples; i++)
            {
                for (int j = 0; j < columns_according_to_mode; j++)
                {
                    if (j == 0)
                    {
                        if (cube)
                        {
                            the_result_matrix[i, j] = data[i, 0] * data[i, 0] * data[i, 0];
                        }
                        else
                        {
                            the_result_matrix[i, j] = data[i, 0] * data[i, 0];
                        }
                    }
                    else if (j == 1)
                    {
                        if (cube)
                        {
                            the_result_matrix[i, j] = data[i, 0] * data[i, 0];
                        }
                        else
                        {
                            the_result_matrix[i, j] = data[i, 0];
                        }
                    }
                    else if (j == 2)
                    {
                        if (cube)
                        {
                            the_result_matrix[i, j] = data[i, 0];
                        }
                        else
                        {
                            the_result_matrix[i, j] = 1;
                        }
                    }
                    else if (j == 3)
                    {
                        the_result_matrix[i, j] = 1;
                    }
                }

                the_result_vector[i, 0] = data[i, 1];

            }
            return Tuple.Create(the_result_matrix, the_result_vector);
        }


        //LSM, step 3: The solution extraction
        static public double[] The_LSM_solution(int rows, int columns, double[,] matrix)
        {
            double[] result = new double[rows];
            int column_index = columns - 1;
            for (int i = 0; i < rows; i++)
            {
                result[i] = matrix[i, column_index];
            }
            return result;
        }


        //LSM, step 2: The Main Part of the algorithm
        static public Tuple<double[], double[,]> LSM(int samples, int data_columns, double[,] data, int mx_columns, int accuracy)
        {
            int vector_columns = 1;

            //MessageBox.Show(Convert_MX_to_string(samples, data_columns, data));
            double[,] data_copy = copy_matrix(samples, 2, data);
            data_copy = LSM_Sort(samples, data_copy);

            double[,] matrix = new double[samples, mx_columns];
            double[,] vector = new double[samples, vector_columns];

            double[,] final_matrix = new double[mx_columns, mx_columns]; //ctvercova matice
            double[,] final_vector = new double[mx_columns, vector_columns];

            double[,] transponed_matrix = new double[mx_columns, samples];

            double[,] big_one = new double[mx_columns, mx_columns + vector_columns];
            double[] Result = new double[mx_columns]; // 3 nebo 4 slozky

            var Tuple_matrix_and_vector = new Tuple<double[,], double[,]>(matrix, vector);
            Tuple_matrix_and_vector = Matrix_Library.LSM_Prepare_matrix(samples, mx_columns, data_copy);
            matrix = Tuple_matrix_and_vector.Item1;
            //MessageBox.Show(Convert_MX_to_string(samples, mx_columns, matrix));
            vector = Tuple_matrix_and_vector.Item2;
            //MessageBox.Show(Convert_MX_to_string(samples, vector_columns, vector));
            transponed_matrix = Matrix_Library.Transposition(samples, mx_columns, matrix, 15); //transpozice
            final_matrix = Matrix_Library.Matrix_Multiplication(mx_columns, samples, transponed_matrix, samples, mx_columns, matrix, 15);
            final_vector = Matrix_Library.Matrix_Multiplication(mx_columns, samples, transponed_matrix, samples, vector_columns, vector, 15);
            big_one = Matrix_Library.Merge_Matrices(mx_columns, mx_columns, final_matrix, vector_columns, final_vector);
            big_one = Matrix_Library.RREF(mx_columns, mx_columns + vector_columns, big_one, false, 15); //RREF
            Result = Matrix_Library.The_LSM_solution(mx_columns, mx_columns + vector_columns, big_one);
            Result = Round(mx_columns, Result, accuracy);
            return Tuple.Create(Result, data_copy);
            //return one_column_matrix_to_vector(samples, vector);
        }


        //Random vector generator
        static public double[] Random_non_zero_generator(int length)
        {
            Boolean non_zero = false;
            double[] Result = new double[length];
            while (!non_zero)
            {
                for (int i = 0; i < length; i++)
                {
                    Result[i] = rnd_generator.Next(0, 999);
                    Result[i] = Result[i] / 1000; // random cislo z intervalu [0, 0.999)
                    if (Result[i] != 0)
                    {
                        non_zero = true;
                    }
                }
            }
            return Result;
        }

        //Random matrix generator
        static public double[,] Random_non_zero_generator(int rows, int columns)
        {
            Boolean non_zero = false;
            double[,] Result = new double[rows, columns];
            while (!non_zero)
            {
                for (int i = 0; i < rows; i++)
                {
                    non_zero = false;
                    for (int j = 0; j < columns; j++)
                    {
                        Result[i, j] = rnd_generator.Next(0, 999);
                        Result[i, j] = Result[i, j] / 1000; // random cislo z intervalu [0, 0.999)
                        if (Result[i, j] != 0)
                        {
                            non_zero = true;
                        }
                    }
                }
            }
            return Result;
        }


        //Eigenvector temporary regular matrix generator
        static public double[,] Random_non_zero_generator(int rows, int columns, double[] Eigenvector, int accuracy)
        {
            Boolean non_zero = false;
            Boolean regular = false;
            int Random_Matrix_Rank = 0;
            double[,] Result = new double[rows, columns];
            int counter = 0;

            for (int i = 0; i < rows; i++)
            {
                Result[i, 0] = Eigenvector[i];
            }

            while (!non_zero && !regular)
            {
                regular = false;
                for (int i = 0; i < rows; i++)
                {
                    non_zero = false;
                    for (int j = 1; j < columns; j++)
                    {
                        Result[i, j] = rnd_generator.Next(0, 1999);
                        Result[i, j] = Result[i, j] / 1000; // random cislo z intervalu [0, 1.999)
                        if (Result[i, j] != 0)
                        {
                            non_zero = true;
                        }
                    }
                }
                Random_Matrix_Rank = Matrix_Rank_Finder(rows, columns, Result, accuracy);
                if (Random_Matrix_Rank == rows && Random_Matrix_Rank == columns)
                {
                    regular = true;
                }
                counter++;
            }
            return Result;
        }


        static private double[,] Generate_1_0_Vectors(int kernel_dim, int length, int index)
        {
            double[,] The_Result = new double[kernel_dim, length - 1];
            //int Matrix_Rank = 0;
            int the_zero_position = 0;

            for (int i = 0; i < kernel_dim; i++)
            {
                the_zero_position = i - 1;
                for (int j = 0; j < length - 1; j++)
                {
                    if (i >= length && index == 0) //aby se vysledky neopakovaly
                    {
                        The_Result[i, j] = 0;
                    }
                    else if (j == the_zero_position) //nula zaruci linearni nezavyslost
                    {
                        The_Result[i, j] = 0;
                    }
                    else
                    {
                        if (j == 0) //prvni volny prvek na radku originalni matice ma vzdy opacne znamenko v srovnani s temp. countings
                        {
                            The_Result[i, j] = -1;
                        }
                        else
                        {
                            The_Result[i, j] = 1;
                        }
                    }
                }
            }
            //Matrix_Rank = Matrix_Rank_Finder(kernel_dim, length - 1, The_Result, 7);
            //MessageBox.Show(Convert.ToString(Random_Matrix_Rank) + " :Rank");
            return The_Result;
        }



        //Power iteration
        static public Tuple<double, double[]> Power_Iteration(int rows, int columns, double[,] matrix, int num_of_simulations_limit, int accuracy)
        {
            Boolean stop = false;
            total_number_of_simulations = 0;
            double Eigenvalue = 0;
            double Eigenvalue_one_step_ago = 2;
            double temp_scalar = 0;
            double[] Eigenvector = new double[rows];
            double[] y_vector = new double[rows];
            double[] one_step_ago = new double[rows];

            if (rows == 1 && columns == 1)
            {
                Eigenvalue = matrix[0, 0];
                Eigenvector[0] = 1;
            }
            else if (Zero_matrix_detector(rows, columns, matrix, accuracy))
            {
                Eigenvalue = 0;
                Eigenvector[0] = 1;
            }
            else
            {
                one_step_ago = Matrix_Library.Random_non_zero_generator(rows);

                y_vector = Matrix_Multiplication(rows, columns, matrix, rows, one_step_ago);
                temp_scalar = 1 / Euclidean_norm(rows, y_vector);
                Eigenvector = Multiplication_by_scalar(temp_scalar, rows, y_vector);

                while (Math.Abs(Eigenvalue - Eigenvalue_one_step_ago) > 0.0001 && !stop) // ukoncovaci podminka
                {
                    // opakuj PI
                    Eigenvalue_one_step_ago = Eigenvalue;
                    one_step_ago = copy_vector(rows, Eigenvector);
                    y_vector = Matrix_Multiplication(rows, columns, matrix, rows, one_step_ago);
                    temp_scalar = 1 / Euclidean_norm(rows, y_vector);
                    Eigenvector = Multiplication_by_scalar(temp_scalar, rows, y_vector);
                    Eigenvalue = Matrix_Multiplication(rows, one_step_ago, y_vector); // row_vector * column_vector
                    total_number_of_simulations++;
                    if (total_number_of_simulations >= num_of_simulations_limit)
                    {
                        if (!Power_it_limit_reached)
                        {
                            Power_it_limit_reached = true;
                        }
                        stop = true;
                    }
                }
            }
            //MessageBox.Show("Num of sims: " + Convert.ToString(number_of_sims));
            Eigenvalue = Matrix_Library.Round(Eigenvalue, accuracy);
            Eigenvector = Matrix_Library.Round(rows, Eigenvector, accuracy);

            //MessageBox.Show(Convert.ToString(total_number_of_simulations));
            //MessageBox.Show("EigVal: " + Convert.ToString(Eigenvalue));
            return Tuple.Create(Eigenvalue, Eigenvector);
        }



        //Extract A2
        static public double[,] Extract_A2(int rows, int columns, double[,] the_big_one)
        {
            int A2_rows = rows - 1, A2_columns = columns - 1;
            double[,] A2_matrix = new double[A2_rows, A2_columns];
            if (A2_rows != 0 && A2_columns != 0)
            {
                for (int i = 1; i < rows; i++)
                {
                    for (int j = 1; j < columns; j++)
                    {
                        A2_matrix[i - 1, j - 1] = the_big_one[i, j];
                    }
                }
            }
            else
            {
                A2_matrix = null;
            }
            return A2_matrix;
        }

        // Tricky row swap to make Power Iteration faster
        static public double[,] Swapping_some_rows(int rows, int columns, double[,] the_matrix)
        {
            Boolean bingo = false;
            double[,] result = new double[rows, columns];
            if (rows == columns && columns == 2)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (the_matrix[0, j] == 0)
                    {
                        bingo = true;
                    }
                }
                if (bingo)
                {
                    result = Matrix_Library.Swap_Rows(rows, columns, the_matrix, 0, 1);
                }
            }
            if (!bingo)
            {
                result = the_matrix;
            }
            return result;
        }

        // Eigenvalues and eigenvectors
        static public double[] All_Eigenvalues(int rows, int columns, double[,] matrix_A, int simulations, int accuracy)
        {
            Boolean stop = false;
            double[] Eigenvalues = new double[rows]; // rows == columns z definice
            double[,] matrix_A2;
            double[,] matrix_S;
            double[,] matrix_S_inversed;
            int A2_rows = rows, A2_columns = columns;
            double[,] temp_big = new double[rows, columns];

            for (int j = 0; j < columns && !stop; j++)
            {
                matrix_A2 = new double[A2_rows, A2_columns];
                matrix_S = new double[A2_rows, A2_columns];
                matrix_S_inversed = new double[A2_rows, A2_columns];
                if (j == 0)
                {
                    matrix_A2 = copy_matrix(rows, columns, matrix_A);
                }
                else
                {
                    matrix_A2 = Matrix_Library.Extract_A2(A2_rows + 1, A2_columns + 1, temp_big);
                    //MessageBox.Show("A2: " + Convert_MX_VR_to_string(A2_rows, A2_columns, matrix_A2));
                }
                var Eigen_value_vector_Tuple = new Tuple<double, double[]>(0, null);
                temp_big = null;
                temp_big = new double[A2_rows, A2_columns];
                //matrix_A2 = Matrix_Library.Swapping_some_rows(A2_rows, A2_columns, matrix_A2);
                //MessageBox.Show("A2: " + Convert_MX_to_string(A2_rows, A2_columns, matrix_A2));
                Eigen_value_vector_Tuple = Power_Iteration(A2_rows, A2_columns, matrix_A2, simulations, 15);
                if (j == 0)
                {
                    First_Eigenvector = new double[rows];
                    First_Eigenvector = copy_vector(rows, Eigen_value_vector_Tuple.Item2);
                }

                if (!Matrix_Library.Zero_vector_detector(A2_rows, Eigen_value_vector_Tuple.Item2, 7) && A2_rows > 0 && A2_columns > 0)
                {
                    matrix_S = Random_non_zero_generator(A2_rows, A2_columns, Eigen_value_vector_Tuple.Item2, 15);
                    //MessageBox.Show("Matrix S: " + Convert_MX_VR_to_string(A2_rows, A2_columns, matrix_S));
                    matrix_S_inversed = Matrix_Library.Inverse(A2_rows, A2_columns, matrix_S, 15).Item2;
                    temp_big = Matrix_Multiplication(A2_rows, A2_columns, matrix_A2, A2_rows, A2_columns, matrix_S, 15);
                    temp_big = Matrix_Multiplication(A2_rows, A2_columns, matrix_S_inversed, A2_rows, A2_columns, temp_big, 15);
                    Eigenvalues[j] = temp_big[0, 0];
                    //MessageBox.Show("Temp big: " + Convert_MX_to_string(A2_rows, A2_columns, temp_big));
                    A2_rows--; A2_columns--;
                }
                else
                {
                    stop = true;
                }
                Eigen_value_vector_Tuple = null;
                matrix_A2 = null;
                matrix_S = null;
                matrix_S_inversed = null;
            }
            return Eigenvalues;
        }


        //Precise Eigenvalues finder
        static public double[] Precise_Eigenvalues_finder(int rows, int columns, double[,] matrix_A, int simulations, int accuracy, int num_of_comparisons)
        {
            int Eigenvalues_count = rows;
            int[] Comparisons = new int[num_of_comparisons];
            double[,] Multiple_run_Eigenvalues = new double[num_of_comparisons, Eigenvalues_count];
            double[] Result_Eigenvalues = new double[Eigenvalues_count];
            double[] temp_Eigenvalues_vector1 = new double[Eigenvalues_count];
            double[] temp_Eigenvalues_vector2 = new double[Eigenvalues_count];

            int maximum_equal_vectors_count = 0;
            int Result_vector_index = 0;
            int accuracy_meter = 0;

            for (int i = 0; i < num_of_comparisons; i++) // ulozeni vektoru z ruznich behu funkce All_Eigenvalue do matice (jednotilive radky = vektory)
            {
                temp_Eigenvalues_vector1 = All_Eigenvalues(rows, columns, matrix_A, simulations, 15);
                for (int j = 0; j < Eigenvalues_count; j++)
                {
                    Multiple_run_Eigenvalues[i, j] = temp_Eigenvalues_vector1[j];
                }
            }

            for (int i = 0; i < num_of_comparisons - 1; i++) // srovnani vsech vektoru ze vsech behu
            {
                for (int j = 0; j < Eigenvalues_count; j++)
                {
                    temp_Eigenvalues_vector1[j] = Multiple_run_Eigenvalues[i, j];
                }
                for (int k = i + 1; k < num_of_comparisons; k++)
                {
                    for (int j = 0; j < Eigenvalues_count; j++)
                    {
                        temp_Eigenvalues_vector2[j] = Multiple_run_Eigenvalues[k, j];
                    }
                    if (Equal_vectors(Eigenvalues_count, temp_Eigenvalues_vector1, temp_Eigenvalues_vector2))
                    {
                        Comparisons[i]++;
                        Comparisons[k]++;
                    }
                }
            }

            for (int i = 0; i < num_of_comparisons; i++)
            {
                if (Comparisons[i] > maximum_equal_vectors_count)
                {
                    maximum_equal_vectors_count = Comparisons[i];
                    Result_vector_index = i;
                }
            }

            if (num_of_comparisons >= 15)
            {
                accuracy_meter = num_of_comparisons / 3 - 1;
            }
            else
            {
                accuracy_meter = num_of_comparisons / 2 - 2;
            }
            if (maximum_equal_vectors_count < accuracy_meter && ! Writings.Result_not_accurate_shown)
            {
                Writings.Show_Result_not_accurate_message();
                //MessageBox.Show("Warning: The result may not be accurate");
            }
            //MessageBox.Show(Convert_MX_to_string(num_of_comparisons, Comparisons));

            for (int j = 0; j < Eigenvalues_count; j++)
            {
                Result_Eigenvalues[j] = Math.Round(Multiple_run_Eigenvalues[Result_vector_index, j], accuracy); 
            }
            //MessageBox.Show(Convert_MX_VR_to_string(Eigenvalues_count, Result_Eigenvalues));
            return Result_Eigenvalues;
        }

        //Substract eigenvalue from the diagonal of the original matrix and do RREF
        public static double[,] Subtract_from_diagonal_plus_RREF(double eigenvalue, int N, double[,] the_matrix, int accuracy)
        {
            double[,] Result_Matrix = new double[N, N + 1];
            double[,] Temp = new double[N, N];
            double[,] Zero_Column = new double[N, 1];
            //eigenvalue = Math.Round(eigenvalue);
            for (int i = 0; i < N; i++) // vytvor Temp
            {
                Zero_Column[i, 0] = 0;
                for (int j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                        Temp[i, j] = the_matrix[i, j] - eigenvalue;
                    }
                    else
                    {
                        Temp[i, j] = the_matrix[i, j];
                    }
                }
            }
            //MessageBox.Show(Convert_MX_to_string(N, N, Temp));
            Result_Matrix = Merge_Matrices(N, N, Temp, 1, Zero_Column);
            //MessageBox.Show(Convert_MX_to_string(N, N + 1, Result));
            Result_Matrix = RREF(N, N + 1, Result_Matrix, false, accuracy);
            //MessageBox.Show(Convert_MX_to_string(N, N + 1, Result));
            return Result_Matrix;
        }


        //EigenValue Check, neni nutne zapotrebi, ale nekdy muze zlepsit presnost
        public static Tuple<Boolean[], double[]> Check_all_Eigenvalues(int N, double[,] the_matrix, double[] Eigenvalues)
        {
            Boolean stop = false;
            Boolean[] Are_Eigenvalues_Eigenvalues = new Boolean[N];
            double[] Result_Eigenvalues = new double[N];
            double[,] Temp_Matrix = new double[N, N + 1];
            double Rounded_Eigenvalue = 0;

            for (int i = 0; i < N; i++)
            {
                Temp_Matrix = Subtract_from_diagonal_plus_RREF(Eigenvalues[i], N, the_matrix, 15); // pokus bez zadneho zaokrouhleni
                //MessageBox.Show(Convert_MX_VR_to_string(N, N + 1, Temp_Matrix) + " Check Temp Matrix");
                if (Matrix_Rank_Finder(N, N+1, Temp_Matrix, 15) != N)
                {
                    stop = true;
                    Rounded_Eigenvalue = Eigenvalues[i];
                    //MessageBox.Show(Convert_MX_VR_to_string(N, N + 1, Temp_Matrix) + " Check Temp Matrix");
                    //MessageBox.Show(Convert.ToString(Matrix_Rank_Finder(N, N + 1, Temp_Matrix, 7)));
                }
                for (int Decimal_places = 15; Decimal_places >= 0 && !stop; Decimal_places--) // postupne zaokrouhluji vlastni cislo dokud neziskam to spravne
                { // cil je cislo najit ne jenom zaokrouhlovat, k samotnemu zaokrouhleni slouzi nasledujici funkce
                    Rounded_Eigenvalue = Round(Eigenvalues[i], Decimal_places);
                    Temp_Matrix = Subtract_from_diagonal_plus_RREF(Rounded_Eigenvalue, N, the_matrix, 15);
                    if (Matrix_Rank_Finder(N, N + 1, Temp_Matrix, 15) != N)
                    {
                        stop = true;
                    }
                }
                if (!stop)
                {
                    Rounded_Eigenvalue = Eigenvalues[i];
                    Are_Eigenvalues_Eigenvalues[i] = false;
                }
                else
                {
                    stop = false;
                    Are_Eigenvalues_Eigenvalues[i] = true;
                }
                Result_Eigenvalues[i] = Rounded_Eigenvalue;
            }
            //MessageBox.Show(Convert_MX_VR_to_string(N, Are_Eigenvalues_Eigenvalues));
            //MessageBox.Show(Convert_MX_VR_to_string(N, Result_Eigenvalues) + " Check");
            return Tuple.Create(Are_Eigenvalues_Eigenvalues, Result_Eigenvalues);
        }

        // Round the Eigenvalues, solve the rounding problem
        public static Tuple<Boolean[], double[]> Eigenvalue_Rounding(int N, double[,] the_matrix, Tuple<Boolean[], double[]> The_Eigenvalues)
        {
            Boolean stop = false;
            Boolean non_equal_matrices = false;
            Boolean methodic_rounding_mistake = false;
            Boolean big_diversion = false;
            Boolean Irrational_Eigenvalues = false;
            double[] Rounded_Eigenvalues = new double[N];
            double[,] Temp_Matrix_The_First = new double[N, N + 1];
            double[,] Temp_Matrix_The_Second = new double[N, N + 1];
            double rounded_Eigenvalue = -1;
            double rounded_Eigenvalue_next_step = -1;
            //MessageBox.Show(Convert_MX_VR_to_string(N, The_Eigenvalues.Item1));
            for (int i = 0; i < N; i++)
            {
                if (The_Eigenvalues.Item1[i] != true)
                {
                    stop = true;
                    Rounded_Eigenvalues[i] = The_Eigenvalues.Item2[i];
                }
                for (int accuracy = 7; accuracy >= 1 && !stop; accuracy--)
                {
                    rounded_Eigenvalue = Round(The_Eigenvalues.Item2[i], accuracy);
                    Temp_Matrix_The_First = Subtract_from_diagonal_plus_RREF(rounded_Eigenvalue, N, the_matrix, 15);
                    rounded_Eigenvalue_next_step = Round(The_Eigenvalues.Item2[i], accuracy - 1);
                    Temp_Matrix_The_Second = Subtract_from_diagonal_plus_RREF(rounded_Eigenvalue_next_step, N, the_matrix, 15);
                    if (Matrix_Rank_Finder(N, N, Temp_Matrix_The_First, 15) == N)
                    {
                        stop = true;
                        Rounded_Eigenvalues[i] = The_Eigenvalues.Item2[i];
                    }
                    if (!Equal_Matrices(N, N + 1, Temp_Matrix_The_First, Temp_Matrix_The_Second) && !stop)
                    {
                        non_equal_matrices = true;
                        //MessageBox.Show("Non equal");
                    }
                    if (Math.Abs(rounded_Eigenvalue - rounded_Eigenvalue_next_step) > 30 * Math.Pow(10, -accuracy) && !stop)
                    {
                        methodic_rounding_mistake = true;
                        //MessageBox.Show("methodic_rounding_mistake");
                    }
                    if (Math.Abs(rounded_Eigenvalue_next_step - The_Eigenvalues.Item2[i]) > 0.3 && !stop)
                    {
                        big_diversion = true;
                        //MessageBox.Show("big diversion");
                    }

                    if (((non_equal_matrices || methodic_rounding_mistake || big_diversion) && !stop) /*&& accuracy < 3 && !Power_it_limit_reached*/)
                    {
                        stop = true;
                        Rounded_Eigenvalues[i] = rounded_Eigenvalue;
                    }
                    else if (accuracy == 1 && !stop)
                    {
                        Rounded_Eigenvalues[i] = rounded_Eigenvalue_next_step;
                    }
                }
                stop = false;
                non_equal_matrices = false;
                methodic_rounding_mistake = false;
                big_diversion = false;
            }

            if (N == 2)
            {
                Irrational_Eigenvalues = true;
                for (int i = 0; i < N && !stop; i++)
                {
                    if (The_Eigenvalues.Item1[i] == true)
                    {
                        stop = true;
                        Irrational_Eigenvalues = false;
                    }
                }
                if (Irrational_Eigenvalues)
                {
                    Writings.Show_Irrational_numbers_message();
                    Irrational_Eigenvalues = false;
                }
            }
            
            //MessageBox.Show(Convert_MX_VR_to_string(N, The_Eigenvalues.Item1));
            //MessageBox.Show(Convert_MX_VR_to_string(N, Rounded_Eigenvalues) + " Rounding");
            return Tuple.Create(The_Eigenvalues.Item1, Rounded_Eigenvalues);
        }


        // Find the multiplicity
        public static Dictionary<double, int> Eigenvalue_Multiplicity(int N, double[] The_Eigenvalues)
        {
            //OrderedDictionary Eigenvectors_and_Multiplicity = new OrderedDictionary();
            Dictionary<double, int> Eigenvalues_and_Multiplicity = new Dictionary<double, int>();
            for (int i = 0; i < N; i++)
            {
                if (!Eigenvalues_and_Multiplicity.ContainsKey(The_Eigenvalues[i]))
                {
                    Eigenvalues_and_Multiplicity.Add(The_Eigenvalues[i], 1);
                }
                else
                {
                    Eigenvalues_and_Multiplicity[The_Eigenvalues[i]]++;
                }
            }
            return Eigenvalues_and_Multiplicity;
        }


        // Dictionary to two vectors
        public static Tuple<int, double[], int[]> Convert_Dict_to_vectors(Dictionary<double, int> Eigenvalues_and_Multiplicity)
        {
            double[] Eigenvalues = new double[Eigenvalues_and_Multiplicity.Count];
            Eigenvalues_and_Multiplicity.Keys.CopyTo(Eigenvalues, 0);
            int[] Multiplicity = new int[Eigenvalues_and_Multiplicity.Count];
            Eigenvalues_and_Multiplicity.Values.CopyTo(Multiplicity, 0);
            return Tuple.Create(Eigenvalues_and_Multiplicity.Count, Eigenvalues, Multiplicity);
        }


        //Final Eigenvalues Function
        public static Tuple<int, double[], int[]> Find_Eigenvalues_Everything_Together(int N, double[,] the_matrix, int simulations, int accuracy, int num_of_comparisons)
        {
            Tuple<Boolean[], double[]> The_Eigvalues_Temp = new Tuple<Boolean[], double[]>(null, null);
            Tuple<Boolean[], double[]> The_EigValues = new Tuple<Boolean[], double[]>(null, null);
            Tuple<int, double[], int[]> EigenValues_and_multiplicity_arrs = new Tuple<int, double[], int[]>(0, null, null);
            Dictionary<double, int> EigenValues_and_Multiplicity = new Dictionary<double, int>();
            Boolean stop = false;
            Boolean[] Are_Eigenvalues_Eigenvalues = new Boolean[N];
            double[] Result_Eigenvalues = new double[N];
            double[,] Temp_Matrix = new double[N, N + 1];

            int the_limit = 0;
            if (accuracy > 3)
            {
                the_limit = 4;
            }
            else
            {
                the_limit = accuracy + 1;
            }

            for (int i = 0; i < the_limit && !stop; i++)
            {
                Result_Eigenvalues = Precise_Eigenvalues_finder(N, N, the_matrix, simulations, accuracy, num_of_comparisons);
                The_Eigvalues_Temp = Check_all_Eigenvalues(N, the_matrix, Result_Eigenvalues);
                The_EigValues = Eigenvalue_Rounding(N, the_matrix, The_Eigvalues_Temp);
                The_Eigvalues_Temp = null;
                stop = true;
                for (int j = 0; j < N; j++)
                {
                    if (The_EigValues.Item1[j] != true)
                    {
                        stop = false;
                    }
                }
            }
            EigenValues_and_Multiplicity = Eigenvalue_Multiplicity(N, The_EigValues.Item2);
            EigenValues_and_multiplicity_arrs = Convert_Dict_to_vectors(EigenValues_and_Multiplicity);

            return EigenValues_and_multiplicity_arrs;
        }


        //Spectral decomposition- create Lambda matrix
        public static double[,] Create_Lambda_Matrix(int number_of_eigenvalues, double[] eigenvalues)
        {
            double[,] Big_Lambda = new double[number_of_eigenvalues, number_of_eigenvalues];
            for (int i = 0; i < number_of_eigenvalues; i++)
            {
                Big_Lambda[i, i] = eigenvalues[i];
            }
            return Big_Lambda;
        }


        //Lambda Matrix Final Function
        public static double[,] Lambda_All_Together(Tuple<int, double[], int[]> Eigenvalues)
        {
            int total_number_of_eigenvalues = 0;
            double single_eigenvalue = 0;
            double[] The_Eigenvalues_Vector;
            double[,] Big_Lambda;
            for (int i = 0; i < Eigenvalues.Item1; i++)
            {
                total_number_of_eigenvalues += Eigenvalues.Item3[i];
            }
            The_Eigenvalues_Vector = new double[total_number_of_eigenvalues];
            int k = 0;
            for (int j = 0; j < Eigenvalues.Item1; j++)
            {
                single_eigenvalue = Eigenvalues.Item3[j];
                while (single_eigenvalue > 0)
                {
                    single_eigenvalue--;
                    The_Eigenvalues_Vector[k] = Eigenvalues.Item2[j];
                    if (k < total_number_of_eigenvalues)
                    {
                        k++;
                    }
                    else
                    {
                        Writings.Show_lambda_error();
                    }
                }
            }
            Big_Lambda = new double[total_number_of_eigenvalues, total_number_of_eigenvalues];
            Big_Lambda = Create_Lambda_Matrix(total_number_of_eigenvalues, The_Eigenvalues_Vector);
            return Big_Lambda;
        }


        // Nulovy sloupec
        private static Boolean Zero_column(int N, double[,] the_matrix, int column_index)
        {
            Boolean zero_column = true;
            for (int i = 0; i < N; i++)
            {
                if (the_matrix[i, column_index] != 0)
                {
                    zero_column = false;
                }
            }
            return zero_column;
        }


        //Solution finder
        private static double[,] Solution_finder(int N, int rank, double[,] the_temp_matrix, int row, List<int> free_positions_in_the_row, double temp_value, int index)
        {
            int Kernel_dimension = N - rank;
            double[,] Temp_countings = new double[Kernel_dimension, N + 1];
            double temp_value_copy = temp_value;
            double[,] random_vectors = new double[Kernel_dimension, free_positions_in_the_row.Count - 1];
            int free_positions_count = free_positions_in_the_row.Count;
            int i = 0; // row index

            while (i < Kernel_dimension) // tonight i have to check this out
            {
                temp_value_copy = 0;
                random_vectors = Generate_1_0_Vectors(Kernel_dimension, free_positions_in_the_row.Count, index);
                //MessageBox.Show(Convert_MX_VR_to_string(Kernel_dimension, free_positions_in_the_row.Count - 1, random_vectors) + "Rnd");
                for (int free = 0; free < free_positions_in_the_row.Count - 1; free++)
                {
                    Temp_countings[i, free_positions_in_the_row[free]] = random_vectors[i, free];
                    temp_value_copy += Temp_countings[i, free_positions_in_the_row[free]] * the_temp_matrix[row, free_positions_in_the_row[free]];
                }
                //MessageBox.Show(Convert.ToString(temp_value_copy));
                int the_last_free_second_index = free_positions_in_the_row[free_positions_in_the_row.Count - 1];
                Temp_countings[i, the_last_free_second_index] = (-1) * temp_value_copy / the_temp_matrix[row, the_last_free_second_index];
                //MessageBox.Show(Convert_MX_VR_to_string(Kernel_dimension, N + 1, Temp_countings) + "Temp countings");
                i++;
            }


            return Temp_countings;
        }



        // Find eigenvectors 1, the monster
        private static double[,] Matrix_solver_Eigenvector_finder(int N, double[,] the_temp_matrix, double Eigenvalue, int rank, int Value_index) // the_temp_matrix <= double[N,N+1]
        { 
            int kernel_dimension = N - rank;
            double[,] Temporary_eigvecs_matrix = new double[kernel_dimension, N + 1];
            double[,] Result_eigenvectors = new double[kernel_dimension, N + 1];
            int last_non_zero_pos_on_the_row = 0;
            int row_Non_zero_counter = 0;
            int last_zero_position = 1;
            int the_0_1 = 0;
            int index = -1;
            double[] fixed_value = new double[kernel_dimension];
            List<int> all_positions_in_the_row = new List<int>();
            List<int> filled_positions_list = new List<int>();
            List<int> Non_zero_counters = new List<int>();
            List<int> Clear_positions = new List<int>();
            List<int> The_0_1s_List = new List<int>();
            List<int> Zero_Columns = new List<int>();
            Boolean stop_zero_vector_counting = false;
            Boolean already_counted = false;
            Boolean stop = false;

            //MessageBox.Show(Convert_MX_VR_to_string(N, N, the_temp_matrix) + " :The TMP");
            all_around_counter = 0;

            for (int k = N - 1; k >= 0; k--) // index radku matice
            {
                if (Clear_positions == null)
                {
                    Clear_positions = new List<int>();
                }

                for (int kernel = 0; kernel < kernel_dimension; kernel++) // spocitam fixed value pro kazdy vlastni vektor
                {
                    fixed_value[kernel] = 0;
                }

                row_Non_zero_counter = 0;
                last_non_zero_pos_on_the_row = 0;

                if (all_positions_in_the_row == null)
                {
                    all_positions_in_the_row = new List<int>();
                }

                for (int j = 0; j < N; j++) // spocitam nenuly v radku
                {
                    if (the_temp_matrix[k, j] != 0)
                    {
                        row_Non_zero_counter++;
                        all_positions_in_the_row.Add(j);
                        last_non_zero_pos_on_the_row = j;
                    }
                }

                if (all_positions_in_the_row != null)
                {
                    foreach (var position in all_positions_in_the_row) // spocitam fixed value pro dany radek
                    {
                        if (filled_positions_list.Contains(position))
                        {
                            for (int kernel = 0; kernel < kernel_dimension; kernel++) // spocitam fixed value pro kazdy vlastni vektor
                            {
                                fixed_value[kernel] += the_temp_matrix[k, position] * Result_eigenvectors[kernel, position];
                            }
                            Clear_positions.Add(position);
                        }
                        else
                        {
                            last_non_zero_pos_on_the_row = position;
                        }
                    }

                    for (int ker = 0; ker < kernel_dimension; ker++) // spocitam fixed value pro kazdy vlastni vektor
                    {
                        fixed_value[ker] = (-1) * fixed_value[ker];
                    }
                    //MessageBox.Show("Fixed: " + Convert.ToString(fixed_value));
                    foreach (var position in Clear_positions)
                    {
                        all_positions_in_the_row.Remove(position);
                    }
                    row_Non_zero_counter = all_positions_in_the_row.Count;
                }

                for (int j = 0; j < N && !stop_zero_vector_counting; j++) // najdu vsechny nulove sloupce a zapamatuji si jejich indexy
                {
                    if (Zero_column(N, the_temp_matrix, j))
                    {
                        Zero_Columns.Add(j);
                        //MessageBox.Show(Convert.ToString(j));
                    }
                }
                for (int i = 0; i < kernel_dimension && !stop_zero_vector_counting; i++) // nulove sloupce
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (Zero_Columns.Contains(j) && Zero_Columns[0] == j)
                        {
                            if (i != 1)
                                Result_eigenvectors[i, j] = 1;
                        }
                        else if (Zero_Columns.Contains(j) && i == 0)
                        {
                            Result_eigenvectors[i, j] = 1;
                        }
                        else if (last_zero_position + 1 == i && i < Zero_Columns.Count)
                        {
                            last_zero_position = i;
                            Result_eigenvectors[i, Zero_Columns[i]] = 0;
                            //MessageBox.Show(Convert.ToString(i));
                            //MessageBox.Show(Convert.ToString(Zero_Columns[i]));
                        }
                        if (Zero_Columns.Contains(j) && i < Zero_Columns.Count && j != Zero_Columns[i])
                        {
                            Result_eigenvectors[i, j] = 1;
                        }
                    }
                }

                if (Zero_Columns.Count == 2 && kernel_dimension >= 3 && !stop_zero_vector_counting) // castecny pripad 1001
                {
                    the_0_1 = 1;
                    for (int i = Zero_Columns.Count; i < kernel_dimension; i++)
                    {
                        Result_eigenvectors[i, Zero_Columns[1 - the_0_1]] = 0;
                        if (the_0_1 == 0)
                        {
                            the_0_1 = 1;
                        }
                        else
                        {
                            the_0_1 = 0;
                        }
                        Result_eigenvectors[i, Zero_Columns[1 - the_0_1]] = 1;
                    }
                }
                the_0_1 = 0;
                stop_zero_vector_counting = true;
                //MessageBox.Show(Convert_MX_VR_to_string(N - rank, N + 1, Result_eigenvectors) + "Test");

                //MessageBox.Show(Convert.ToString(row_Non_zero_counter) + "non zero's");

                if (row_Non_zero_counter == 1)
                {
                    for (int i = 0; i < kernel_dimension; i++) // indexy vl. vektoru
                    {
                        if (fixed_value[i] == 0)
                        {
                            Result_eigenvectors[i, last_non_zero_pos_on_the_row] = 0;
                        }
                        else
                        {
                            Result_eigenvectors[i, last_non_zero_pos_on_the_row] = fixed_value[i] / the_temp_matrix[k, last_non_zero_pos_on_the_row];
                        }
                    }
                    filled_positions_list.Add(last_non_zero_pos_on_the_row);
                }
                else if (row_Non_zero_counter == 2)
                {
                    if (!filled_positions_list.Contains(last_non_zero_pos_on_the_row)) // vypocitam posledni pozici na radku
                    {
                        for (int i = 0; i < kernel_dimension; i++) // indexy vl. vektoru
                        {
                            if (fixed_value[i] == 0)
                            {
                                
                                if (the_0_1 == 0 || all_around_counter < 3) // stridani hodnot
                                {
                                    the_0_1 = 1;
                                    all_around_counter++;
                                }
                                else
                                {
                                    the_0_1 = 0;
                                    all_around_counter = 0;
                                }
                                The_0_1s_List.Add(the_0_1);
                                Result_eigenvectors[i, last_non_zero_pos_on_the_row] = (-1) / the_temp_matrix[k, last_non_zero_pos_on_the_row] * the_0_1;
                                //MessageBox.Show(Convert.ToString(the_0_1) + " ,   Eig.:" + Convert.ToString(Eigenvalue) + ", The act.:" + Convert.ToString(Result_eigenvectors[i, last_non_zero_pos_on_the_row]));
                            }
                            else
                            {
                                Result_eigenvectors[i, last_non_zero_pos_on_the_row] = (-1) * ((fixed_value[i] + 1) / the_temp_matrix[k, last_non_zero_pos_on_the_row]); // ok, works
                            }
                            fixed_value[i] += (-1) * Result_eigenvectors[i, last_non_zero_pos_on_the_row] * the_temp_matrix[k, last_non_zero_pos_on_the_row];
                        }
                        filled_positions_list.Add(last_non_zero_pos_on_the_row);
                    }
                    else
                    {
                        already_counted = true; // posledni pozice na radku byla jiz spoctena
                    }


                    for (int j = 0; j < last_non_zero_pos_on_the_row && !stop; j++) // predchozi pozice, ktera urcite nebyla spoctena 
                    { // najit a spocitat
                        if (the_temp_matrix[k, j] != 0)
                        {
                            stop = true;
                            if (!already_counted) // v predchozim kroku jsem spocital druhou pozici, a koeficient u prvni je vzdy 1
                            {
                                for (int i = 0; i < kernel_dimension; i++) // indexy vl. vektoru
                                {
                                    //MessageBox.Show(Convert.ToString(fixed_value[i]) + " :The_Fixed_value");
                                    if (The_0_1s_List[i] == 0 && fixed_value[i] == 0)
                                    {
                                        Result_eigenvectors[i, j] = 0;
                                    }
                                    else
                                    {
                                        if (fixed_value[i] != 0)
                                        {
                                            Result_eigenvectors[i, j] = fixed_value[i] / the_temp_matrix[k, j]; // == 1
                                        }
                                        else
                                        {
                                            Result_eigenvectors[i, j] = the_temp_matrix[k, j] * the_0_1; // == 1
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < N - rank; i++) // indexy vl. vektoru
                                {
                                    Result_eigenvectors[i, j] = ((-1) * Result_eigenvectors[i, last_non_zero_pos_on_the_row] * the_temp_matrix[k, last_non_zero_pos_on_the_row]) / the_temp_matrix[k, j];
                                }
                            }
                            filled_positions_list.Add(j);
                        }
                    }
                    stop = false;
                }
                else if (row_Non_zero_counter > 2)
                {
                    //row_Non_zero_counter = all_positions_in_the_row.Count;
                    if (Non_zero_counters != null && Non_zero_counters.Contains(row_Non_zero_counter))
                    {
                        index++;
                    }
                    Non_zero_counters.Add(row_Non_zero_counter);
                    for (int kernel = 0; kernel < kernel_dimension; kernel++) // spocitam fixed value pro kazdy vlastni vektor
                    {
                        Temporary_eigvecs_matrix = Solution_finder(N, rank, the_temp_matrix, k, all_positions_in_the_row, fixed_value[kernel], index);
                    }
                    //MessageBox.Show(Convert_MX_VR_to_string(kernel_dimension, N + 1, Temporary_eigvecs_matrix) + " Temp Vecs");
                    //MessageBox.Show(Convert_MX_VR_to_string(kernel_dimension, N + 1, Result_eigenvectors) + " Result");
                    Result_eigenvectors = Sum_Two_Matrices_EigenVectors_Edition(N - rank, N + 1, Temporary_eigvecs_matrix, Result_eigenvectors);


                    foreach (var position in all_positions_in_the_row)
                    {
                        filled_positions_list.Add(position);
                    }
                    //Result_eigenvectors = Temporary_eigvecs_matrix;
                }
                all_positions_in_the_row = null;
            }

            for (int i = 0; i < kernel_dimension; i++) // indexy vl. vektoru
            {
                Result_eigenvectors[i, N] = Value_index;
            }

            return Result_eigenvectors;
        }


        // Find eigenvectors 2, complete
        public static Tuple<double[,], int[]> Find_All_EigenVectors(int N, double[,] the_matrix, Tuple<int, double[], int[]> Eigenvalues, int accuracy)
        {
            double[,] Result_eigenvectors = new double[0, 0];
            double[,] Temp_Result;
            double[,] Temp_eigenvectors;
            double[,] Temp_matrix = new double[N, N + 1];
            int[] Geomertic_multiplicity = new int[Eigenvalues.Item1];
            int Temp_matrix_rank = 0;
            //MessageBox.Show("Item Count: " + Convert.ToString(Eigenvalues.Item1));
            for (int i = 0; i < Eigenvalues.Item1; i++)
            {
                Temp_matrix = Subtract_from_diagonal_plus_RREF(Eigenvalues.Item2[i], N, the_matrix, 15);
                //MessageBox.Show(Convert_MX_VR_to_string(N, N + 1, Temp_matrix) + "TMP Matrix");
                Temp_matrix_rank = Matrix_Rank_Finder(N, N, Temp_matrix, 15);
                Geomertic_multiplicity[i] = N - Temp_matrix_rank;
                Temp_eigenvectors = new double[N - Temp_matrix_rank, N + 1];
                Temp_Result = null;
                Temp_Result = new double[Result_eigenvectors.GetLength(0), Result_eigenvectors.GetLength(1)];
                Temp_Result = copy_matrix(Result_eigenvectors.GetLength(0), Result_eigenvectors.GetLength(1), Result_eigenvectors);
                Result_eigenvectors = null;

                if (Temp_matrix_rank < N)
                {
                    Temp_eigenvectors = Matrix_solver_Eigenvector_finder(N, Temp_matrix, Eigenvalues.Item2[i], Temp_matrix_rank, i);
                }

                if (i == 0)
                {
                    Result_eigenvectors = new double[N - Temp_matrix_rank, N + 1];
                    Result_eigenvectors = copy_matrix(N - Temp_matrix_rank, N + 1, Temp_eigenvectors);
                    Result_eigenvectors = Matrix_Library.Round(N - Temp_matrix_rank, N + 1, Result_eigenvectors, accuracy);
                }
                else if (i != 0)
                {
                    Result_eigenvectors = new double[Temp_Result.GetLength(0) + N - Temp_matrix_rank, N + 1];
                    Result_eigenvectors = Merge_Matrices_Vertically(Temp_Result.GetLength(0), N + 1, Temp_Result, N - Temp_matrix_rank, Temp_eigenvectors).Item3;
                    Result_eigenvectors = Matrix_Library.Round(Temp_Result.GetLength(0) + N - Temp_matrix_rank, N + 1, Result_eigenvectors, accuracy);
                }
                //Result_eigenvectors = Temp_eigenvectors;
                //MessageBox.Show(Convert_MX_VR_to_string(N - Temp_matrix_rank, N + 1, Temp_eigenvectors) + "Vecs");
                Temp_eigenvectors = null;
            }
            //MessageBox.Show(Convert_MX_VR_to_string(Result_eigenvectors.GetLength(0), Result_eigenvectors.GetLength(1), Result_eigenvectors) + " :The result");
            return Tuple.Create(Result_eigenvectors, Geomertic_multiplicity);
        }


        //Spectral decomposition
        public static Tuple<Boolean, double[,], double[,], double[,]> Spectral_Decomposition(int N, double[,] Eigenvectors, Tuple<int, double[], int[]> Eigenvalues, int accuracy)
        {
            double[,] the_vect_matrix = new double[N, N];
            double[,] Lambda_Matrix = new double[N, N];
            double[,] Inversed_matrix = new double[N, N];
            Boolean Operation_successful = false;

            Lambda_Matrix = Lambda_All_Together(Eigenvalues);

            if (Eigenvectors.GetLength(0) == N)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        the_vect_matrix[j, i] = Round(Eigenvectors[i, j], accuracy);
                    }
                }
            }
            var The_Inversed_Tuple = Inverse(N, N, the_vect_matrix, accuracy);
            Inversed_matrix = The_Inversed_Tuple.Item2;
            Operation_successful = The_Inversed_Tuple.Item1;
            return Tuple.Create(Operation_successful, the_vect_matrix, Lambda_Matrix, Inversed_matrix);
        }


        //Spectral decomposition check
        public static Boolean Spectral_Decomposition_Check(int N, double[,] the_original, double[,] matrix_1, double[,] matrix_2, double[,] matrix_3)
        {
            Boolean equal_MXs = false;
            double[,] temp_matrix = new double[N, N];
            double[,] result_matrix = new double[N, N];
            temp_matrix = Matrix_Multiplication(N, N, matrix_1, N, N, matrix_2, 15);
            result_matrix = Matrix_Multiplication(N, N, temp_matrix, N, N, matrix_3, 5);
            //MessageBox.Show(Convert_MX_VR_to_string(N, N, result_matrix));
            //MessageBox.Show(Convert_MX_VR_to_string(N, N, the_original));
            if (Equal_Matrices(N, N, the_original, result_matrix))
            {
                equal_MXs = true;
            }
            return equal_MXs;
        }

        // Atr. * A
        public static double[,] Diagonalize_matrix(int rows, int columns, double[,] the_original_matrix, int accuracy)
        {
            double[,] Result = new double[columns, columns];
            double[,] Temporary_transposed = new double[columns, rows];
            Temporary_transposed = Transposition(rows, columns, the_original_matrix, accuracy);
            Result = Matrix_Multiplication(columns, rows, Temporary_transposed, rows, columns, the_original_matrix, accuracy);
            return Result;
        }


        // The order change
        public static Tuple<int, double[], int[]> Change_the_values_order(Tuple<int, double[], int[]> Eigenvalues)
        {
            double temp = 0;
            int temp1 = 0;
            Tuple<int, double[], int[]> Eigenvalues_copy = new Tuple<int, double[], int[]>(Eigenvalues.Item1, Eigenvalues.Item2, Eigenvalues.Item3);
            Boolean changed = true;
            while (changed)
            {
                changed = false;
                for (int i = 0; i < Eigenvalues_copy.Item1 - 1; i++)
                {
                    if (Eigenvalues_copy.Item2[i] < Eigenvalues_copy.Item2[i + 1])
                    {
                        changed = true;
                        temp = Eigenvalues_copy.Item2[i];
                        Eigenvalues_copy.Item2[i] = Eigenvalues_copy.Item2[i + 1];
                        Eigenvalues_copy.Item2[i + 1] = Eigenvalues_copy.Item2[i];
                        temp1 = Eigenvalues_copy.Item3[i];
                        Eigenvalues_copy.Item3[i] = Eigenvalues_copy.Item3[i + 1];
                        Eigenvalues_copy.Item3[i + 1] = Eigenvalues_copy.Item3[i];
                    }
                }
            }
            return Tuple.Create(Eigenvalues_copy.Item1, Eigenvalues_copy.Item2, Eigenvalues_copy.Item3);
        }


        // SVD - eigenvalues change
        private static double[] SVD_Singular_values(Tuple<int, double[], int[]> Sorted_Eigenvalues, int matrix_A_rank)
        {
            Tuple<int, double[], int[]> Eigenvalues_copy = new Tuple<int, double[], int[]>(Sorted_Eigenvalues.Item1, Sorted_Eigenvalues.Item2, Sorted_Eigenvalues.Item3);
            double[] the_result_singular_values = new double[matrix_A_rank];

            int j = 0;
            int multiplicity = Eigenvalues_copy.Item3[j];
            for (int i = 0; i < matrix_A_rank; i++)
            {
                the_result_singular_values[i] = Math.Sqrt(Eigenvalues_copy.Item2[j]) ;
                multiplicity--;
                if (multiplicity == 0 && j < Sorted_Eigenvalues.Item1 - 1)
                {
                    j++;
                    multiplicity = Eigenvalues_copy.Item3[j];
                }
            }
            
            return the_result_singular_values;
        }


        // S matrix, Sqrt. Eigenvalues
        private static double[,] SVD_Singular_Matrix(int the_rank, double[] singular_values)
        {
            double[,] the_result = new double[the_rank, the_rank];
            for (int i = 0; i < the_rank; i++)
            {
                for (int j = 0; j < the_rank; j++)
                {
                    if (i == j)
                    {
                        the_result[i, j] = singular_values[i];
                    }
                    else
                    {
                        the_result[i, j] = 0;
                    }
                }
            }
            return the_result;
        }


        // Coeficient and Inverse checker
        private static double[,] Create_Orthogonal_Spectral_decomposition(int N, double[,] the_matrix, double[,] the_inveresed_matrix)
        {
            double[] coeficient = new double[N];
            double[,] the_result = new double[N, N];
            double[,] the_result_tr = new double[N, N];
            double[,] checker = new double[N, N];
            double[,] identity_matrix = new double[N, N];
            identity_matrix = create_identity_matrix(N, N);
            //MessageBox.Show(Convert_MX_VR_to_string(N, N, the_matrix) + " : the original");
            //MessageBox.Show(Convert_MX_VR_to_string(N, N, the_inveresed_matrix) + " : the inversed");
            //MessageBox.Show(Convert_MX_VR_to_string(N, N, Matrix_Multiplication(N, N, the_matrix, N, N, the_inveresed_matrix, 15)) + " : Mult");
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    try
                    {
                        the_result[i, j] = Math.Sqrt(the_matrix[i, j] * the_inveresed_matrix[j, i]);
                    }
                    catch (Exception The_Exception)
                    {
                        the_result[i, j] = 0;
                        MessageBox.Show(The_Exception.Message);
                    }
                    if (the_matrix[i, j] < 0)
                    {
                        the_result[i, j] = (-1) * the_result[i, j];
                    }
                }
            }
            //MessageBox.Show(Convert_MX_VR_to_string(N, N, the_result) + " : the result");
            the_result_tr = Transposition(N, N, the_result, 15);
            checker = Matrix_Multiplication(N, N, the_result, N, N, the_result_tr, 4);
            //MessageBox.Show(Convert_MX_VR_to_string(N, N, checker) + " : checker");
            //MessageBox.Show(Convert_MX_VR_to_string(N, N, identity_matrix) + " : IDE");
            //double coeficient = the_matrix[0, 0] / the_inveresed_matrix[0, 0];
            if (Equal_Matrices(N, N, checker, identity_matrix))
            {
                //MessageBox.Show("WELL");
                return the_result;
            }
            else
            {
                return null;
            }
        }


        // The Xi matrix
        private static double[,] SVD_Create_Sigma_Matrix(int rows, int columns, int rank, double[,] the_S_matrix)
        {
            double[,] the_Xi_matrix = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (i < rank && j < rank)
                    {
                        the_Xi_matrix[i, j] = the_S_matrix[i, j];
                    }
                    else
                    {
                        the_Xi_matrix[i, j] = 0;
                    }
                }
            }
            return the_Xi_matrix;
        }


        // The V1 matrix
        private static double[,] SVD_Create_V1_matrix(int N, int rank, double[,] the_V_matrix)
        {
            double[,] the_V1_matrix = new double[N, rank];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < rank; j++)
                {
                    the_V1_matrix[i, j] = the_V_matrix[i, j];
                }
            }
            return the_V1_matrix;
        }


        // The U matrix
        private static double[,] SVD_The_U_matrix_creation(int rows, int rank, double[,] the_U1)
        {
            int M = rows;
            int U1_kernel = M - rank;
            double[] the_coeficient = new double[U1_kernel];
            double[,] Transposed_U1 = new double[rank, M];
            double[,] Bigger_Transposed_U1 = new double[M, M];
            double[,] Solution = new double[U1_kernel, M + 1];
            double[,] The_Solution = new double[U1_kernel, M];
            double[,] The_Solution_tr = new double[M, U1_kernel];
            double[,] temp_matrix_for_coef_finding = new double[U1_kernel, U1_kernel];
            Transposed_U1 = Transposition(rows, rank, the_U1, 15);
            double[,] the_U_matrix = new double[M, M];

            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if (i < rank)
                    {
                        Bigger_Transposed_U1[i, j] = Transposed_U1[i, j];
                    }
                    else
                    {
                        Bigger_Transposed_U1[i, j] = 0;
                    }
                }
            }
            Solution = Matrix_solver_Eigenvector_finder(M, Bigger_Transposed_U1, 0, rank, 0);

            for (int i = 0; i < U1_kernel; i++) // zkratim The_solution, protoze nepotrebuji posledny sloupec
            {
                for (int j = 0; j < M; j++)
                {
                    The_Solution[i, j] = Solution[i, j];
                }
            }
            The_Solution_tr = Transposition(U1_kernel, M, The_Solution, 15); // potrebuji to abych nasel koeficienty pro kazdy radek vysledku
            temp_matrix_for_coef_finding = Matrix_Multiplication(U1_kernel, M, The_Solution, M, U1_kernel, The_Solution_tr, 15);
            for (int i = 0; i < U1_kernel; i++)
            {
                the_coeficient[i] = Math.Sqrt(temp_matrix_for_coef_finding[i, i]) / temp_matrix_for_coef_finding[i, i]; // zajimava cisla musi byt na diagonale, vsechno ostatni musi byt 0
                for (int j = 0; j < M; j++)
                {
                    The_Solution[i, j] = The_Solution[i, j] * the_coeficient[i]; // upravim reseni do spravneho tvaru
                }
            }

            the_U_matrix = Merge_Matrices_Vertically(rank, M, Transposed_U1, U1_kernel, The_Solution).Item3;
            the_U_matrix = Transposition(M, M, the_U_matrix, 15);

            return the_U_matrix;
        }


        // SVD- everything together
        public static Tuple<Boolean, double[,], double[,], double[,]> SVD(int rows, int columns, double[,] the_matrix_A, int number_of_simulations, int accuracy, int number_of_comparisons)
        {
            int N = columns;
            double[,] AtransposedA = new double[N, N];
            double[,] the_V_matrix = new double[N, N];
            double[,] the_V_transposed = new double[N, N];
            double[,] the_Sigma_matrix = new double[rows, columns];

            int rank_A = 0;
            rank_A = Matrix_Rank_Finder(rows, columns, the_matrix_A, accuracy);
            Boolean spectral_decomposition_check = false;
            double[] Singular_values_vector = new double[rank_A];
            double[,] the_singular_matrix = new double[rank_A, rank_A];
            double[,] the_S_matrix_Inversed = new double[rank_A, rank_A];
            double[,] the_V1 = new double[N, rank_A];
            double[,] temp_mult_mx = new double[N, rank_A];
            double[,] The_U1_matrix = new double[rows, rank_A];
            double[,] The_U_matrix = new double[rows, rows];
            double[,] tmp = new double[rows, columns];
            double[,] tester = new double[rows, columns];

            AtransposedA = Diagonalize_matrix(rows, columns, the_matrix_A, 15);

            Tuple<int, double[], int[]> EigenValues = new Tuple<int, double[], int[]>(0, null, null);
            Tuple<double[,], int[]> EigenVectors = new Tuple<double[,], int[]>(null, null);
            Tuple<Boolean, double[,], double[,], double[,]> Spectral_matrices = new Tuple<Boolean, double[,], double[,], double[,]>(false, null, null, null);

            EigenValues = Matrix_Library.Find_Eigenvalues_Everything_Together(N, AtransposedA, number_of_simulations, 15, number_of_comparisons);
            EigenValues = Matrix_Library.Change_the_values_order(EigenValues);
            //MessageBox.Show(Convert_MX_VR_to_string(EigenValues.Item1, EigenValues.Item2));
            EigenVectors = Matrix_Library.Find_All_EigenVectors(N, AtransposedA, EigenValues, 15);

            Spectral_matrices = Matrix_Library.Spectral_Decomposition(N, EigenVectors.Item1, EigenValues, 15);
            if (Spectral_matrices.Item1)
            {
                spectral_decomposition_check = Matrix_Library.Spectral_Decomposition_Check(N, AtransposedA, Spectral_matrices.Item2, Spectral_matrices.Item3, Spectral_matrices.Item4);
            }

            if (spectral_decomposition_check)
            {
                try
                {
                    Singular_values_vector = SVD_Singular_values(EigenValues, rank_A);
                    the_singular_matrix = SVD_Singular_Matrix(rank_A, Singular_values_vector);
                    the_Sigma_matrix = SVD_Create_Sigma_Matrix(rows, columns, rank_A, the_singular_matrix);

                    the_V_matrix = Create_Orthogonal_Spectral_decomposition(N, Spectral_matrices.Item2, Spectral_matrices.Item4);
                    //MessageBox.Show(Convert_MX_VR_to_string(N, N, the_V_matrix));
                    the_V_transposed = Transposition(N, N, the_V_matrix, 15);

                    var S_Inversed = Inverse(rank_A, rank_A, the_singular_matrix, 15);
                    if (!S_Inversed.Item1)
                    {
                        spectral_decomposition_check = false;
                    }
                    the_S_matrix_Inversed = S_Inversed.Item2;
                    the_V1 = SVD_Create_V1_matrix(N, rank_A, the_V_matrix);
                    temp_mult_mx = Matrix_Multiplication(N, rank_A, the_V1, rank_A, rank_A, the_S_matrix_Inversed, 15);
                    The_U1_matrix = Matrix_Multiplication(rows, columns, the_matrix_A, N, rank_A, temp_mult_mx, 15);
                    The_U_matrix = SVD_The_U_matrix_creation(rows, rank_A, The_U1_matrix);
                    tmp = Matrix_Multiplication(rows, columns, the_Sigma_matrix, columns, columns, the_V_transposed, 15);
                    tester = Matrix_Multiplication(rows, rows, The_U_matrix, rows, columns, tmp, 4);
                    if (!Equal_Matrices(rows, columns, tester, the_matrix_A))
                    {
                        spectral_decomposition_check = false;
                    }
                }
                catch (Exception The_Exception)
                {
                    MessageBox.Show(Writings.SVD_Error + The_Exception.Message);
                }
            }
            //MessageBox.Show(Convert_MX_VR_to_string(rows, rows, The_U_matrix) + " : the U");
            //MessageBox.Show(Convert_MX_VR_to_string(rows, columns, the_Sigma_matrix) + " : Sigma");
            //MessageBox.Show(Convert_MX_VR_to_string(columns, columns, the_V_transposed) + " : Vtr.");
            The_U_matrix = Round(rows, rows, The_U_matrix, accuracy);
            the_Sigma_matrix = Round(rows, columns, the_Sigma_matrix, accuracy);
            the_V_transposed = Round(columns, columns, the_V_transposed, accuracy);
            return Tuple.Create(spectral_decomposition_check, The_U_matrix, the_Sigma_matrix, the_V_transposed);
        }


        // Cholesky
        public static Tuple<Boolean, Boolean, double[,], double[,]> Cholesky(int N, double[,] the_A_matrix, int accuracy)
        {
            double temporary_checker = 0;
            double temporary_sum = 0;
            Boolean positive_def = true;
            Boolean result_check = false;
            double[,] the_Lower_mx = new double[N, N];
            double[,] the_Upper_mx = new double[N, N];
            double[,] result_checker_mx = new double[N, N];

            for (int k = 0; k < N && positive_def; k++)
            {
                temporary_sum = 0;
                for (int j = 0; j <= k - 1; j++)
                {
                    temporary_sum += Math.Pow(the_Lower_mx[k, j], 2);  
                }

                temporary_checker = the_A_matrix[k, k] - temporary_sum;

                if (temporary_checker <= 0)
                {
                    positive_def = false;
                }
                else
                {
                    the_Lower_mx[k, k] = Math.Sqrt(temporary_checker);
                    
                    for (int i = k + 1; i < N; i++)
                    {
                        temporary_sum = 0;
                        for (int j = 0; j <= k - 1; j++)
                        {
                            temporary_sum += the_Lower_mx[i, j] * the_Lower_mx[k, j];
                        }
                        the_Lower_mx[i, k] = (1 / the_Lower_mx[k, k]) * (the_A_matrix[i, k] - temporary_sum);
                    }
                }
            }


            the_Upper_mx = Transposition(N, N, the_Lower_mx, 15);
            result_checker_mx = Matrix_Multiplication(N, N, the_Lower_mx, N, N, the_Upper_mx, accuracy);

            if (Equal_Matrices(N, N, the_A_matrix, result_checker_mx))
            {
                result_check = true;
            }
            the_Lower_mx = Round(N, N, the_Lower_mx, accuracy);
            the_Upper_mx = Round(N, N, the_Upper_mx, accuracy);

            return Tuple.Create(positive_def, result_check, the_Lower_mx, the_Upper_mx);
        }

    }
}
