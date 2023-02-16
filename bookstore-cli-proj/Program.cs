namespace bookstore_cli_proj.bookstore
{
     class Program
     {
          static void Main(string[] args)
          {
               Bookstore myStore = new Bookstore();
               myStore.setupDatabase();
               myStore.OpeningDay();
          }
     }
}