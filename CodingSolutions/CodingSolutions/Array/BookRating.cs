public class BookRatings
{
    private List<string> books;
    private List<string> genres;
    private List<int> ratings;
    
    public BookRatings(List<string> books, List<string> genres, List<int> ratings)
    {
        if (books.Count != genres.Count || genres.Count != ratings.Count)
        {
            throw new ArgumentException("All lists must have the same length");
        }

        this.books = new List<string>(books);
        this.genres = new List<string>(genres);
        this.ratings = new List<int>(ratings);
    }

    public string GetHighestRatingBookByGenre(string genre)
    {
        var genreBooks = new List<Tuple<string, int>>();

        for (int i = 0; i < genres.Count; i++)
        {
            if (genres[i] == genre)
            {
                genreBooks.Add(new Tuple<string, int>(books[i], ratings[i]));
            }
        }

        if (genreBooks.Count == 0)
        {
            return null; // or throw an exception or return a default value
        }

        genreBooks.Sort((a, b) =>
        {
            int ratingComparison = b.Item2.CompareTo(a.Item2);
            if (ratingComparison == 0)
            {
                return a.Item1.CompareTo(b.Item1);
            }
            return ratingComparison;
        });

        return genreBooks[0].Item1;
    }

    public void UpdateBookRatingByBookName(string bookName, int rating)
    {
        int index = books.IndexOf(bookName);
        if (index != -1)
        {
            ratings[index] = rating;
        }
        else
        {
            throw new ArgumentException("Book not found");
        }
    }
}