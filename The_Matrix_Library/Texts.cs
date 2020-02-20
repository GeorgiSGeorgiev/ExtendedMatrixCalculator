using System;
using System.Windows.Forms;


namespace Extended_Matrix_Calculator
{
    public static class Writings
    {
        public static int Chosen_Language = 1;
        // 0 = Bulharstina
        // 1 = Cestina
        // 2 = Anglictina
        // 3 = Nemcina

        public static Boolean Result_not_accurate_shown = false;
        public static Boolean Irrational_numbers = false;


        //Choice screen and Form1
        public static string Unacceptable_Value = "Nebyla zadana validní hodnota.";
        public static string Same_Value = "Byla zadana stejná hodnota.";
        public static string Sqare_Matrix_Needed = "Vybraný mód funguje jenom pro čtvercové matice. Prosím, bud' změnte mód nebo klikněte na tlačítko Zpět a změnte rozměry matice.";
        public static string Mode_not_chosen = "Chyba 101: Nebyl vybrán mód.";

        public static string Number_of_rows = "Počet řádků: ";
        public static string Number_of_columns = "Počet sloupců: ";
        public static string Least_Squares_Method = "Metoda Nejmenších Čtverců";

        public static string Second_Matrix_Size = "Velikost Druhé Matice";
        public static string Samples_count = "Počet vzorků:";
        public static string Data_types = "Typy dat:";

        //Result screen and Buttons
        public static string Result_Title = "Výsledek";
        public static string Matrix = "Matice";
        public static string Vect = "Vekt.";
        public static string Inv = "inv.";
        public static string Tr = "tr.";
        public static string Coefficient = "Koeficinety: ";
        public static string Data_str = "Data";
        public static string Aproximation = "Aprox.";
        public static string First_EigenVal = "První vlastní číslo: ";
        public static string The_First_EigenVector = "První vlastní vektor: ";

        public static string Open_as_txt = "Otevřít txt.";
        public static string Save_all_as = "Uložit vše...";
        public static string Save_as = "Uložit...";
        public static string Save_as_Image = "Uložit jako obrázek";
        public static string Next = "Další";
        public static string Back = "Zpět";
        public static string Close = "Zavřít";
        public static string Fill_0s = "Vyplň 0-y";
        public static string Clear_all = "Smaž vše";

        public static string Null_matrix = "Chyba: Matice rovná 'Null'. ";
        public static string SVD_Error = "Singulární rozklad se nepovedl. Spektrální rozklad selhal. Chyba: ";

        public static string The_result_is = "VÝSLEDEK JE: ";

        public static string LSM_Error = "LSM chyba";
        public static string Spectral_decomposition_successful = "Spektrální rozklad se povedl. ";
        public static string Spectral_decomposition_unsuccessful = "Spektrální rozklad se nepovedl. Matice není diagonalizovatelná! ";
        public static string SVD_successful = "Stav: SVD se povedl. ";
        public static string SVD_unsuccessful = "Stav: SVD se nepovedl. ";
        public static string Cholesky_Mx_not_positive_definite = "Chyba: Matice není positivně definitní. ";
        public static string Cholesky_failed = "Choleského rozklad se nepovedl. ";
        public static string Cholesky_successful = "Choleského rozklad se povedl. ";

        public static string Error_In_Merging = "Chyba při slevání, počet sloupců se neshoduje.";

        public static string Could_not_draw_scale = "Nepovedlo se nakreslit stupnici. Chyba: ";
        public static string Could_not_draw_the_curve = "Nepovedlo se nakreslit křívku. Graf nebyl dokončen. Chyba: ";

        public static string Cutting_error = "Nepovedlo se odříznutí vyžadovaného kusu čísel od matice. Chyba:";
        public static string Inverse_not_successful = "Inverze nebyla uspěšná. Matice není regulární.";

        //Matrix screen
        public static string Matrix_ID = "ID matice: ";
        public static string Open_from_txt = "Otevřít z .txt";
        public static string Open_text_file = "Otevřít .txt soubor";
        public static string Could_not_read_file = "Nepovedlo se nahravání souboru. Chyba: ";
        public static string Dimensions_missmatch = "Chyba 102. Rozměry matic se neshodují. Prosím, změnte rozměry matice.";


