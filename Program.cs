using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Micro_Nerdle
{
    

    public class Program
    {
        
        
        enum Index
        {
            D1,
            OP,
            D2,
            Equals,
            Result
        }

        static void Main(string[] args)
        {
            // Globals

            List<Pair> history = new List<Pair>();
            history.Add(new Pair("1+2=3", "RBBGB"));
            history.Add(new Pair("7*0=0", "GBBGB"));
            history.Add(new Pair("7-3=4", "GGRGR"));
            
            //history.Add(new Pair("7-4=3", "GGGGG"));


            string my_guess = "     ";

            ArrayList numbers = new ArrayList() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            ArrayList operators = new ArrayList() { '+', '*', '-', '/' };

            ArrayList[] WrongDigitsOfIndex = new ArrayList[5];
            for(int i = 0 ; i < 5; i++)
            {
                WrongDigitsOfIndex[i] = new ArrayList();
            }

            // Analyze History

            foreach (Pair pair in history)
            {
                string guess = pair.Guess;
                string feedback = pair.Feedback;


                // read feedback
                for (int i = 0; i < 5; i++)
                {
                    int idx_to_remove = -1;

                    switch (feedback[i])
                    {

                        case 'G':
                            if (i == (int) Index.OP)
                            {
                                // Remove Last
                                idx_to_remove = operators.LastIndexOf((char)guess[i]);
                                if(idx_to_remove != -1) operators.RemoveAt(idx_to_remove);


                                // Add First
                                operators.Insert(0, (char)guess[i]);
                            }
                            else
                            {
                                // Remove Last
                                idx_to_remove = numbers.LastIndexOf((int)guess[i]);
                                if (idx_to_remove != -1) numbers.RemoveAt(idx_to_remove);

                                // Add First
                                numbers.Insert(0, (int)guess[i]);
                            }
                            break;

                        case 'R':
                            // Remove Last
                            idx_to_remove = numbers.LastIndexOf((int)guess[i]);
                            if (idx_to_remove != -1) numbers.RemoveAt(idx_to_remove);

                            // Add First
                            numbers.Insert(0, (int)guess[i]);

                            // Add to wrong positions
                            WrongDigitsOfIndex[i].Add((int)guess[i]);

                            break;

                        case 'B':
                            if (i == (int)Index.OP)
                            {
                                // Remove Last
                                idx_to_remove = operators.LastIndexOf((char)guess[i]);
                                if (idx_to_remove != -1) operators.RemoveAt(idx_to_remove);
                               
                            }
                            else
                            {
                                // Remove Last
                                idx_to_remove = numbers.LastIndexOf((int)guess[i]);
                                if (idx_to_remove != -1) numbers.RemoveAt(idx_to_remove);
                            }
                            break;

                    }
                }
            }


            // Generate an expression    // Worst case => O(4,000)

            bool broken_1 = false;
            bool broken_2 = false;
            bool broken_3 = false;
            bool broken_4 = false;


            for (int i = 0; i < operators.Count; i++)    // operator
            {
                char op = (char)operators[i];

                for(int j = 0; j < numbers.Count; j++)  // d1
                {
                    int d1 = (int)numbers[j];

                    for( int k = 0; k < numbers.Count; k++) // d2
                    {
                        int d2 = (int)numbers[k];

                        for(int m = 0; m < numbers.Count; m++)  // result
                        {
                            int r = (int)numbers[m];
                            // make a guess

                            string temp_guess = $"{d1}{op}{d2}={r}";


                            // Validate Calculation
                            bool valid_calc = Validate_Calculation(d1, d2, r, op);

                            // Validate Positions
                            bool valid_positions = Validate_Positions(WrongDigitsOfIndex, d1, d2, r);

                            // Validate Single Digits
                            bool Valid_single_digits = Validate_Single_Digits(d1, d2, r);

                            if (valid_calc && valid_positions && Valid_single_digits)
                            {
                                Console.WriteLine($"The new guess is {temp_guess}");
                                broken_1 = true;
                                break;
                            }
                            else continue;
                        }
                        if(broken_1 == true)
                        {
                            broken_2 = true;
                            break;
                        }
                    }
                    if (broken_2 == true)
                    {
                        broken_3 = true;
                        break;
                    }
                }
                if (broken_3 == true)
                {
                    broken_1 = true;
                    break;
                }

            }









            Console.ReadKey();


        }
        static bool Validate_Calculation(int d1, int d2, int r, char op)
        {
            int expected_result = Calculate(d1, d2, op);
            if(expected_result == r)
            {
                return true;
            }
            return false;
        }
        static bool Validate_Positions(ArrayList[] WrongDigitsOfIndex, int d1, int d2, int r)
        {
            bool wrong_d1 = WrongDigitsOfIndex[(int)Index.D1].Contains(d1);
            bool wrong_d2 = WrongDigitsOfIndex[(int)Index.D2].Contains(d2);
            bool wrong_r = WrongDigitsOfIndex[(int)Index.Result].Contains(r);

            if(wrong_d1 || wrong_d2 || wrong_r)
            {
                return false;
            }
            return true;
        }
        static int Calculate(int d1, int d2, char op)
        {
            int result = -1;

            try
            {
                switch (op)
                {
                    case '+':
                        result = d1 + d2;
                        break;
                    case '*':
                        result = d1 * d2;
                        break;
                    case '-':
                        result = d1 - d2;
                        break;
                    case '/':
                        result = d1 / d2;
                        break;
                }
            }
            catch { Console.WriteLine("Invalid Calculation"); return result; }
            return result;
        }
        static bool Validate_Single_Digits(int d1, int d2, int r)
        {
            return (d1 >= 0 && d1 <= 9 && d2 >= 0 && d2 <= 9 && r >= 0 && r <= 9);
        }
        
       


    }
}
