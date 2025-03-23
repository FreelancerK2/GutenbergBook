using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Book_Downloader
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Book> Books { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            // Sample books (You can replace this with actual API call)
            Books = new ObservableCollection<Book>
        {
            new Book { Title = "The Adventures of Sherlock Holmes",
                       TextUrl = "https://www.gutenberg.org/files/1661/1661-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/1661/pg1661.cover.medium.jpg" },
            new Book { Title = "Pride and Prejudice",
                       TextUrl = "https://www.gutenberg.org/files/1342/1342-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/1342/pg1342.cover.medium.jpg" },
            new Book { Title = "Dracula",
                       TextUrl = "https://www.gutenberg.org/files/345/345-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/345/pg345.cover.medium.jpg" },
            new Book { Title = "Moby-Dick",
                       TextUrl = "https://www.gutenberg.org/files/2701/2701-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/2701/pg2701.cover.medium.jpg" },
            new Book { Title = "Frankenstein",
                       TextUrl = "https://www.gutenberg.org/files/84/84-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/84/pg84.cover.medium.jpg" },
            new Book { Title = "The Picture of Dorian Gray",
                       TextUrl = "https://www.gutenberg.org/files/174/174-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/174/pg174.cover.medium.jpg" },
            new Book { Title = "The Secret Garden",
                       TextUrl = "https://www.gutenberg.org/files/17396/17396-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/17396/pg17396.cover.medium.jpg" },
            new Book { Title = "Alice's Adventures in Wonderland",
                       TextUrl = "https://www.gutenberg.org/files/11/11-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/11/pg11.cover.medium.jpg" },
            new Book { Title = "The War of the Worlds",
                       TextUrl = "https://www.gutenberg.org/files/36/36-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/36/pg36.cover.medium.jpg" },
            new Book { Title = "The Call of the Wild",
                       TextUrl = "https://www.gutenberg.org/files/215/215-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/215/pg215.cover.medium.jpg" },
            new Book { Title = "The Metamorphosis",
                       TextUrl = "https://www.gutenberg.org/files/5200/5200-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/5200/pg5200.cover.medium.jpg" },
            new Book { Title = "War and Peace",
                       TextUrl = "https://www.gutenberg.org/files/2600/2600-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/2600/pg2600.cover.medium.jpg" },
            new Book { Title = "Ulysses",
                       TextUrl = "https://www.gutenberg.org/files/4300/4300-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/4300/pg4300.cover.medium.jpg" },
            new Book { Title = "Les Misérables",
                       TextUrl = "https://www.gutenberg.org/files/135/135-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/135/pg135.cover.medium.jpg" },
            new Book { Title = "The Count of Monte Cristo",
                       TextUrl = "https://www.gutenberg.org/files/1184/1184-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/1184/pg1184.cover.medium.jpg" },
            new Book { Title = "The Odyssey",
                       TextUrl = "https://www.gutenberg.org/files/1727/1727-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/1727/pg1727.cover.medium.jpg" },
            new Book { Title = "The Brothers Karamazov",
                       TextUrl = "https://www.gutenberg.org/files/28054/28054-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/28054/pg28054.cover.medium.jpg" },
            new Book { Title = "Don Quixote",
                       TextUrl = "https://www.gutenberg.org/files/2000/2000-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/2000/pg2000.cover.medium.jpg" },
            new Book { Title = "Anna Karenina",
                       TextUrl = "https://www.gutenberg.org/files/1399/1399-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/1399/pg1399.cover.medium.jpg" },
            new Book { Title = "Crime and Punishment",
                       TextUrl = "https://www.gutenberg.org/files/2554/2554-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/2554/pg2554.cover.medium.jpg" },
            new Book { Title = "The Scarlet Letter",
                       TextUrl = "https://www.gutenberg.org/files/33/33-0.txt",
                       CoverUrl = "https://www.gutenberg.org/cache/epub/33/pg33.cover.medium.jpg" }
        };

            BookList.ItemsSource = Books;
        }


        private async void BookList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookList.SelectedItem is Book selectedBook)
            {
                SelectedBookTitle.Text = selectedBook.Title;
                TextUrl.Text = selectedBook.TextUrl; // Set book URL

                // Load Book Cover
                await LoadBookCover(selectedBook.CoverUrl);

                // Load Book Content (First Few Lines)
                await LoadBookContent(selectedBook.TextUrl);
            }
        }

        private async Task LoadBookCover(string coverUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(coverUrl))
                {
                    using HttpClient client = new HttpClient();
                    var imageBytes = await client.GetByteArrayAsync(coverUrl);

                    using var stream = new MemoryStream(imageBytes);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze(); // Freezing for UI thread safety

                    BookCover.Source = bitmap;
                }
                else
                {
                    BookCover.Source = new BitmapImage(new Uri("pack://application:,,,/Images/placeholder.png"));
                }
            }
            catch
            {
                BookCover.Source = new BitmapImage(new Uri("pack://application:,,,/Images/placeholder.png"));
            }
        }

        private async Task LoadBookContent(string textUrl)
        {
            try
            {
                using HttpClient client = new HttpClient();
                string content = await client.GetStringAsync(textUrl);
                string previewText = string.Join("\n", content.Split('\n')[..10]); // Get first 10 lines

                BookDetails.Document.Blocks.Clear();
                BookDetails.Document.Blocks.Add(new Paragraph(new Run(previewText)));
            }
            catch
            {
                BookDetails.Document.Blocks.Clear();
                BookDetails.Document.Blocks.Add(new Paragraph(new Run("Unable to load book content.")));
            }
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookList.SelectedItem is Book selectedBook)
            {
                try
                {
                    // Define Download Folder
                    string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DownloadedBooks");

                    // Create Directory if it doesn't exist
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    // Get Valid File Name
                    string validFileName = string.Concat(selectedBook.Title.Split(Path.GetInvalidFileNameChars()));
                    string filePath = Path.Combine(folderPath, $"{validFileName}.txt");

                    using HttpClient client = new HttpClient();
                    string content = await client.GetStringAsync(selectedBook.TextUrl);
                    await File.WriteAllTextAsync(filePath, content);

                    MessageBox.Show($"Book downloaded successfully:\n{filePath}", "Download Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error downloading book: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a book first.", "No Book Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void TextUrl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (BookList.SelectedItem is Book selectedBook)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = selectedBook.TextUrl,
                    UseShellExecute = true
                });
            }
        }
    }

    public class Book
    {
        public string Title { get; set; }
        public string TextUrl { get; set; }
        public string CoverUrl { get; set; }
    }
}
