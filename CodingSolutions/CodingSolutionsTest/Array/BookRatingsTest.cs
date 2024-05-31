using FluentAssertions;

namespace CodingSolutionsTest.Array
{
        /*

    List of books, list of Genre of books and book ratings are given, all list were of equal length.
    book[], genre[], rating[] of equal length n

    Now there were 2 methods which we need to implement,
    1. getHighestRatingBookByGenre("Genre_name") If same rating books then lexographical order
    2. updateBookRatingbyBookName("book_name", int rating)



    */

    public class BookRatingsTest
    {
        [Fact]
        public void Test1()
        {
            List<string> books = new List<string> { "BookA", "BookB", "BookC", "BookD" };
            List<string> genres = new List<string> { "Fiction", "Non-Fiction", "Fiction", "Fiction" };
            List<int> ratings = new List<int> { 5, 3, 5, 4 };

            BookRatings bookRatings = new BookRatings(books, genres, ratings);

            string highestRatedFiction = bookRatings.GetHighestRatingBookByGenre("Fiction");
            highestRatedFiction.Should().Be("BookA");

            bookRatings.UpdateBookRatingByBookName("BookB", 5);
            string highestRatedNonFiction = bookRatings.GetHighestRatingBookByGenre("Non-Fiction");
            highestRatedNonFiction.Should().Be("BookB");
        }
    }

}
