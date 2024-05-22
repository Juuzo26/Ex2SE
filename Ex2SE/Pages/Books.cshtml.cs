using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Ex2SE.Pages
{
    public class BooksModel : PageModel
    {
        public List<BookInfo> listBooks = new List<BookInfo>();

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=(local)\\SQLEXPRESS;Initial Catalog=LabDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        SELECT 
                            b.BookId, 
                            b.title,
                            b.isbn,
                            p.PublisherName,
                            a.AuthorName,
                            c.CategoryName
                        FROM 
                            BOOK b
                        INNER JOIN 
                            PUBLISHER p ON b.PublisherId = p.PublisherId
                        INNER JOIN 
                            AUTHOR a ON b.AuthorId = a.AuthorId
                        INNER JOIN 
                            CATEGORY c ON b.CategoryId = c.CategoryId;
                    ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BookInfo book = new BookInfo();
                                book.BookId = reader.GetInt32(0);
                                book.Title = reader.GetString(1);
                                book.ISBN = reader.GetString(2);
                                book.PublisherName = reader.GetString(3);
                                book.AuthorName = reader.GetString(4);
                                book.CategoryName = reader.GetString(5);

                                listBooks.Add(book);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log it, show a user-friendly message, etc.)
                throw new Exception("An error occurred while retrieving data from the database.", ex);
            }
        }
    }

    public class BookInfo
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublisherName { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
    }
}