        //Eigenvalues
        public static string Irrational_numbers_message = "Pozor! Některé z vlastních čísel může být iracionální, program nedokázal spočítat příslušné vlastní vektory.";
        public static string lambda_error = "Lambda Chyba";
        public static string Result_not_accurate = "Pozor: Výseldek může být nepřesný.";
        public static string Eigenvalues = "Vlastní čísla:";
        public static string Eigenvalue = "Vlastní číslo ";
        public static string Algebraic_multiplicity = "Algebraická násobnost: ";
        public static string Geometric_multiplicity = "Geometrická násobnost: ";
        public static string No_eigenvectors = "Nelze vypočítat vlastní vektory příslušné k danému vlastnímu číslu.";
        public static string Eigenvectors = "Vlastní vektry:";
        public static string Eigenvector = "Vlastní vektor";

        //Menu
        public static string Accuracy = "Přesnost:";
        public static string Swap_rows = "Výměna řádků";
        public static string Swap_columns = "Výměna sloupců";
        public static string Sum = "Sčítání";
        public static string Subtract = "Odečítání";
        public static string Multiply = "Násobení";
        public static string Transpose = "Transpozice";
        public static string REF = "REF";
        public static string RREF = "RREF";
        public static string Inverse = "Inverze";
        public static string Determinant = "Determinant";
        public static string Power_Iteration = "Mocninná metoda";
        public static string EigenValues_txt = "Vlastní čísla";
        public static string Cholesky = "Choleského rozklad";
        public static string Spectral_Decomposition = "Spektrální rozklad";
        public static string SVD = "SVD";
        public static string LSM = "LSM";

        //Form1
        public static string Save_the_Result = "Uložení výsledku";
        public static string Saving_Matrix = "Uložení matice ";



        public static void Show_Irrational_numbers_message()
        {
            if (!Irrational_numbers)
            {
                Irrational_numbers = true;
                MessageBox.Show(Irrational_numbers_message);
            }
        }


        public static void Show_Result_not_accurate_message()
        {
            if (!Result_not_accurate_shown)
            {
                Result_not_accurate_shown = true;
                MessageBox.Show(Result_not_accurate);
            }
        }


        public static void Show_lambda_error()
        {
            MessageBox.Show(lambda_error);
        }

        public static Boolean Show_Dimensions_missmatch_error(Boolean Message_shown)
        {
            if (!Message_shown)
            {
                MessageBox.Show(Dimensions_missmatch);
            }
            return true;
        }


        // Preklad do ruznych jazyku
        public static void Change_Language(int Language_index)
        {
            switch (Language_index)
            {
                case 0:
                    Switch_to_Bulgarian();
                    break;
                case 1:
                    Switch_to_Czech();
                    break;
                case 2:
                    Switch_to_English();
                    break;
                case 3:
                    Switch_to_German();
                    break;
            }
        }

