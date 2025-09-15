using System.Text.Json;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        // Problem 1 - O(n) symmetric 2-char pair finder using a HashSet
        // Contract:
        //  - Input: array of 2-character (assumed) distinct strings (may contain duplicates in stress test)
        //  - Output: each unique unordered pair (w, rev) where rev == reverse(w) and w[0] != w[1]
        //  - Complexity: O(n) time, O(n) space
        // Approach:
        //  1. Add all words to a HashSet for O(1) membership lookups.
        //  2. Iterate original list once. For each word w:
        //     - Skip if not length 2 or characters identical ("aa").
        //     - Compute reversed form rev.
        //     - If rev exists in the set, generate a canonical key using min/max ordering so we only add once.
        //     - Use a HashSet to ensure uniqueness of produced pairs (important when duplicates exist in input test).
        //  3. Return list of strings in format "X & Y" (ordering internal does not matter due to canonicalization in tests).
        var all = new HashSet<string>(words);
        var seenPairs = new HashSet<string>(); // canonical key = smaller+"|"+larger
        var result = new List<string>();

        foreach (var w in words)
        {
            if (string.IsNullOrEmpty(w) || w.Length != 2) continue;
            if (w[0] == w[1]) continue; // skip same-letter words (no self pair per instructions)
            var rev = new string(new[] { w[1], w[0] });
            if (!all.Contains(rev)) continue;

            // Canonical ordering to avoid duplicates (store smaller|larger)
            var a = string.CompareOrdinal(w, rev) < 0 ? w : rev;
            var b = string.CompareOrdinal(w, rev) < 0 ? rev : w;
            var key = a + "|" + b;
            if (seenPairs.Add(key))
            {
                // Following expected examples they put lexicographically larger first, but tests canonicalize anyway.
                result.Add(b + " & " + a);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>fixed array of divisors</returns>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();
        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(",");
            // Problem 2 - Count degree occurrences (4th column -> index 3)
            if (fields.Length > 3)
            {
                var degree = fields[3].Trim();
                if (degree.Length == 0) continue;
                if (degrees.ContainsKey(degree))
                    degrees[degree] += 1;
                else
                    degrees[degree] = 1;
            }
        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a 
    /// new word.  A dictionary is used to solve the problem.
    /// 
    /// Examples:
    /// is_anagram("CAT","ACT") would return true
    /// is_anagram("DOG","GOOD") would return false because GOOD has 2 O's
    /// 
    /// Important Note: When determining if two words are anagrams, you
    /// should ignore any spaces.  You should also ignore cases.  For 
    /// example, 'Ab' and 'Ba' should be considered anagrams
    /// 
    /// Reminder: You can access a letter by index in a string by 
    /// using the [] notation.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        // Problem 3 - O(n) anagram check ignoring spaces and case.
        // Efficient approach using a fixed-size frequency table of all possible char values (UTF-16 range).
        if (word1 == null || word2 == null) return false;

        var counts = new int[char.MaxValue + 1];
        int len1 = 0, len2 = 0;

        foreach (var c in word1)
        {
            if (c == ' ') continue;
            var lower = char.ToLowerInvariant(c);
            counts[lower]++;
            len1++;
        }
        foreach (var c in word2)
        {
            if (c == ' ') continue;
            var lower = char.ToLowerInvariant(c);
            counts[lower]--;
            len2++;
        }

        if (len1 != len2) return false; // quick length check after removing spaces

        // Verify all counts balanced
        foreach (var v in counts)
        {
            if (v != 0) return false;
        }
        return true;
    }

    /// <summary>
    /// This function will read JSON (Javascript Object Notation) data from the 
    /// United States Geological Service (USGS) consisting of earthquake data.
    /// The data will include all earthquakes in the current day.
    /// 
    /// JSON data is organized into a dictionary. After reading the data using
    /// the built-in HTTP client library, this function will return a list of all
    /// earthquake locations ('place' attribute) and magnitudes ('mag' attribute).
    /// Additional information about the format of the JSON data can be found 
    /// at this website:  
    /// 
    /// https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php
    /// 
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        // TODO Problem 5:
        // 1. Add code in FeatureCollection.cs to describe the JSON using classes and properties 
        // on those classes so that the call to Deserialize above works properly.
        // 2. Add code below to create a string out each place a earthquake has happened today and its magitude.
        // 3. Return an array of these string descriptions.
        var list = new List<string>();
        if (featureCollection != null && featureCollection.Features != null)
        {
            foreach (var f in featureCollection.Features)
            {
                if (f == null || f.Properties == null) continue;
                var place = f.Properties.Place;
                var mag = f.Properties.Mag; // 0.0 might represent either real 0 magnitude or missing; we still display it.
                if (!string.IsNullOrWhiteSpace(place))
                {
                    list.Add($"{place} - Mag {mag:0.0}");
                }
            }
        }
        return list.ToArray();
    }
}