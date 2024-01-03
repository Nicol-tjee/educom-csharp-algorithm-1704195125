﻿using System;
using System.Collections.Generic;

namespace Organizer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ShiftHighestSort shiftSorter = new ShiftHighestSort();
            RotatePivotSort rotateSorter = new RotatePivotSort();
            List <int> randomNumbers = MakeList(10);

            ShowList("Unsorted List", randomNumbers);

            // ShiftHigestSort
            List<int> shiftSortedNumbers = shiftSorter.ShiftSort(randomNumbers);
            ShowList("Sorted List ShiftHighestSort", shiftSortedNumbers);
            bool isShiftSorted = ValidateSortedArray(shiftSortedNumbers);
            Console.WriteLine($"\nIs the list correctly sorted? {isShiftSorted}");    
            
            // RotateSort
            List<int> rotateSortedNumbers = rotateSorter.RotateSort(randomNumbers);
            ShowList("Sorted List RotateSort", rotateSortedNumbers);
            bool isRotateSorted = ValidateSortedArray(rotateSortedNumbers);
            Console.WriteLine($"\nIs the list correctly sorted? {isRotateSorted}");
        }

        public static void ShowList(string label, List<int> theList)
        {
            int count = theList.Count;
            if (count > 100)
            {
                count = 300; // Do not show more than 300 numbers
            }
            Console.WriteLine();
            Console.Write(label);
            Console.Write(':');
            for (int index = 0; index < count; index++)
            {
                if (index % 20 == 0) // when index can be divided by 20 exactly, start a new line
                {
                    Console.WriteLine();
                }
                Console.Write(string.Format("{0,3}, ", theList[index]));  // Show each number right aligned within 3 characters, with a comma and a space
            }
            Console.WriteLine();
        }

        static List<int> MakeList(int N)
        {
           List<int> numbers = new List<int>();
            var random = new Random ();
            for (int i = 0; i < N; i++) 
            {
                numbers.Add(random.Next(-99, 99));
            }
            return numbers;
        }

        static bool ValidateSortedArray(List<int> array)
        {
            for (int i = 1; i < array.Count; i++)
            {
                if (array[i] < array[i - 1])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
