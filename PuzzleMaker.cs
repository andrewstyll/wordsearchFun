using System;

namespace puzzleMaker {

    public static class Globals {
    
        public const int HEIGHT = 6;
        public const int WIDTH = 6;
        public const int DIR_EN = 8;
        public const int MAX_TRY = 500;
    }


    class Puzzle {
 
        private int height;
        private int width;
        private char?[,] result;
        private string[] resultParsed;

        static Random rnd;
        
        const int DIR_EN = Globals.DIR_EN;
        const int MAX_TRY = Globals.MAX_TRY;

        enum Directions {
                N = 0,
                E = 1, 
                S = 2,
                W = 3,
                NE = 4,
                SE = 5,
                SW = 6,
                NW = 7
            };

        public Puzzle(int w, int h) {
            rnd = new Random();
            height = h;
            width = w;
            result = new char?[height, width];
            resultParsed = new string[height];
        }

        void adjustIndex(ref int y, ref int x, string ydir, string xdir) {
            if (ydir == "--") {
                y--;
            } else if (ydir == "++") {
                y++;
            }

            if (xdir == "--") {
                x--;
            } else if (xdir == "++") {
                x++;
            }
        }

        bool insertWord(int x, int y, string word, string operation, int d1, int d2, int d3 = 1, int d4 = 0) {

            int yInd = y;
            int xInd = x;
            int wLength = word.Length;

            char?[] tmp = new char?[wLength];
            string [] s = operation.Split('#');
            
            if ( d1 > d2 && d3 > d4 ) {
                for(int i = 0; i < wLength; i++) {
                        tmp[i] = result[yInd, xInd];
                    if (tmp[i] == word[i] || !tmp[i].HasValue ) {
                        tmp[i] = word[i];
                    } else { // word didn't fit
                        return false;
                    }
                    adjustIndex(ref yInd, ref xInd, s[0], s[1]);
                }
                yInd = y;
                xInd = x;
                for(int i = 0; i < wLength; i++) {
                    //passed all tests, so copy word into master array
                    result[yInd, xInd] = word[i];
                    adjustIndex(ref yInd, ref xInd, s[0], s[1]);
                }
                return true;
            } else {
                return false;
            }
        }

        bool checkWord(int x, int y, string word, string dir) {
            
            int dx = 0;
            int dy = 0;
            int wLength = word.Length;
            
            //format is ydir#xdir
            if (dir == "N") { 
                string op = "--#null";
                dy = y-wLength+1;
                return insertWord(x, y, word, op, dy, -1);

            } else if (dir ==  "W") {
                string op = "null#--";
                dx = x - wLength + 1; 
                return insertWord(x, y, word, op, dx, -1);
            
            } else if (dir == "E") {
                string op = "null#++";
                dx = x + wLength - 1; 
                return insertWord(x, y, word, op, -1*dx, -1*this.width);
            
            } else if (dir == "S") {
                string op = "++#null";
                dy = y + wLength - 1; 
                return insertWord(x, y, word, op, -1*dy, -1*this.height);
            
            } else if (dir == "NE") {
                string op = "--#++";
                dy = y - wLength + 1;
                dx = x + wLength - 1; 
                return insertWord(x, y, word, op, dy, -1, -1*dx, -1*this.width);

            } else if (dir == "SE") {
                string op = "++#++";
                dy = y + wLength - 1;
                dx = x + wLength - 1; //0+3-1 = 2
                return insertWord(x, y, word, op, -1*dy, -1*this.height, -1*dx, -1*this.width);
                
            } else if (dir == "SW") {
                string op = "++#--";
                dy = y + wLength - 1;
                dx = x - wLength + 1; //0+3-1 = 2
                return insertWord(x, y, word, op, -1*dy, -1*this.height, dx, -1);
            } else {//NW 
                string op = "--#--";
                dy = y - wLength + 1;
                dx = x - wLength + 1; //0+3-1 = 2
                return insertWord(x, y, word, op, dy, -1, dx, -1);
            
            }
        }

        bool placeWords(string word, int w, int h, ref bool[, ,] vTracker) {

            int vTrackerConHits = 0;

            //potentially stuckin here forever
            for (int i = 0; i < MAX_TRY; i++) {
                int x = rnd.Next(0, w);
                int y = rnd.Next(0, h);
                int dir = rnd.Next(DIR_EN);

                if(vTracker[y, x, dir] == true && vTrackerConHits < 3) {
                    i--;
                    vTrackerConHits++;
                    continue;
                } else {
                    vTracker[y, x, dir] = true;
                }

                if (checkWord(x, y, word, Enum.GetName(typeof(Directions), dir)) == true) {
                    return true;
                }
            }
            return false;
        }

        bool forceWord(string word, bool[, ,] vTracker) {
            for(int i = 0; i < this.height; i++) { //for every y
                for(int j = 0; j < this.width; j++) { //for every x
                    for(int k = 0; k < DIR_EN; k++) { //for every direction
                        if (vTracker[i, j, k] != true) {
                            if (checkWord(j, i, word, Enum.GetName(typeof(Directions), k)) == true) {
                                return true; 
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void getWord(ref string[] words) {

            for(int i = 0; i < words.Length; i++) {
                bool[, ,] vTracker = new bool[this.height, this.width, DIR_EN];
                if (!placeWords(words[i], this.width, this.height, ref vTracker)) {
                    if (!forceWord(words[i], vTracker)) {
                        //the wordsearch has failed
                        words[i] = " ";
                        Console.WriteLine("failed word placement");
                    }
                }
            }
        }

        public char?[,] fill() {

            for(int i = 0; i < this.height; i++) {
                for(int j = 0; j < this.width; j++) {
                    if (!result[i, j].HasValue ) {
                        result[i, j] = (char)rnd.Next(97, 122);
                    }
                    resultParsed[i] += result[i, j];
                }
            }
            return result;
        }

        public void show() { 
            for(int i = 0; i < this.height; i++) {
                for(int j = 0; j < this.width; j++) {
                    if (!result[i, j].HasValue ) {
                        //Console.Write(".  ");
                        Console.Write((char)rnd.Next(97, 122) + "  ");
                    } else {
                        Console.Write(result[i, j] + "  ");    
                    }
                }
            Console.Write("\n");
            }
        }

        public string[] getGridParsed() {
            return resultParsed;
        }
    }


/*    class Tester {

        static int height = Globals.HEIGHT;
        static int width = Globals.WIDTH;
        static string[] dictionary = new string[] {"banditos","cat", "bat", "mutt", "dog", "grape"}; 

        static void Main() {
            Puzzle wordsearch = new Puzzle(width, height);
            Array.Sort(dictionary, (x, y) => y.Length.CompareTo(x.Length));
            wordsearch.getWord(ref dictionary);
            wordsearch.fill();
            wordsearch.show();
            foreach(String s in dictionary) {
                Console.WriteLine(s);
            }
        }
    }*/
}
