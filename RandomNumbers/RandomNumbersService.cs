using Newtonsoft.Json;
using RestSharp;
using System.Configuration;

namespace RandomNumbers
{
    public class RandomNumbersService : IRandomNumbersService
    {
        private delegate int[] Sort(int[] array);

        private readonly Dictionary<int, Sort> _methods = new Dictionary<int, Sort>() { { 1, new Sort(HeapSort) }, { 2, new Sort(ShellSort) } };

        // For the test address of the postman test server is used
        private readonly string _url = ConfigurationManager.AppSettings["URL"].ToString();

        public void Run()
        {
            int[] array = GenerateNumbers();
            int[] sortArray = DoSort(array);
            SendArray(sortArray);
        }

        /// <summary>
        /// The method generates an array of random numbers ranging from -100 to 100 and prints it to console. The length of the array is random, from 20 to 100
        /// </summary>
        /// <returns></returns>
        private int[] GenerateNumbers()
        {
            Random rnd = new Random();
            int value = rnd.Next(20, 101);
            int[] numbers = new int[value];

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = rnd.Next(-100, 101);
                Console.Write(numbers[i] + " ");
            }

            return numbers;
        }

        /// <summary>
        /// The method chooses a random sorting method, sorts the array and prints it to console
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private int[] DoSort(int[] array)
        {
            Random rnd = new Random();
            int value = rnd.Next(1, 3);
            int[] aaa = _methods[value](array);

            Console.WriteLine();
            foreach (int a in aaa)
            {
                Console.Write(a + " ");
            }

            return aaa;
        }

        /// <summary>
        /// Shell sort. An improved version of insertion sort. Comparison of elements that are not only next to each other, but also at a certain distance from each other
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private static int[] ShellSort(int[] array)
        {
            int j;
            // Step in half the length of the array
            int step = array.Length / 2;

            //Permutation of elements
            while (step > 0)
            {
                for (int i = 0; i < (array.Length - step); i++)
                {
                    j = i;
                    while ((j >= 0) && (array[j] > array[j + step]))
                    {
                        int tmp = array[j];
                        array[j] = array[j + step];
                        array[j + step] = tmp;
                        j -= step;
                    }
                }

                step = step / 2;
            }

            return array;
        }

        /// <summary>
        /// The method performs a comparison sort based on a binary heap
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private static int[] HeapSort(int[] array)
        {
            int n = array.Length;

            // Building a heap (rearrange the array)
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Heapify(array, n, i);
            }

            // Retrieving elements from the heap one by one
            for (int i = n - 1; i >= 0; i--)
            {
                // Move the current root to the end
                int tmp = array[0];
                array[0] = array[i];
                array[i] = tmp;

                // Call procedure Heapify on the reduced heap
                Heapify(array, i, 0);
            }

            return array;
        }

        /// <summary>
        /// Procedure for converting to a binary heap a subtree with root node i; i - index in array[]; n - heap size
        /// </summary>
        /// <param name="array"></param>
        /// <param name="n"></param>
        /// <param name="i"></param>
        private static void Heapify(int[] array, int n, int i)
        {
            // Initialize the largest element as root
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            // If the left child is greater than root
            if (left < n && array[left] > array[largest])
            {
                largest = left;
            }

            // If the right child is larger than the largest element currently
            if (right < n && array[right] > array[largest])
            {
                largest = right;
            }

            // If the largest element is not root
            if (largest != i)
            {
                int tmp = array[i];
                array[i] = array[largest];
                array[largest] = tmp;

                // Recursively convert the affected subtree into a binary heap
                Heapify(array, n, largest);
            }
        }

        /// <summary>
        /// The method sends an array in JSON format to api at address from configuration file using RestSharp
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private ResponseStatus SendArray(int[] array)
        {
            var client = new RestClient();
            var request = new RestRequest(_url, Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(array), ParameterType.RequestBody);
            var response = client.Execute(request);
            return response.ResponseStatus;
        }

    }
}
