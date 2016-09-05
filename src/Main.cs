//This file will tie everything together, and will call for the creation of both the Trie and the wordsearch. IT will
//then call for the solver to solve it
using System;
using puzzleMaker;
using dataStructure;

class wordsearchFun {

    static string[] dictionary = new string[] {"banditos","cat", "bat", "mutt", "dog", "grape"};
    static char?[,] result;
    static int height = 10;
    static int width = 10;
    
    //for each word in the wordsearch, I need to search all eight directions around it. The following instructions are
    //what will happen each time we start looking at a character in the grid.
    //1. check to see if the character (string for searching) is a match in the trie.
    //2. if it doesn't move on. If it does, then we can look around the letter in all 8 directions. if we find a match
    //  in a direction, continue looking in that direction by compounding the new letters we come across.
    //  we will either get a failure (so it won't exist in the Trie) or we will get an endword OR the word will be part
    //  of a larger word. In this third case we have to remember the word SPECIFY MORE HERE

    static void adjustIndex(ref int y, ref int x, string ydir, string xdir) {
        if (ydir == "++") {
            y++;
        } else if (ydir == "--") {
            y--;
        }
        
        if (xdir == "++") {
            x++;
        } else if (xdir == "--") {
            x--;
        }
    }

    static void checkWords(string substring, Trie t, int y, int x, string operation) {
        string [] s = operation.Split('#');
        substring += result[y, x];
        if(t.findWord(substring)) {
            //this is a word in the wordsearch
            Console.WriteLine("FOUND WORD: " + substring + " at y: " + y + " x: "+ x);
            return;
        }
        if(t.stringExists(substring)) {
            //the substring exists and it wasn't a full word. Continue in that direction
            adjustIndex(ref y, ref x, s[0], s[1]);
            if( y >= height || x >= width || x < 0 || y < 0) {
                //no more room to check, pack it up this direction's no good
                return;
            }
            checkWords(substring, t, y, x, operation);
        } else {
            return;
        }
    }

    static void iteratePuzzle(Trie t) {
        for(int i = 0; i < height; i++) {
            for(int j = 0; j < width; j++) {
                //check in each direction "yy#xx"
                checkWords("", t, i, j, "--#null"); //N
                checkWords("", t, i, j, "null#--"); //W
                checkWords("", t, i, j, "null#++"); //E
                checkWords("", t, i, j, "++#null"); //S
                checkWords("", t, i, j, "--#++"); //NE
                checkWords("", t, i, j, "++#++"); //SE
                checkWords("", t, i, j, "++#--"); //SW
                checkWords("", t, i, j, "--#--"); //NW
            }
        }
    }

    static void initPuzzle(Puzzle p, Trie t, string[] dic) {
        Array.Sort(dic, (x, y) => y.Length.CompareTo(x.Length));
        p.getWord(ref dic);
        result = p.fill();
        p.show();

        Array.Sort(dic, (x, y) => y.Length.CompareTo(x.Length));
        for(int i = 0; i < dictionary.Length; i++) {
            if(t.insertWord(dictionary[i])) {
                Console.WriteLine(dictionary[i]);
            }
        }
    }

    public static void Main(string[] args) {

        Puzzle wordsearch = new Puzzle(width, height);
        Trie trie = new Trie();
        initPuzzle(wordsearch, trie, dictionary);
        iteratePuzzle(trie);
    }
}
