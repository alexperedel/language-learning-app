using Newtonsoft.Json;

namespace LearningApp;

public partial class SearchPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    public SearchPage()
	{
		InitializeComponent();

	}

    private async void OnSearchButtonPressed(object sender, EventArgs e)
    {
        var searchBar = (SearchBar)sender;
        string query = searchBar.Text;
        if (!string.IsNullOrEmpty(query))
        {
            await SearchApiAsync(query);
        }
    }

    private async Task SearchApiAsync(string query)
    {
        string apiUrl = $"https://api.dictionaryapi.dev/api/v2/entries/en/{query}";

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            string jsonData = await response.Content.ReadAsStringAsync();
            List<Words>? wordMeanings = JsonConvert.DeserializeObject<List<Words>>(jsonData);

            if (wordMeanings != null && wordMeanings.Count > 0)
            {
                foreach (var wordMeaning in wordMeanings)
                {
                    Console.WriteLine(wordMeaning);
                }
                var firstWord = wordMeanings[0];
                Console.WriteLine($"Word: {firstWord.Word}");
                foreach (var phonetic in firstWord.Phonetics ?? new List<Phonetic>())
                {
                    Console.WriteLine($"Phonetic: {phonetic.Text}");
                }
                firstWord.ExtractPartsOfSpeech();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        string newText = e.NewTextValue;
    }
}