        private static void Switch_to_Bulgarian()
        {
            //Choice screen and Form1
            Unacceptable_Value = "Зададента стойност не е валидна.";
            Same_Value = "Зададена е същата стойност.";
            Sqare_Matrix_Needed = "Избраният режим работи само с квадратни матрици. Моля, сменете режима или кликнете на бутона 'Назад' и сменете размерите на матрицата.";
            Mode_not_chosen = "Грешка 101: Не сте избрали режим.";

            Number_of_rows = "Брой редове: ";
            Number_of_columns = "Брой колони: ";
            Least_Squares_Method = "Метод на най-малките квадр.";

            Second_Matrix_Size = "Размери на втората матрица";
            Samples_count = "Брой проби:";
            Data_types = "Видове данни:";

            //Result screen and Buttons
            Result_Title = "Резултат";
            Matrix = "Матрица";
            Vect = "Вект.";
            Inv = "обр.";
            Tr = "тр.";
            Coefficient = "Коефициенти: ";
            Data_str = "Данни";
            Aproximation = "Апрокс.";
            First_EigenVal = "Първа собствена стойност: ";
            The_First_EigenVector = "Първи собствен вектор: ";

            Open_as_txt = "Отвори в txt.";
            Save_all_as = "Запази всичко";
            Save_as = "Запази...";
            Save_as_Image = "Запази като изображение";
            Next = "Напред";
            Back = "Назад";
            Close = "Затвори";
            Fill_0s = "Напиши 0-и";
            Clear_all = "Изтрий всичко";

            Null_matrix = "Грашка: Матрица е равна на стойността 'Null'. ";
            SVD_Error = "Декомпозицията по сингулярни стойности не е успешна. Спектралното разлагане не е проведено успешно. Грешка: ";

            The_result_is = "РЕЗУЛТАТЪТ Е: ";

            LSM_Error = "Грашка при Метода на най-малките квадрати";
            Spectral_decomposition_successful = "Спектралната декомпозиция е успешна.";
            Spectral_decomposition_unsuccessful = "Спектралната декомпозиция не е успешна. Матрицата не е диагонализируема! ";
            SVD_successful = "Състояние: SVD е успешна. ";
            SVD_unsuccessful = "Състояние: SVD не е успешна. ";
            Cholesky_Mx_not_positive_definite = "Грешка: Матрицата не е позитивно определяема. ";
            Cholesky_failed = "Разлагането на Холецки не е успешно. ";
            Cholesky_successful = "Разлагането на Холецки е успешно. ";

            Error_In_Merging = "Грешка при съединяването на матриците, броят на колоните не е еднакъв.";

            Could_not_draw_scale = "Чертането на скалата не е успешно. Грешка: ";
            Could_not_draw_the_curve = "Изчертаването на кривата не в успешно. Графиката не е довършена. Грешка: ";
            Cutting_error = "Изрязването на матрицата не е успешно. Грешка:";
            Inverse_not_successful = "Грешка. Матрицата не е обратима.";

            //Matrix screen
            Matrix_ID = "ID: ";
            Open_from_txt = "Прочети от .txt";
            Open_text_file = "Отвори в .txt файл";
            Could_not_read_file = "Прочитането на файла не е узпешно. Грешка: ";
            Dimensions_missmatch = "Грашка 102. Размерите на матриците не са сходни. Моля, сменете размерите на матрицата или самата матрица.";


            //Eigenvalues
            Irrational_numbers_message = "Внимание! Някоя от собствените стойности може да е ирзционално число, програмата не успя да изчисли съответните собсвени вектори.";
            lambda_error = "Ламбда грешка";
            Result_not_accurate = "Внимание: Резултатът може да е неточен.";
            Eigenvalues = "Собствени стойности:";
            Eigenvalue = "Собствена ст. ";
            Algebraic_multiplicity = "Алгебрична множ.: ";
            Geometric_multiplicity = "Геометрична множ.: ";
            No_eigenvectors = "Не могат да бъдат изчислени собсвените вектори.";
            Eigenvectors = "Собствени вектори:";
            Eigenvector = "Собствен вектор";

            //Menu
            Accuracy = "Точност:";
            Swap_rows = "Размяна на ред.";
            Swap_columns = "Размяна на колони";
            Sum = "Събиране";
            Subtract = "Изваждане";
            Multiply = "Умножение";
            Transpose = "Транспониране";
            REF = "REF";
            RREF = "RREF";
            Inverse = "Обратна матрица";
            Determinant = "Детерминанта";
            Power_Iteration = "Степенен метод";
            EigenValues_txt = "Собствени стойн.";
            Cholesky = "Разл. на Холецки";
            Spectral_Decomposition = "Спектрално разл.";
            SVD = "SVD";
            LSM = "LSM";

            //Form1
            Save_the_Result = "Записване на резултата ";
            Saving_Matrix = "Записване на матрицата ";
        }

