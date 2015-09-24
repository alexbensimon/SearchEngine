using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SearchEngineProject
{
    class PorterStemmer
    {
        // a single consonant
        private static string c = "[^aeiou]";
        // a single vowel
        private static string v = "[aeiouy]";

        // a sequence of consonants; the second/third/etc consonant cannot be 'y'
        private static string C = c + "[^aeiouy]*";
        // a sequence of vowels; the second/third/etc cannot be 'y'
        private static string V = v + "[aeiou]*";


        // this regex tests if the token has measure > 0 [at least one VC].
        private static Regex mGr0 = new Regex("^(" + C + ")?" + V + C);


        // add more Regex variables for the following patterns:
        // m equals 1: token has measure == 1
        private static Regex mEq1 = new Regex("^(" + C + ")?" + V + C + "(" + V + ")?$");

        // m greater than 1: token has measure > 1
        private static Regex mGr1 = new Regex("^(" + C + ")?" + V + C + V + C);

        // vowel: token has a vowel after the first (optional) C
        private static Regex vowel = new Regex("^(" + C + ")?" + V);

        // double consonant: token ends in two consonants that are the same, 
        //			unless they are L, S, or Z. (look up "backreferencing" to help 
        //			with this)
        private static Regex dbcons = new Regex(@"([^aeioulszy])\1$");

        // m equals 1, Cvc: token is in Cvc form, where the last c is not w, x, 
        //			or y.
        private static Regex mEq1Cvc = new Regex("^(" + C + ")" + v + "[^aeiouwxy]$");

        private static Dictionary<string, string> suffixListS2 = new Dictionary<string, string>() {   { "ational", "ate" },
                                                                                                { "tional", "tion" },
                                                                                                {"enci", "ence" },
                                                                                                { "anci", "ance"},
                                                                                                { "izer", "ize"},
                                                                                                { "bli", "ble"},
                                                                                                {"logi", "log" },
                                                                                                { "alli", "al"},
                                                                                                { "entli", "ent"},
                                                                                                { "eli", "e"},
                                                                                                { "ousli", "ous"},
                                                                                                { "ization", "ize"},
                                                                                                { "ation", "ate"},
                                                                                                { "ator", "ate"},
                                                                                                { "alism", "al"},
                                                                                                { "iveness", "ive"},
                                                                                                { "fulness", "ful"},
                                                                                                { "ousness", "ous"},
                                                                                                { "aliti", "al"},
                                                                                                { "iviti", "ive"},
                                                                                                { "biliti", "ble"}};

        private static Dictionary<string, string> suffixListS3 = new Dictionary<string, string>() {   { "icate", "ic" },
                                                                                                { "ative", "" },
                                                                                                {"alize", "al" },
                                                                                                { "iciti", "ic"},
                                                                                                { "ical", "ic"},
                                                                                                { "ful", ""},
                                                                                                { "ness", ""}};

        private static string[] suffixListS4 = new string[] { "al",
                                                            "ance",
                                                            "ence",
                                                            "er",
                                                            "ic",
                                                            "able",
                                                            "ible",
                                                            "ant",
                                                            "ement",
                                                            "ment",
                                                            "ent",
                                                            "ion",
                                                            "ou",
                                                            "ism",
                                                            "ate",
                                                            "iti",
                                                            "ous",
                                                            "ive",
                                                            "ize"};

        public static string ProcessToken(string token)
        {
            if (token.Length < 3) return token; // token must be at least 3 chars

            // step 1a
            if (token.EndsWith("sses") || token.EndsWith("ies"))
                token = token.Substring(0, token.Length - 2);
            // program the other steps in 1a. 
            // note that Step 1a.3 implies that there is only a single 's' as the 
            //	suffix; ss does not count. you may need a regex here for 
            // "not s followed by s".
            else if (!token.EndsWith("ss") && token.EndsWith("s"))
                token = token.Substring(0, token.Length - 1);


            // step 1b
            bool doStep1Bb = false;
            //		step 1b
            if (token.EndsWith("eed"))
            {
                // 1b.1
                // token.Substring(0, token.Length - 3) is the stem prior to "eed".
                // if that has m>0, then remove the "d".
                string stem = token.Substring(0, token.Length - 3);
                if (mGr0.IsMatch(stem))
                {
                    token = stem + "ee";
                }
            }
            // program the rest of 1b. set the bool doStep1bb to true if Step 1b* 
            // should be performed.
            else if (token.EndsWith("ed"))
            {
                string stem = token.Substring(0, token.Length - 2);
                if (vowel.IsMatch(stem))
                {
                    token = stem;
                    doStep1Bb = true;
                }
            }
            else if (token.EndsWith("ing"))
            {
                string stem = token.Substring(0, token.Length - 3);
                if (vowel.IsMatch(stem))
                {
                    token = stem;
                    doStep1Bb = true;
                }
            }

            // step 1b*, only if the 1b.2 or 1b.3 were performed.
            if (doStep1Bb)
            {
                if (token.EndsWith("at") || token.EndsWith("bl") || token.EndsWith("iz"))
                {
                    token = token + "e";
                }
                else if (dbcons.IsMatch(token))
                {
                    token = token.Substring(0, token.Length - 1);
                }
                else if (mEq1Cvc.IsMatch(token))
                {
                    token = token + "e";
                }
                // use the regexes you wrote for 1b*.4 and 1b*.5
            }

            // step 1c
            // program this step. test the suffix of 'y' first, then test the 
            // condition *v*.
            if (token.EndsWith("y"))
            {
                string stem = token.Substring(0, token.Length - 1);
                if (vowel.IsMatch(stem))
                {
                    token = stem + "i";
                }
            }


            // step 2
            // program this step. for each suffix, see if the token ends in the 
            // suffix. 
            //		* if it does, extract the stem, and do NOT test any other suffix.
            //    * take the stem and make sure it has m > 0.
            //			* if it does, complete the step. if it does not, do not 
            //				attempt any other suffix.


            // you may want to write a helper method for this. a matrix of 
            // "suffix"/"replacement" pairs might be helpful. It could look like
            // string[][] step2pairs = {  new string[] {"ational", "ate"}, 
            //										new string[] {"tional", "tion"}, ....
            token = Step23(token, suffixListS2);

            // step 3
            // program this step. the rules are identical to step 2 and you can use
            // the same helper method. you may also want a matrix here.
            token = Step23(token, suffixListS3);


            // step 4
            // program this step similar to step 2/3, except now the stem must have
            // measure > 1.
            // note that ION should only be removed if the suffix is SION or TION, 
            // which would leave the S or T.
            // as before, if one suffix matches, do not try any others even if the 
            // stem does not have measure > 1.
            foreach (string suffix in suffixListS4)
            {
                if (token.EndsWith(suffix))
                {
                    string stem = token.Substring(0, token.Length - suffix.Length);
                    if (mGr1.IsMatch(stem))
                    {
                        if (!(suffix == "ion") || stem.EndsWith("s") || stem.EndsWith("t"))
                        {
                            token = stem;
                        }
                    }
                    break;
                }
            }


            // step 5
            // program this step. you have a regex for m=1 and for "Cvc", which
            // you can use to see if m=1 and NOT Cvc.
            if (token.EndsWith("e"))
            {
                string stem = token.Substring(0, token.Length - 1);
                if (mGr1.IsMatch(stem) || (mEq1.IsMatch(stem) && !mEq1Cvc.IsMatch(stem)))
                {
                    token = stem;
                }
            }

            if (token.EndsWith("ll") && mGr1.IsMatch(token))
            {
                token = token.Substring(0, token.Length - 1);
            }



            // all your code should change the variable token, which represents
            // the stemmed term for the token.
            return token;
        }

        private static string Step23(string token, Dictionary<string, string> suffixList)
        {
            foreach (string suffix in suffixList.Keys)
            {
                if (token.EndsWith(suffix))
                {
                    string stem = token.Substring(0, token.Length - suffix.Length);
                    if (mGr0.IsMatch(stem))
                    {
                        return stem + suffixList[suffix];
                    }
                    return token;
                }
            }
            return token;
        }

    }
}
