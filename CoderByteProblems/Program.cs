using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace CoderByteProblems
{
    class Program
    {
        static void Main(string[] args)
        {
            //SerialNumber Tests
            var test1 = "11.124.667";
            var test2 = "114.568.112";

            Console.WriteLine(SerialNumber(test1));
            Console.WriteLine(SerialNumber(test2));

            //LRU Cache Tests
            var test3 = new string[] { "A", "B", "C", "D", "A", "E", "D", "Z" };
            var test4 = new string[] { "A", "B", "A", "C", "A", "B" };
            var test5 = new string[] { "A", "B", "C", "D", "E", "D", "Q", "Z", "C" };

            Console.WriteLine(LRUCache(test3));
            Console.WriteLine(LRUCache(test4));
            Console.WriteLine(LRUCache(test5));

            //Step Walking Tests
            Console.WriteLine(StepWalking(3));
            Console.WriteLine(StepWalking(4));
            Console.WriteLine(StepWalking(5));
            Console.WriteLine(StepWalking(6));
            Console.WriteLine(StepWalking(7));
            Console.WriteLine(StepWalking(8));
            Console.WriteLine(StepWalking(9));
            Console.WriteLine(StepWalking(13));

        }

        public static string SerialNumber(string str)
        {

            var numbers = str.Split('.');

            //Check if there are 3 sets
            if (numbers.Length != 3)
                return "false";

            //Check if values are numerical
            if (!int.TryParse(numbers[0], out int firstNum))
                return "false";

            if (!int.TryParse(numbers[1], out int secondNum))
                return "false";

            if (!int.TryParse(numbers[2], out int thirdNum))
                return "false";

            //Check if each vales are of length 3
            if (numbers[0].Length != 3 || numbers[1].Length != 3 || numbers[2].Length != 3)
                return "false";

            //Check sum totals of sets
            if (GetSumOfDigits(firstNum) % 2 != 0)
                return "false";

            if (GetSumOfDigits(secondNum) % 2 != 1)
                return "false";

            //Check if third digit of third set is greater than first two digits
            var thirdSetDigits = GetListOfDigitsFromInt(thirdNum);

            if (thirdSetDigits[2] < thirdSetDigits[1] || thirdSetDigits[2] < thirdSetDigits[0])
                return "false";

            return "true";

        }

        public static int GetSumOfDigits(int input)
        {
            var sum = 0;
            while (input != 0)
            {
                sum += input % 10;
                input /= 10;
            }

            return sum;
        }

        public static List<int> GetListOfDigitsFromInt(int num)
        {
            List<int> listOfInts = new List<int>();

            while (num > 0)
            {
                listOfInts.Add(num % 10);
                num = num / 10;
            }
            listOfInts.Reverse();
            return listOfInts;
        }

        //You could also use a LinkedList for this
        public static string LRUCache(string[] strArr)
        {
            var cacheMax = 5;

            var cache = new List<string>();

            for (int i = 0; i < strArr.Length; i++)
            {
                var currentElement = strArr[i];

                var isFound = cache.Contains(currentElement);

                //Add item to cache if max hasn't been reached
                if (cache.Count < cacheMax && !isFound)
                {
                    cache.Add(currentElement);
                    continue;
                }

                //If already in the cache, bring to first position
                if (isFound)
                {
                    var elementCurrentIndex = cache.FindIndex(a => a == currentElement);
                    cache.MoveItemAtIndexToFront<string>(elementCurrentIndex);
                    continue;
                }

                //Cache already at max, so add item to end and remove 1st item in cache
                cache.Add(currentElement);
                cache.RemoveAt(0);
            }

            return string.Join("-", cache);
        }
        public static int StepWalking(int num)
        {            
            if (num < 0 || num > 15) 
                return -1;

            var stepIncrements = new List<int>();
            stepIncrements.Add(1); //Possible step increments
            stepIncrements.Add(2);

            //List of valid step combinations
            var stepCombinations = new List<List<int>>();

            //Breadth-first search
            var queue = new List<List<int>>();
            queue.Add(new List<int> { 1 }); //Arbitrary low starting values to use iterative approach
            queue.Add(new List<int> { 1, 1 });

            while (queue.Count != 0)
            {
                var nextQueue = new List<List<int>>();

                foreach(var stepList in queue)
                {
                    var sumOfSteps = stepList.Sum(x => Convert.ToInt32(x));

                    //Add step combination if step list adds up to number of stairs (num)
                    if (sumOfSteps == num)
                    {
                        stepCombinations.Add(stepList);
                        continue;
                    }

                    //Use both 1 and 2 step increments
                    foreach(var increment in stepIncrements)
                    {
                        if (sumOfSteps + increment <= num)
                        {
                            //If not exceeding total number of stairs, take further steps with new queue
                            var stepListCopy = GenericCopier<List<int>>.DeepCopy(stepList);
                            stepListCopy.Add(increment);
                            nextQueue.Add(stepListCopy);
                        }
                    }
                }

                queue = nextQueue;
            }

            return stepCombinations.Count;
        }
    }

}

public static class GenericCopier<T>
{
    public static T DeepCopy(object objectToCopy)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, objectToCopy);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return (T)binaryFormatter.Deserialize(memoryStream);
        }
    }
}
static class ListExtensions
{
    public static void MoveItemAtIndexToFront<T>(this List<T> list, int index)
    {
        T item = list[index];
        list.RemoveAt(index);
        list.Add(item);
    }
}