        private static void Switch_to_Czech()
        {
            //Choice screen and Form1
            Unacceptable_Value = "Nebyla zadana validní hodnota.";
            Same_Value = "Byla zadana stejná hodnota.";
            Sqare_Matrix_Needed = "Vybraný mód funguje jenom pro čtvercové matice. Prosím, bud' změnte mód nebo klikněte na tlačítko 'Zpět' a změnte rozměry matice.";
            Mode_not_chosen = "Chyba 101: Nebyl vybrán mód.";

            Number_of_rows = "Počet řádků: ";
            Number_of_columns = "Počet sloupců: ";
            Least_Squares_Method = "Metoda Nejmenších Čtverců";

            Second_Matrix_Size = "Velikost Druhé Matice";
            Samples_count = "Počet vzorků:";
            Data_types = "Typy dat:";

            //Result screen and Buttons
            Result_Title = "Výsledek";
            Matrix = "Matice";
            Vect = "Vekt.";
            Inv = "inv.";
            Tr = "tr.";
            Coefficient = "Koeficienty: ";
            Data_str = "Data";
            Aproximation = "Aprox.";
            First_EigenVal = "První vlastní číslo: ";
            The_First_EigenVector = "První vlastní vektor: ";

            Open_as_txt = "Otevřít txt.";
            Save_all_as = "Uložit vše...";
            Save_as = "Uložit...";
            Save_as_Image = "Uložit jako obrázek";
            Next = "Další";
            Back = "Zpět";
            Close = "Zavřít";
            Fill_0s = "Vyplň 0-y";
            Clear_all = "Smaž vše";

            Null_matrix = "Chyba: Matice rovná 'Null'. ";
            SVD_Error = "Singulární rozklad se nepovedl. Spektrální rozklad selhal. Chyba: ";

            The_result_is = "VÝSLEDEK JE: ";

            LSM_Error = "LSM chyba";
            Spectral_decomposition_successful = "Spektrální rozklad se povedl. ";
            Spectral_decomposition_unsuccessful = "Spektrální rozklad se nepovedl. Matice není diagonalizovatelná! ";
            SVD_successful = "Stav: SVD se povedl. ";
            SVD_unsuccessful = "Stav: SVD se nepovedl. ";
            Cholesky_Mx_not_positive_definite = "Chyba: Matice není positivně definitní. ";
            Cholesky_failed = "Choleského rozklad se nepovedl. ";
            Cholesky_successful = "Choleského rozklad se povedl. ";

            Error_In_Merging = "Chyba při slevání, počet sloupců se neshoduje.";

            Could_not_draw_scale = "Nepovedlo se nakreslit stupnici. Chyba: ";
            Could_not_draw_the_curve = "Nepovedlo se nakreslit křívku. Graf nebyl dokončen. Chyba: ";
            Cutting_error = "Nepovedlo se odříznutí vyžadovaného kusu čísel od matice. Chyba:";
            Inverse_not_successful = "Inverze nebyla uspěšná. Matice není regulární.";

            //Matrix screen
            Matrix_ID = "ID matice: ";
            Open_from_txt = "Otevřít z .txt";
            Open_text_file = "Otevřít .txt soubor";
            Could_not_read_file = "Nepovedlo se nahravání souboru. Chyba: ";
            Dimensions_missmatch = "Chyba 102. Rozměry matic se neshodují. Prosím, změnte rozměry matice.";


            //Eigenvalues
            Irrational_numbers_message = "Pozor! Některé z vlastních čísel může být iracionální, program nedokázal spočítat příslušné vlastní vektory.";
            lambda_error = "Lambda Chyba";
            Result_not_accurate = "Pozor: Výseldek může být nepřesný.";
            Eigenvalues = "Vlastní čísla:";
            Eigenvalue = "Vlastní číslo ";
            Algebraic_multiplicity = "Algebraická násobnost: ";
            Geometric_multiplicity = "Geometrická násobnost: ";
            No_eigenvectors = "Nelze vypočítat vlastní vektory příslušné k danému vlastnímu číslu.";
            Eigenvectors = "Vlastní vektry:";
            Eigenvector = "Vlastní vektor";

            //Menu
            Accuracy = "Přesnost:";
            Swap_rows = "Výměna řádků";
            Swap_columns = "Výměna sloupců";
            Sum = "Sčítání";
            Subtract = "Odčítání";
            Multiply = "Násobení";
            Transpose = "Transpozice";
            REF = "REF";
            RREF = "RREF";
            Inverse = "Inverze";
            Determinant = "Determinant";
            Power_Iteration = "Mocninná metoda";
            EigenValues_txt = "Vlastní čísla";
            Cholesky = "Choleského rozklad";
            Spectral_Decomposition = "Spektrální rozklad";
            SVD = "SVD";
            LSM = "LSM";

            //Form1
            Save_the_Result = "Uložení výsledku";
            Saving_Matrix = "Uložení matice ";
        }

