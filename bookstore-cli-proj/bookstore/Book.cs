namespace bookstore_cli_proj.bookstore
{
     internal class Book
     {
          public string? title;
          public string author;
          public string price;
          public int qty;
          public Book()
          {
               title = "N/A";
               author = "N/A";
               price = "0.0";
               qty = 0;
          }
          public Book(string title, string author, string price, int qty)
          {
               this.title = title;
               this.author = author;
               this.price = price;
               this.qty = qty;
          }
     }
}