﻿// Excludes.cs
// This is a system that goes with the word filter system to exclude words, preventing them from triggering the word filter.

using System.Collections.Generic;


namespace FluffyEars.BadWords
{
    public static class Excludes
    {
        /// <summary>List of words to exclude.</summary>
        private static List<string> excludeList;
        public const string BaseFile = "wordexcludes";
        private static readonly object lockObj = (object)@"
               ((`\
            ___ \\ '--._
         .'`   `'    o  )
        /    \   '. __.'
       _|    /_  \ \_\_
jgs   {_\______\-'\__\_\";
        private static SaveFile saveFile = new SaveFile(BaseFile);

        public static void Default()
        {
            // Default values in this case is an empty list.
            excludeList = new List<string>();
            ReorganizeList();
            Save();
        }

        public static void Save()
        {
            saveFile.Save<List<string>>(excludeList, lockObj);
        }
        public static bool CanLoad() => saveFile.IsExistingSaveFile();
        public static void Load()
        {
            if (CanLoad())
            {
                excludeList = saveFile.Load<List<string>>(lockObj);
                ReorganizeList();
            }
            else Default();
        }

        private static void ReorganizeList()
        {
            excludeList.Sort();
            excludeList.Reverse();
        }

        /// <summary>Check if a word is in the exclude list.</summary>
        public static bool IsExcluded(string word) => excludeList.Contains(word.ToLower());
        public static bool IsExcluded(string msgOriginal, string badWord, int badWordIndex)
        {
            // The default return value is false because if there are no excluded words, then nothing can be excluded.
            bool returnVal = false;
            string msgLwr = msgOriginal.ToLower();

            if (excludeList.Count > 0) 
            {
                // Let's loop through every excluded word to check them against the list.
                foreach (string excludedWord in excludeList)
                {
                    if (returnVal)
                        break; // NON-SESE BREAK POINT! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! 

                    int excludedWordLength = excludedWord.Length;

                    int foundExcludeIndex = 0, scanIndex = 0; do
                    {
                        if (scanIndex <= msgOriginal.Length)
                            foundExcludeIndex = msgLwr.IndexOf(excludedWord, scanIndex);
                        else
                            foundExcludeIndex = -1;

                        if (foundExcludeIndex != -1)
                        {
                            // A && B && C && D
                            // (A) the bad word starts at or after the found excluded word.
                            // (B) the bad word ends at or before the found excluded word ends.
                            // (C) exception protect: let's make sure the substring we want to get next is within bounds of the message.
                            // (D) the found excluded word contains the bad word.
                            returnVal = badWordIndex >= foundExcludeIndex &&
                                        badWordIndex + badWord.Length <= foundExcludeIndex + excludedWordLength &&
                                        foundExcludeIndex + excludedWordLength <= msgOriginal.Length &&
                                        msgLwr.Substring(foundExcludeIndex, excludedWordLength).IndexOf(excludedWord) != -1;

                            if(!returnVal)
                                scanIndex += foundExcludeIndex + excludedWordLength;
                        }

                    } while (foundExcludeIndex != -1 && !returnVal);

                }
            }

            return returnVal;
        }

        /// <summary>
        /// Returns a list of the indexes of all occurrences of a substring in this string.
        /// </summary>
        /// <param name="word"></param>
        private static List<int> AllIndexesOf(this string str, string word, int startIndex)
        {
            List<int> returnVal = new List<int>();

            int i = 0; do
            {
                i = str.IndexOf(word, i);
                
                if (i != -1)
                {
                    returnVal.Add(i);
                    // Every time we find a word, we increment by its length so we can skip that word.
                    i += word.Length;
                }
            } while (i != -1);

            return returnVal;
        }

        public static void AddWord(string word)
        {
            excludeList.Add(word.ToLower());
            ReorganizeList();
        }
        public static void RemoveWord(string word)
        {
            excludeList.Remove(word.ToLower());
            ReorganizeList();
        }
            public static List<string> GetWords() => excludeList;
        public static int GetWordCount() => excludeList.Count;
    }
}