        private static void Switch_to_English()
        {
            //Choice screen and Form1
            Unacceptable_Value = "Entered value is not valid.";
            Same_Value = "Equal values were entered.";
            Sqare_Matrix_Needed = "The selected mode only accepts square matrices. Please, change mode or Click on the Back button and change matrix type.";
            Mode_not_chosen = "Error 101: Mode was not selected.";

            Number_of_rows = "Row count: ";
            Number_of_columns = "Column count:";
            Least_Squares_Method = "The Least Squares Method";

            Second_Matrix_Size = "Second Matrix Size";
            Samples_count = "Samples count:";
            Data_types = "Data types:";

            //Result screen and Buttons
            Result_Title = "The Result";
            Matrix = "Matrix";
            Vect = "Vect.";
            Inv = "inv.";
            Tr = "tr.";
            Coefficient = "Coefficients: ";
            Data_str = "Data";
            Aproximation = "Approx.";
            First_EigenVal = "The first eigenvalue: ";
            The_First_EigenVector = "The first eigenvector: ";

            Open_as_txt = "Open as txt.";
            Save_all_as = "Save all as ...";
            Save_as = "Save as...";
            Save_as_Image = "Save as image";
            Next = "Next";
            Back = "Back";
            Close = "Close";
            Fill_0s = "Fill 0s";
            Clear_all = "Clear all";

            Null_matrix = "Error: Null Matrix. ";
            SVD_Error = "The Singular Value Decomposition wasn't successful. Spectral decomposition failed. Error: ";

            The_result_is = "THE RESULT IS: ";

            LSM_Error = "LSM error";
            Spectral_decomposition_successful = "Spectral decomposition was successful. ";
            Spectral_decomposition_unsuccessful = "Spectral decomposition was not succssful. The matrix isn't diagonalizable! ";
            SVD_successful = "Status: SVD was successful. ";
            SVD_unsuccessful = "Status: SVD wasn't successful. ";
            Cholesky_Mx_not_positive_definite = "Error: The matrix isn't positive definite. ";
            Cholesky_failed = "Cholesky wasn't successful. ";
            Cholesky_successful = "Cholesky decomposition was successful. ";

            Error_In_Merging = "Error in merging, columns don't match.";

            Could_not_draw_scale = "Could not draw axes. Error: ";
            Could_not_draw_the_curve = "Could not draw the curve. Graph is incomplete. Error: ";
            Cutting_error = "Couldn't cut the wanted piece from the matrix. Error:";
            Inverse_not_successful = "Matrix inverse wasn't successful. The matrix isn't regular.";

            //Matrix screen
            Matrix_ID = "Matrix ID: ";
            Open_from_txt = "Open from a .txt";
            Open_text_file = "Open Text File";
            Could_not_read_file = "Could not read the file. Error: ";
            Dimensions_missmatch = "Error 102. Dimensions mismatch. Please, change matrix dimensions.";


            //Eigenvalues
            Irrational_numbers_message = "Warning! Eigenvalues may be irrational numbers, eigenvectors can not be computed.";
            lambda_error = "Lambda Error";
            Result_not_accurate = "Warning: The result may not be accurate";
            Eigenvalues = "Eigenvalues:";
            Eigenvalue = "Eigenvalue ";
            Algebraic_multiplicity = "Algebraic multiplicity: ";
            Geometric_multiplicity = "Geometric multiplicity: ";
            No_eigenvectors = "Could not compute eigenvectors for the given eigenvalue.";
            Eigenvectors = "Eigenvectors:";
            Eigenvector = "Eigenvector";

            //Menu
            Accuracy = "Accuracy:";
            Swap_rows = "Swap rows";
            Swap_columns = "Swap columns";
            Sum = "Sum";
            Subtract = "Subtract";
            Multiply = "Multiply";
            Transpose = "Transpose";
            REF = "REF";
            RREF = "RREF";
            Inverse = "Inverse";
            Determinant = "Determinant";
            Power_Iteration = "Power Iteration";
            EigenValues_txt = "Eigenvalues";
            Cholesky = "Cholesky";
            Spectral_Decomposition = "Spectral decompos.";
            SVD = "SVD";
            LSM = "LSM";

            //Form1
            Save_the_Result = "Save the Result";
            Saving_Matrix = "Saving matrix ";
        }

