using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace SearchEngineProject
{
    internal class QueryReformulation
    {
        private static readonly Dictionary<string, double[]> TermDocumentMatrix = new Dictionary<string, double[]>();
        private static int _numberOfTerms;
        private static string[] _termArray;
        private static Dictionary<string, string[]> _extensionDictionary = new Dictionary<string, string[]>();
        private static Dictionary<int, int[]> _associatedTermMatrix = new Dictionary<int, int[]>();

        public static void AddWeightToMatrix(string term, double wdt, int docId, int numberOfDocuments)
        {
            if (new[] {"the", "in", "a", "to", "and", "of", "on", "he", "with", "hi", "that", "it", "for", "wa", "a",
"able",
"about",
"above",
"abst",
"accordance",
"according",
"accordingly",
"across",
"act",
"actually",
"added",
"adj",
"affected",
"affecting",
"affects",
"after",
"afterwards",
"again",
"against",
"ah",
"all",
"almost",
"alone",
"along",
"already",
"also",
"although",
"always",
"am",
"among",
"amongst",
"an",
"and",
"announce",
"another",
"any",
"anybody",
"anyhow",
"anymore",
"anyone",
"anything",
"anyway",
"anyways",
"anywhere",
"apparently",
"approximately",
"are",
"aren",
"arent",
"arise",
"around",
"as",
"aside",
"ask",
"asking",
"at",
"auth",
"available",
"away",
"awfully",
"b",
"back",
"be",
"became",
"because",
"become",
"becomes",
"becoming",
"been",
"before",
"beforehand",
"begin",
"beginning",
"beginnings",
"begins",
"behind",
"being",
"believe",
"below",
"beside",
"besides",
"between",
"beyond",
"biol",
"both",
"brief",
"briefly",
"but",
"by",
"c",
"ca",
"came",
"can",
"cannot",
"can't",
"cause",
"causes",
"certain",
"certainly",
"co",
"com",
"come",
"comes",
"contain",
"containing",
"contains",
"could",
"couldnt",
"d",
"date",
"did",
"didn't",
"different",
"do",
"does",
"doesn't",
"doing",
"done",
"don't",
"down",
"downwards",
"due",
"during",
"e",
"each",
"ed",
"edu",
"effect",
"eg",
"eight",
"eighty",
"either",
"else",
"elsewhere",
"end",
"ending",
"enough",
"especially",
"et",
"et-al",
"etc",
"even",
"ever",
"every",
"everybody",
"everyone",
"everything",
"everywhere",
"ex",
"except",
"f",
"far",
"few",
"ff",
"fifth",
"first",
"five",
"fix",
"followed",
"following",
"follows",
"for",
"former",
"formerly",
"forth",
"found",
"four",
"from",
"further",
"furthermore",
"g",
"gave",
"get",
"gets",
"getting",
"give",
"given",
"gives",
"giving",
"go",
"goes",
"gone",
"got",
"gotten",
"h",
"had",
"happens",
"hardly",
"has",
"hasn't",
"have",
"haven't",
"having",
"he",
"hed",
"hence",
"her",
"here",
"hereafter",
"hereby",
"herein",
"heres",
"hereupon",
"hers",
"herself",
"hes",
"hi",
"hid",
"him",
"himself",
"his",
"hither",
"home",
"how",
"howbeit",
"however",
"hundred",
"i",
"id",
"ie",
"if",
"i'll",
"im",
"immediate",
"immediately",
"importance",
"important",
"in",
"inc",
"indeed",
"index",
"information",
"instead",
"into",
"invention",
"inward",
"is",
"isn't",
"it",
"itd",
"it'll",
"its",
"itself",
"i've",
"j",
"just",
"k",
"keep",
"keeps",
"kept",
"kg",
"km",
"know",
"known",
"knows",
"l",
"largely",
"last",
"lately",
"later",
"latter",
"latterly",
"least",
"less",
"lest",
"let",
"lets",
"like",
"liked",
"likely",
"line",
"little",
"'ll",
"look",
"looking",
"looks",
"ltd",
"m",
"made",
"mainly",
"make",
"makes",
"many",
"may",
"maybe",
"me",
"mean",
"means",
"meantime",
"meanwhile",
"merely",
"mg",
"might",
"million",
"miss",
"ml",
"more",
"moreover",
"most",
"mostly",
"mr",
"mrs",
"much",
"mug",
"must",
"my",
"myself",
"n",
"na",
"name",
"namely",
"nay",
"nd",
"near",
"nearly",
"necessarily",
"necessary",
"need",
"needs",
"neither",
"never",
"nevertheless",
"new",
"next",
"nine",
"ninety",
"no",
"nobody",
"non",
"none",
"nonetheless",
"noone",
"nor",
"normally",
"nos",
"not",
"noted",
"nothing",
"now",
"nowhere",
"o",
"obtain",
"obtained",
"obviously",
"of",
"off",
"often",
"oh",
"ok",
"okay",
"old",
"omitted",
"on",
"once",
"one",
"ones",
"only",
"onto",
"or",
"ord",
"other",
"others",
"otherwise",
"ought",
"our",
"ours",
"ourselves",
"out",
"outside",
"over",
"overall",
"owing",
"own",
"p",
"page",
"pages",
"part",
"particular",
"particularly",
"past",
"per",
"perhaps",
"placed",
"please",
"plus",
"poorly",
"possible",
"possibly",
"potentially",
"pp",
"predominantly",
"present",
"previously",
"primarily",
"probably",
"promptly",
"proud",
"provides",
"put",
"q",
"que",
"quickly",
"quite",
"qv",
"r",
"ran",
"rather",
"rd",
"re",
"readily",
"really",
"recent",
"recently",
"ref",
"refs",
"regarding",
"regardless",
"regards",
"related",
"relatively",
"research",
"respectively",
"resulted",
"resulting",
"results",
"right",
"run",
"s",
"said",
"same",
"saw",
"say",
"saying",
"says",
"sec",
"section",
"see",
"seeing",
"seem",
"seemed",
"seeming",
"seems",
"seen",
"self",
"selves",
"sent",
"seven",
"several",
"shall",
"she",
"shed",
"she'll",
"shes",
"should",
"shouldn't",
"show",
"showed",
"shown",
"showns",
"shows",
"significant",
"significantly",
"similar",
"similarly",
"since",
"six",
"slightly",
"so",
"some",
"somebody",
"somehow",
"someone",
"somethan",
"something",
"sometime",
"sometimes",
"somewhat",
"somewhere",
"soon",
"sorry",
"specifically",
"specified",
"specify",
"specifying",
"still",
"stop",
"strongly",
"sub",
"substantially",
"successfully",
"such",
"sufficiently",
"suggest",
"sup",
"sure",
"t",
"take",
"taken",
"taking",
"tell",
"tends",
"th",
"than",
"thank",
"thanks",
"thanx",
"that",
"that'll",
"thats",
"that've",
"the",
"their",
"theirs",
"them",
"themselves",
"then",
"thence",
"there",
"thereafter",
"thereby",
"thered",
"therefore",
"therein",
"there'll",
"thereof",
"therere",
"theres",
"thereto",
"thereupon",
"there've",
"these",
"they",
"theyd",
"they'll",
"theyre",
"they've",
"think",
"this",
"those",
"thou",
"though",
"thoughh",
"thousand",
"throug",
"through",
"throughout",
"thru",
"thus",
"til",
"tip",
"to",
"together",
"too",
"took",
"toward",
"towards",
"tried",
"tries",
"truly",
"try",
"trying",
"ts",
"twice",
"two",
"u",
"un",
"under",
"unfortunately",
"unless",
"unlike",
"unlikely",
"until",
"unto",
"up",
"upon",
"ups",
"us",
"use",
"used",
"useful",
"usefully",
"usefulness",
"uses",
"using",
"usually",
"v",
"value",
"various",
"'ve",
"very",
"via",
"viz",
"vol",
"vols",
"vs",
"w",
"want",
"wants",
"was",
"wasnt",
"way",
"we",
"wed",
"welcome",
"we'll",
"went",
"were",
"werent",
"we've",
"what",
"whatever",
"what'll",
"whats",
"when",
"whence",
"whenever",
"where",
"whereafter",
"whereas",
"whereby",
"wherein",
"wheres",
"whereupon",
"wherever",
"whether",
"which",
"while",
"whim",
"whither",
"who",
"whod",
"whoever",
"whole",
"who'll",
"whom",
"whomever",
"whos",
"whose",
"why",
"widely",
"willing",
"wish",
"with",
"within",
"without",
"wont",
"words",
"world",
"would",
"wouldnt",
"www",
"x",
"y",
"yes",
"yet",
"you",
"youd",
"you'll",
"your",
"youre",
"yours",
"yourself",
"yourselves",
"you've",
"z",
"zero"
}.Contains(term)) return;

            if (!TermDocumentMatrix.ContainsKey(term))
            {
                var array = new double[numberOfDocuments];
                array.Initialize();
                TermDocumentMatrix.Add(term, array);
            }
            TermDocumentMatrix[term][docId] = wdt;
        }

        public static void CreateMatrix(int numberOfDocuments)
        {
            _numberOfTerms = TermDocumentMatrix.Count;
            var matrix = new double[_numberOfTerms][];
            _termArray = new string[TermDocumentMatrix.Count];

            int i = 0;
            foreach (var pair in TermDocumentMatrix)
            {
                int j = 0;
                matrix[i] = new double[numberOfDocuments];
                foreach (var weight in pair.Value)
                {
                    matrix[i][j] = weight/pair.Value.Length;
                    j++;
                }
                _termArray[i] = pair.Key;
                i++;
            }



            var transposedMatrix = TransposeMatrix(matrix, numberOfDocuments);

            _associatedTermMatrix = MultiplyMatrix(matrix, transposedMatrix, numberOfDocuments);

        }

        private static Dictionary<int, int[]> MultiplyMatrix(double[][] a, double[][] b, int numberOfDocuments)
        {
            //That dictionary associate a termId to an array of 3 termIds
            var associatedTermsDictionary = new Dictionary<int, int[]>();

            for (var i = 0; i < _numberOfTerms; i++)
            {
                var coocurenceCoeffDictionary = new List<KeyValuePair<double, int>>();
                
                for (var j = 0; j < _numberOfTerms; j++)
                {
                    double coocurenceCoefficient = 0;
                    for (var k = 0; k < numberOfDocuments; k++)
                    {
                        coocurenceCoefficient += a[i][k] * b[k][j];
                    }

                    //Keep only the 3 best coocurence coefficient
                    if (Math.Abs(coocurenceCoefficient) < double.Epsilon) continue;
                    if (coocurenceCoeffDictionary.Count < 3)
                        coocurenceCoeffDictionary.Add(new KeyValuePair<double, int>(coocurenceCoefficient, j));
                    else if (coocurenceCoefficient > coocurenceCoeffDictionary.First().Key)
                    {
                        coocurenceCoeffDictionary.Remove(coocurenceCoeffDictionary.First());
                        coocurenceCoeffDictionary.Add(new KeyValuePair<double, int>(coocurenceCoefficient, j));
                    }
                    coocurenceCoeffDictionary.Sort((apair,bpair)=> apair.Key.CompareTo(bpair.Key));
                    
                }
                //Add the associated TermIds to the dictionnary
                var tab = new int[3];
                var counter = 0;
                foreach (var keyvaluepair in coocurenceCoeffDictionary)
                {
                    tab[counter] = keyvaluepair.Value;
                    counter++;
                }
                associatedTermsDictionary.Add(i, tab);
            }

            return associatedTermsDictionary;
        }

        private static double[][] TransposeMatrix(double[][] matrix, int numberOfDocuments)
        {
            var transposedMatrix = new double[numberOfDocuments][];

            for (var i = 0; i < numberOfDocuments; i++)
            {
                transposedMatrix[i] = new double[_numberOfTerms];
                for (var j = 0; j < _numberOfTerms; j++)
                {
                    transposedMatrix[i][j] = matrix[j][i];
                }
            }

            return transposedMatrix;
        }

        public static void ToDisk(string folder)
        {
            // Create the matrix file.
            var matrixFile = new FileStream(Path.Combine(folder, "matrix.bin"), FileMode.Create);
            var vocabList = new StreamWriter(Path.Combine(folder, "vocabMatrix.bin"), false, Encoding.ASCII);
            var vocabTable = new FileStream(Path.Combine(folder, "vocabTableMatrix.bin"), FileMode.Create);

            //Write the size of the matrix
            var sizeBytes = BitConverter.GetBytes(_numberOfTerms);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(sizeBytes);
            matrixFile.Write(sizeBytes, 0, sizeBytes.Length);

            // Write the termId and termIds associated.
            var count = 0;
            foreach (var termIndex in _associatedTermMatrix.Keys)
            {
                var termIdBytes = BitConverter.GetBytes(termIndex);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(termIdBytes);
                matrixFile.Write(termIdBytes, 0, termIdBytes.Length);

                for (var i = 0; i < 3; i++)
                {
                    var termIdAssociatedBytes = BitConverter.GetBytes(_associatedTermMatrix[termIndex][i]);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(termIdAssociatedBytes);
                    matrixFile.Write(termIdAssociatedBytes, 0, termIdAssociatedBytes.Length);
                }

                // Write the length of the term
                var tSize = BitConverter.GetBytes(_termArray[count].Length);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(tSize);
                vocabTable.Write(tSize, 0, tSize.Length);

                //Write the term
                vocabList.Write(_termArray[count]);
                count++;
            }

            matrixFile.Close();
            vocabList.Close();
            vocabTable.Close();
        }

        public static void ToMemory(string folder)
        {
            var matrixFile = new FileStream(Path.Combine(folder, "matrix.bin"), FileMode.Open, FileAccess.Read);
            var vocabTable = new FileStream(Path.Combine(folder, "vocabTableMatrix.bin"), FileMode.Open, FileAccess.Read);
            var vocabList = new FileStream(Path.Combine(folder, "vocabMatrix.bin"), FileMode.Open, FileAccess.Read);

            //Read the size of the matrix
            var buffer = new byte[4];
            matrixFile.Read(buffer, 0, buffer.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            var matrixSize = BitConverter.ToInt32(buffer, 0);

            //Get the vocabulary
            var vocab = new string[matrixSize];

            // Read the word.
            for (var i = 0; i < matrixSize; i++)
            {
                // Read the length of the word.
                buffer = new byte[4];
                vocabTable.Read(buffer, 0, buffer.Length);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(buffer);
                var wordLength = BitConverter.ToInt32(buffer, 0);

                buffer = new byte[wordLength];
                vocabList.Read(buffer, 0, wordLength);
                vocab[i] = Encoding.ASCII.GetString(buffer);
            }

            //Get the associatedTerm matrix
            for (var i = 0; i < matrixSize; i++)
            {
                // Read the weight of the word.
                buffer = new byte[4];
                matrixFile.Read(buffer, 0, buffer.Length);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(buffer);
                var termId = BitConverter.ToInt32(buffer, 0);

                var term = vocab[termId];
                _extensionDictionary.Add(term, new string[3]);
                for (var j = 0; j < 3; j++)
                {
                    // Read the weight of the word.
                    buffer = new byte[4];
                    matrixFile.Read(buffer, 0, buffer.Length);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(buffer);
                    var termIdAssociated = BitConverter.ToInt32(buffer, 0);

                    _extensionDictionary[term][j] = vocab[termIdAssociated];
                }
            }
        }

        public static string[] GetExtendedQueries(string term)
        {
            return _extensionDictionary[term];
        }

    }
}