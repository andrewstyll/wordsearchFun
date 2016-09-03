//This file will tie everything together, and will call for the creation of both the Trie and the wordsearch. IT will
//then call for the solver to solve it
using System;
using puzzleMaker;
using dataStructure;

class wordsearchFun {
    static string[] dictionary = new string[] {"banditos","cat", "bat", "mutt", "dog", "grape"};

    public static void initPuzzle(Puzzle p, Trie t, string[] dic) {
        Array.Sort(dic, (x, y) => y.Length.CompareTo(x.Length));
        p.getWord(ref dic);
        p.fill();
        p.show();

        Array.Sort(dic, (x, y) => y.Length.CompareTo(x.Length));
        for(int i = 0; i < dictionary.Length; i++) {
            if(t.insertWord(dictionary[i])) {
                Console.WriteLine(dictionary[i]);
            }
        }
    }

    public static void Main(string[] args) {
    

        Puzzle wordsearch = new Puzzle(10, 10);
        Trie trie = new Trie();
        initPuzzle(wordsearch, trie, dictionary);
    }
}
