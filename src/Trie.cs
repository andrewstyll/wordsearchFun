using System;

namespace dataStructure {
  
    public static class Globals {
        public const int ALPHABET_SIZE = 26;
    }

    class Node {
        private Node[] alphabet = new Node[Globals.ALPHABET_SIZE];
        private char letter;
        private bool endWord;

        public Node(char c) {
            for(int i = 0; i < Globals.ALPHABET_SIZE; i++) {
                alphabet[i] = null;
            }
            letter = c;
            endWord = false;
        }

        bool isLeaf() {
            for(int i = 0; i < Globals.ALPHABET_SIZE; i++) {
                if(alphabet[i] != null) {
                    return false;
                }
            }
            return true;
        }

        void clear() {
            for(int i = 0; i < Globals.ALPHABET_SIZE; i++) {
                if(alphabet[i] != null) {
                    alphabet[i] = null;
                }
            }
        }

        public int getIndex(char c) {
            return c - 'a';
        }

        public int size() {
            int count = 0;
            for(int i = 0; i < Globals.ALPHABET_SIZE; i++) {
                if(alphabet[i] != null) {
                    count += alphabet[i].size();
                }
            }
            return 1 + count;
        }

        public bool insertWord(string word, int i) {
            
            int j;
            if(i == word.Length) {
                endWord = true;
                return true;
            }
            j = getIndex(word[i]); //get index in the alphabet for the word
            
            if(alphabet[j] == null) {
                alphabet[j] = new Node(word[i]);
            }
            return alphabet[j].insertWord(word, i+1);
        }

        public bool removeWord(string word, int i) {
            
            if(i == word.Length) {
                if(endWord == true) {
                    endWord = false;
                    return true;
                }
                return false;
            }
            
            Node tmp = alphabet[getIndex(word[i])]; 
            
            if(tmp == null) {
                return false;
            }
            
            if(tmp.removeWord(word, i+1)) { //that means it was the end of the word
                if(tmp.isLeaf() == true) {
                    tmp = null;
                    alphabet[getIndex(word[i])] = null;
                }
                return true;
            }
            return false;

        }

        public bool findWord(string word, int i) {
            if(i == word.Length) {
                if(endWord == true) {
                    return true;
                }
                return false;
            }
            Node tmp = alphabet[getIndex(word[i])];
            if(tmp == null) {
                return false;
            }
             
            return tmp.findWord(word, i+1);
        }

        //ca is the string. ca.length = 2
        public bool stringExists(string word, int i) {
            if(i == word.Length) {
                return true;
            }
            Node tmp = alphabet[getIndex(word[i])];
            if(tmp == null) {
                return false;
            }
             
            return tmp.stringExists(word, i+1);
        }
    }
    
    class Trie {
        private Node head;

        public Trie() {
            head = null;
        }

        public int size() {
            if(head == null) {
                return 0;
            } else {
                return head.size(); 
            }
        }
        public bool insertWord(string word) {
            if (word == " ") {
                return false;
            } else if(head == null) {
                head = new Node('\0');
                return head.insertWord(word, 0);
            } else {
                return head.insertWord(word, 0);
            }
        }

        public bool removeWord(string word) {
            if(head == null) {
                return false;
            } else {
                return head.removeWord(word, 0);
            }
        }
        
        public bool findWord(string word) {
            if(head == null) {
                return false;
            } else {
                return head.findWord(word, 0);
            }
        }
        
        public bool stringExists(string word) {
            if(head == null) {
                return false;
            } else {
                return head.stringExists(word, 0);
            }
        }
    }

    /*my program has to traverse a grid and return feedback to determine if the word was printed
    the word has to be able to add and remove words as the program runs. the program also has to find 
    words based on an input character. Let's start with the addition of words*/

    /*class Tester {
        static void Main() {
            string[] dictionary = new string[] {"banditos","cat", "bat", "mutt", "dog", "grape"};
            
            Trie t = new Trie();
            for(int i = 0; i < dictionary.Length; i++) {
                if(t.insertWord(dictionary[i])) {
                    Console.WriteLine(dictionary[i]);
                }

            }
            for(int i = 0; i < dictionary.Length; i++) {
                if(t.findWord(dictionary[i])) {
                    Console.WriteLine(dictionary[i]);
                }
            }
            if(!t.stringExists("al;kdsjf")) {
                Console.WriteLine("NOPE");
            }
            for(int i = 0; i < dictionary.Length; i++) {
                if(t.removeWord(dictionary[i])) {
                    Console.WriteLine(dictionary[i]);
                }
            }
            Console.WriteLine(t.size());
        }
    }*/
}
