using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu();
                       


        }
        public static void Menu()
        {
            int ans = 0;
            while (!(ans == 1 || ans == 2 || ans == 3))
            {
                Console.Clear();
                Console.WriteLine("Choices:");
                Console.WriteLine("[1] FIFO");
                Console.WriteLine("[2] LRU");
                Console.WriteLine("[3] LFU");
                Console.Write("Enter Choice:");
                string x = Console.ReadLine();
                if (int.TryParse(x, out ans) == false)
                    continue;
            }
            Console.Clear();
            switch (ans)
            {
                case 1:
                    FIFO();
                    break;
                case 2:
                    LRU();
                    break;
                case 3:
                    LFU();
                    break;
            }
        }

        #region LFU
        public static void LFU()
        {
            Console.Write("How many frames:");
            int frameCount = 2;
            frameCount = int.Parse(Console.ReadLine());
            Console.Write("Enter Input:");
            string inp = Console.ReadLine();

            string[] frame = new string[frameCount];
            Dictionary<string, int> frequency = new Dictionary<string, int>();
            List<List<string>> display = new List<List<string>>();
            List<string> interrupt = new List<string>();


            string[] input = inp.Split(' ');
            int curFrame = 0;
            
            for (int i = 0; i < frameCount; i++)
            {
                frame[i] = "-";
                display.Add(new List<string>());
            }
            string firstIn = null;
            foreach (string x in input)
            {
                firstIn = checkFrequency(frequency, x);
                if (frame[curFrame] == "-")
                {
                    if (isExisting(frame, x) == false)//if not existing
                    {
                        frame[curFrame] = x;
                        if (frame[(curFrame + 1) % frameCount] == "-")
                        {
                            curFrame++;
                        }
                        interrupt.Add("*");
                    }
                    else
                    {
                        interrupt.Add(" ");
                    }
                    
                }
                else
                {
                    if (isExisting(frame, x) == false)//not existing
                    {
                        if (firstIn != null)
                        {
                            //use curFrame
                            for (int ind = 0; ind < frameCount; ind++)
                            {
                                if (frame[ind] == firstIn)
                                {
                                    frequency[firstIn] = 0;
                                    frame[ind] = x;
                                    firstIn = null;
                                    interrupt.Add("*");
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //get least frequent
                            string resetKey = getLeastFrequent(frequency);
                            //reset the least frequent
                            frequency[resetKey] = 0;

                            //write the new letter
                            for (int ind = 0; ind < frameCount; ind++)
                            {
                                if (frame[ind] == resetKey)
                                {
                                    frame[ind] = x;
                                    interrupt.Add("*");
                                    break;
                                }
                            }
                        }

                    }
                }
                for (int z = 0; z < frameCount; z++)
                {
                    display[z].Add(frame[z]);
                }
                IncrementFrequency(frequency, x);
            }
            Console.WriteLine("\nSimulation:");
            foreach (List<string> x in display)
            {
                foreach (string y in x)
                {
                    Console.Write(y + " ");
                }
                Console.Write("\n");
            }
            foreach (string y in interrupt)
            {
                Console.Write(y + " ");
            }

            Console.ReadLine();
        }

        public static string checkFrequency(Dictionary<string, int> myDict, string key)
        {
            string ret = null;
            if (myDict.ContainsKey(key))
            {
                int val = myDict[key] + 1;
                foreach (KeyValuePair<string, int> x in myDict)
                {
                    if (x.Key == key)
                        continue;
                    else
                    {
                        if (x.Value == val)
                        {
                            ret = x.Key;
                            break;
                        }
                    }
                }
                
            }
            return ret;
        }
        public static string getLeastFrequent(Dictionary<string, int> myDict)
        {
            string key = "";
            int Min = 0;
            //get first
            foreach(KeyValuePair<string,int> x in myDict)
            {
                Min = x.Value;
                key = x.Key;
                break;
            }
            foreach (KeyValuePair<string, int> x in myDict)
            {
                if (x.Key == key)
                    continue;
                else
                {
                    if (x.Value < Min)
                    {
                        Min = x.Value;
                        key = x.Key;
                    }
                }
            }
            return key;
        }

        public static void IncrementFrequency(Dictionary<string, int> myDict, string key)
        {
            if (myDict.ContainsKey(key))
            {
                myDict[key]++;
            }
            else
            {
                myDict.Add(key, 0);
                myDict[key]++;
            }
        }
        #endregion


        #region LRU
        public static void LRU()
        {
            Console.Write("How many frames:");
            int frameCount = 2;
            frameCount = int.Parse(Console.ReadLine());
            Console.Write("Enter Input:");
            string inp = Console.ReadLine();

            string[] frame = new string[frameCount];
            string[] leastRecent = new string[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                leastRecent[i] = i.ToString();
            }
            string[] input = inp.Split(' ');

            int curFrame = 0;
            List<List<string>> display = new List<List<string>>();

            List<string> interrupt = new List<string>();
            for (int i = 0; i < frameCount; i++)
            {
                frame[i] = "-";
                display.Add(new List<string>());
            }
            foreach (string x in input)
            {
                if (frame[curFrame] == "-")
                {
                    if (isExisting(frame, x) == false)//if not existing
                    {
                        frame[curFrame] = x;
                        if (frame[(curFrame + 1)%frameCount] == "-")
                        {
                            UpdateLRU(leastRecent, curFrame.ToString());
                            curFrame++;
                        }
                        interrupt.Add("*");
                    }
                    else//if existing
                    {
                        for (int ind = 0; ind < frameCount; ind++)
                        {
                            if (frame[ind] == x)
                            {
                                UpdateLRU(leastRecent, ind.ToString());
                                break;
                            }
                        }
                        interrupt.Add(" ");
                    }
                }
                else
                {
                    if (isExisting(frame, x) == false)//not existing
                    {
                        curFrame = int.Parse(leastRecent[0]);
                        frame[curFrame] = x;
                        UpdateLRU(leastRecent, curFrame.ToString());
                        interrupt.Add("*");
                    }
                    else//if existing
                    {
                        for (int ind = 0; ind < frameCount; ind++)
                        {
                            if (frame[ind] == x)
                            {
                                UpdateLRU(leastRecent, ind.ToString());
                                interrupt.Add(" ");
                                break;
                            }
                        }
                    }
                }

                for (int z = 0; z < frameCount; z++)
                {
                    display[z].Add(frame[z]);
                }
            }
            Console.WriteLine("\nSimulation:");
            foreach (List<string> x in display)
            {
                foreach (string y in x)
                {
                    Console.Write(y + " ");
                }
                Console.Write("\n");
            }
            foreach (string y in interrupt)
            {
                Console.Write(y + " ");
            }
            Console.ReadLine();
        }
        public static void UpdateLRU(string[] leastRecent,string recentlyUsed)
        {
            for (int i = 0; i < leastRecent.Length; i++)
            {
                if (leastRecent[i] == recentlyUsed)
                {
                    string temp = leastRecent[i];
                    for (int x = i; x < leastRecent.Length-1; x++)
                    {
                        leastRecent[x] = leastRecent[x + 1];
                    }
                    leastRecent[leastRecent.Length-1] = temp;
                    break;
                }
            }
        }
        #endregion


        #region FIFO
        public static void FIFO()
        {
            Console.Write("How many frames:");

            int frameCount = 2;
            frameCount = int.Parse(Console.ReadLine());//rows
            Console.Write("Enter Input: ");
            string inp = Console.ReadLine();

            string[] frame = new string[frameCount];
            string[] input = inp.Split(' ');
            int curFrame = 0;

            List<List<string>> display = new List<List<string>>();

            List<string> interrupt = new List<string>();
            for (int i = 0; i < frameCount; i++)
            {
                frame[i] = "-";
                display.Add(new List<string>());
            }
            foreach (string x in input)
            {
                if (isExisting(frame, x) == false)
                {
                    frame[curFrame % frameCount] = x;
                    curFrame++;
                    //Console.WriteLine("Interrupted");
                    interrupt.Add("*");
                }
                else
                {
                    interrupt.Add(" ");
                }
                for (int z = 0; z < frameCount; z++)
                {
                    display[z].Add(frame[z]);
                }

            }

            //display
            Console.WriteLine("\nSimulation:");
            foreach (List<string> x in display)
            {
                foreach (string y in x)
                {
                    Console.Write(y + " ");
                }
                Console.Write("\n");
            }
            foreach (string y in interrupt)
            {
                Console.Write(y + " ");
            }


            Console.ReadLine();

        }
        public static bool isExisting(string[] frames, string find)
        {
            foreach (string x in frames)
            {
                if (x == find)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