        private static void Switch_to_German()
        {
            //Choice screen and Form1
            Unacceptable_Value = "Der eingegebene Wert ist ungültig.";
            Same_Value = "Es wurden gleiche Werte eingegeben.";
            Sqare_Matrix_Needed = "Der ausgewählte Modus akzeptiert nur quadratische Matrizen. Bitte ändern Sie den Modus oder klicken Sie auf die Schaltfläche Zurück und ändern Sie den Matrixtyp.";
            Mode_not_chosen = "Fehler 101: Modus wurde nicht ausgewählt.";

            Number_of_rows = "Zeilenanzahl:";
            Number_of_columns = "Spaltenzahl:";
            Least_Squares_Method = "Methode der kleinsten Quadrate";

            Second_Matrix_Size = "Zweite Matrix";
            Samples_count = "Probenzahl:";
            Data_types = "Datentypen:";

            //Result screen and Buttons
            Result_Title = "Das Ergebnis";
            Matrix = "Matrix";
            Vect = "Vekt.";
            Inv = "inv.";
            Tr = "Tr.";
            Coefficient = "Koeffizienten: ";
            Data_str = "Daten";
            Aproximation = "Annäherung"; 
            First_EigenVal = "Der erste Eigenwert: ";
            The_First_EigenVector = "Der erste Eigenvektor: ";

            Open_as_txt = "Als.txt öffnen";
            Save_all_as = "Speichern alles";
            Save_as = "Speichern";
            Save_as_Image = "Als Bild speichern";
            Next = "Weiter";
            Back = "Zurück";
            Close = "Beenden";
            Fill_0s = "0-en ausfüllen";
            Clear_all = "Alles löschen";

            Null_matrix = "Fehler: 'Null' Matrix.";
            SVD_Error = "Die Singulärwertzerlegung war nicht erfolgreich. Die Spektralzerlegung ist fehlgeschlagen. Fehler: ";

            The_result_is = "DAS ERGEBNIS IST: ";

            LSM_Error = "MKQ Fehler";
            Spectral_decomposition_successful = "Die Spektralzerlegung war erfolgreich. ";
            Spectral_decomposition_unsuccessful = "Die Spektralzerlegung ist fehlgeschlagen. Die Matrix ist nicht diagonalisierbar! ";
            SVD_successful = "Status: SWZ war erfolgreich. ";
            SVD_unsuccessful = "Status: SWZ ist fehlgeschlagen. ";
            Cholesky_Mx_not_positive_definite = "Fehler: Die Matrix ist nicht positiv definit. ";
            Cholesky_failed = "Die Cholesky-Zerlegung ist fehlgeschlagen. ";
            Cholesky_successful = "Die Cholesky-Zerlegung war erfolgreich. ";

            Error_In_Merging = "Kann nicht Matrizen zusammenfügen, die Spaltenzahl ist nicht gleich.";

            Could_not_draw_scale = "Die Achsen konnten nicht gezeichnet werden. Fehler: ";
            Could_not_draw_the_curve = "Die Kurve konnte nicht gezeichnet werden. Das Diagramm ist unvollständig. Fehler: ";
            Cutting_error = "Das gewünschte Stück konnte nicht aus der Matrix geschnitten werden. Fehler:";
            Inverse_not_successful = "Konnte die Inverse nicht finden. Die Matrix ist nicht regulär.";

            //Matrix screen
            Matrix_ID = "Matrix ID: ";
            Open_from_txt = "Aus .txt öffnen";
            Open_text_file = "Die Textdatei öffnen";
            Could_not_read_file = "Die Datei konnte nicht gelesen werden. Fehler: ";
            Dimensions_missmatch = "Fehler 102. Matrixtyp stimmt nicht überein, bitte ändern Sie den Matrixtyp.";


            //Eigenvalues
            Irrational_numbers_message = "Achtung! Eigenwerte können irrationale Zahlen sein, Eigenvektoren können nicht berechnet werden.";
            lambda_error = "Lambda Fehler";
            Result_not_accurate = "Achtung: Das Ergebnis ist möglicherweise nicht genau";
            Eigenvalues = "Eigenwerte:";
            Eigenvalue = "Eigenwert ";
            Algebraic_multiplicity = "Algebraische Vielfachheit: ";
            Geometric_multiplicity = "Geometrische Vielfachheit: ";
            No_eigenvectors = "Es konnten keine Eigenvektoren für den angegebenen Eigenwert berechnet werden.";
            Eigenvectors = "Eigenvektoren:";
            Eigenvector = "Eigenvektor";

            //Menu
            Accuracy = "Genauigkeit:";
            Swap_rows = "Zeilen tauschen";
            Swap_columns = "Spalten tauschen";
            Sum = "Summieren";
            Subtract = "Subtrahieren";
            Multiply = "Multiplizieren";
            Transpose = "Transponieren";
            REF = "REF";
            RREF = "RREF";
            Inverse = "Inverse finden";
            Determinant = "Determinante";
            Power_Iteration = "Potenzmethode";
            EigenValues_txt = "Eigenwerte";
            Cholesky = "Cholesky-Zerlegung";
            Spectral_Decomposition = "Spektralzerlegung";
            SVD = "SWZ";
            LSM = "MKQ";

            //Form1
            Save_the_Result = "Das Ergebnis speichern";
            Saving_Matrix = "Die Matrix speichern ";
        }
    }
}
