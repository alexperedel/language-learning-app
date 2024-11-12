namespace LearningApp;

public partial class DictionaryPage : ContentPage
{

    private DatabaseService _databaseService;
    private List<Word> _words;

    public DictionaryPage()
	{
		InitializeComponent();
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "words.db3");

        // Initialize the DatabaseService with the existing database path
        _databaseService = new DatabaseService(dbPath);

        // Load the words from the database asynchronously
        LoadWordsAsync();
    }

    private async void LoadWordsAsync()
    {
        _words = await _databaseService.GetWordsAsync();
        List<String> wordsNames = new List<String>();

        foreach (Word word in _words)
        {
            wordsNames.Add(word.Text);
        }
        wordListView.ItemsSource = wordsNames;
    }

    // Handle search text change to filter words
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchQuery = wordSearchBar.Text?.ToLower();
        if (string.IsNullOrEmpty(searchQuery))
        {
            wordListView.ItemsSource = _words;
        }
        else
        {
            wordListView.ItemsSource = _words.Where(word => word.Text.ToLower().Contains(searchQuery)).ToList();
        }
    }

    // Handle tapping on a word in the list
    private async void OnWordTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            var tappedWord = e.Item.ToString();
            var wordDefinition = await _databaseService.GetWordAsync(tappedWord);
            await DisplayAlert("Word", wordDefinition ?? "Definition not found", "OK");
        }
    }

}