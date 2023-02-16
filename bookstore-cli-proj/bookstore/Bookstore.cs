using MySql.Data.MySqlClient;

namespace bookstore_cli_proj.bookstore
{
     class Bookstore
     {
          DateTime foundedDate;
          List<Book> MyBooks;
          public string name;
          MySqlConnection connection;
          public Bookstore()
          {
               name = "CTG";
               foundedDate = DateTime.Now;
               connection = getConnection();
               MyBooks = fetchBookData();

          }
          public MySqlConnection getConnection()
          {
               //Research if this string han be pulled from hidden file
               string server = "localhost";
               string database = "myBookStoredb";
               string username = "root";
               string password = "CSCI4050";
               string constring = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + username + ";" + "PASSWORD=" + password + ";";
               MySqlConnection myconnection = new MySqlConnection(constring);
               return myconnection;
          }
          public List<Book> fetchBookData()
          {
               List<Book> tempData = new List<Book>();
               this.connection.Open();
               string query = "select * from books";
               MySqlCommand cmd = new MySqlCommand(query, this.connection);
               try
               {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                         tempData.Add(new Book((string)reader["title"], (string)reader["author"], (string)reader["price"], (int)reader["qty"]));
                    }
               }
               catch (Exception e)
               {
                    Console.WriteLine(e.Message);
               }
               this.connection.Close();
               return tempData;
          }
          public void executeMySqlQuery(string query)
          {
               //Dynamic sql execution of query
               try
               {
                    this.connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, this.connection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Close();
                    this.connection.Close();
               }
               catch (Exception e)
               {
                    Console.WriteLine(e.Message);
               }
          }
          public Boolean bookAlreadyInList(string mytitle, string myauthor)
          {
               for (int i = 0; i < MyBooks.Count; i++)
               {
                    if (MyBooks[i].title!.ToLower() == mytitle.ToLower() && MyBooks[i].author.ToLower() == myauthor.ToLower())
                    {
                         return true;
                    }
               }
               return false;
          }
          public void viewAllBooks()
          {
               Console.WriteLine($"Mybooks count is {MyBooks.Count}");
               for (int i = 0; i < MyBooks.Count; i++)
               {
                    Console.WriteLine($"{i} - {MyBooks[i].title}");
               }
          }
          public void viewBook()
          {
               if (MyBooks.Count() > 0)
               {
                    Console.WriteLine("Enter slot id");
                    string? slotIdInput = Console.ReadLine();
                    try
                    {
                         //Make sure not null with slotIdInput!
                         int sid = Int32.Parse(slotIdInput!);
                         Console.WriteLine("Book Info");
                         Console.WriteLine($"Title: \t\t{MyBooks[sid].title}");
                         Console.WriteLine($"Author: \t{MyBooks[sid].author}");
                         Console.WriteLine($"Price: \t\t{MyBooks[sid].price}");
                         Console.WriteLine($"Quantity: \t{MyBooks[sid].qty}");

                    }
                    catch (Exception e)
                    {
                         //FormatException or ArgumentOutOfRangeException
                         Console.WriteLine(e.Message);
                         viewBook();
                    }
               }
               else
               {
                    Console.WriteLine("No books in inventory");
               }

          }
          public void addBook()
          {
               Console.WriteLine("Add Book? Y or N");
               string? input = Console.ReadLine();
               if (input!.ToLower() == "y" || input!.ToLower() == "yes")
               {
                    try
                    {
                         Console.Write("Enter Title: ");
                         string? titleInput = Console.ReadLine()!.Trim();
                         Console.Write("Enter Author: ");
                         string? authorInput = Console.ReadLine()!.Trim();
                         //add if autohr or title null end methods
                         Console.Write("Enter Price: ");
                         string? priceInput = Console.ReadLine()!.Trim();
                         Console.Write("Enter Quantity: ");
                         string? qtyInput = Console.ReadLine()!.Trim();
                         Boolean bookInList = bookAlreadyInList(titleInput, authorInput);
                         if (!bookInList)
                         {
                              string query = @$"insert into books (title, author, price, qty) values (""{titleInput}"", ""{authorInput}"", {priceInput}, {qtyInput})";
                              MyBooks.Add(new Book(titleInput, authorInput, priceInput, Convert.ToInt32(qtyInput)));
                              executeMySqlQuery(query);
                         }
                         else
                         {
                              Console.WriteLine("Duplicate Entry. Try Again");
                         }
                    }
                    catch (Exception e)
                    {
                         Console.WriteLine(e.Message);
                    }
               }
          }

          public void editBook()
          {
               if (MyBooks.Count() > 0)
               {
                    Console.WriteLine("Edit Book? Y or N");
                    string? input = Console.ReadLine();
                    if (input!.ToLower() == "y" || input!.ToLower() == "yes")
                    {
                         try
                         {
                              Console.WriteLine("Enter slot id");
                              string? slotIdInput = Console.ReadLine();
                              int sid = Int32.Parse(slotIdInput!);
                              Boolean editingBook = true;
                              while (editingBook)
                              {
                                   Console.WriteLine("Enter letter option to edit");
                                   Console.WriteLine("0 - Title");
                                   Console.WriteLine("1 - Author");
                                   Console.WriteLine("2 - Price");
                                   Console.WriteLine("3 - Quantity");
                                   Console.WriteLine("4 - Done");
                                   Console.WriteLine("Edit option: ");
                                   string? editInput = Console.ReadLine();
                                   if (editInput == "0")
                                   {
                                        Console.Write("Enter New Title: ");
                                        string? titleInput = Console.ReadLine()!.Trim();
                                        Boolean bookInList = bookAlreadyInList(titleInput, MyBooks[sid].author);
                                        if (!bookInList)
                                        {
                                             string query = @$"update books set title = ""{titleInput}"" where title = ""{MyBooks[sid].title}"" and author = ""{MyBooks[sid].author}""";
                                             executeMySqlQuery(query);
                                             MyBooks[sid].title = titleInput;
                                        }
                                        else
                                        {
                                             Console.WriteLine("New title is the same as old title");
                                        }
                                   }
                                   else if (editInput == "1")
                                   {
                                        Console.Write("Enter New Author: ");
                                        string? authorInput = Console.ReadLine()!.Trim();
                                        Boolean bookInList = bookAlreadyInList(MyBooks[sid].title!, authorInput);
                                        if (!bookInList)
                                        {
                                             string query = @$"update books set title = ""{authorInput}"" where title = ""{MyBooks[sid].title}"" and author = ""{MyBooks[sid].author}""";
                                             executeMySqlQuery(query);
                                             MyBooks[sid].author = authorInput;
                                        }
                                        else
                                        {
                                             Console.WriteLine("New author is the same as old author");
                                        }
                                   }
                                   else if (editInput == "2")
                                   {
                                        Console.Write("Enter New Price: ");
                                        string? priceInput = Console.ReadLine()!.Trim();
                                        string query = @$"update books set price = ""{priceInput}"" where title = ""{MyBooks[sid].title}"" and author = ""{MyBooks[sid].author}""";
                                        executeMySqlQuery(query);
                                        MyBooks[sid].price = priceInput;
                                   }
                                   else if (editInput == "3")
                                   {
                                        Console.Write("Enter New Quantity: ");
                                        string? qtyInput = Console.ReadLine()!.Trim();
                                        string query = @$"update books set qty = ""{Convert.ToInt32(qtyInput)}"" where title = ""{MyBooks[sid].title}"" and author = ""{MyBooks[sid].author}""";
                                        executeMySqlQuery(query);
                                        MyBooks[sid].qty = Convert.ToInt32(qtyInput);
                                   }
                                   else if (editInput == "4")
                                   {
                                        editingBook = false;
                                   }
                                   else
                                   {
                                        Console.WriteLine("Incorrect Input, Try Again.");
                                   }
                              }
                         }
                         catch (Exception e)
                         {
                              Console.WriteLine(e.Message);
                         }
                    }
               }
               else
               {
                    Console.WriteLine("No books in inventory");
               }

          }

          public void deleteBook()
          {
               if (MyBooks.Count() > 0)
               {
                    Console.WriteLine("Continue to delete book? Y or N");
                    string? input = Console.ReadLine();
                    if (input!.ToLower() == "y" || input.ToLower() == "yes")
                    {
                         Console.WriteLine("Enter slot id");
                         string? slotIdInput = Console.ReadLine();
                         try
                         {
                              //Make sure not null with slotIdInput!
                              int sid = Int32.Parse(slotIdInput!);
                              Boolean bookInList = bookAlreadyInList(MyBooks[sid].title!, MyBooks[sid].author);
                              if (bookInList)
                              {
                                   string query = @$"delete from books where title = ""{MyBooks[sid].title}"" and author = ""{MyBooks[sid].author}"" and price = ""{MyBooks[sid].price}"" and qty = {MyBooks[sid].qty}";
                                   MyBooks.Remove(MyBooks[sid]);
                                   executeMySqlQuery(query);
                                   Console.WriteLine("Book removed");
                              }
                              else
                              {
                                   Console.WriteLine("Book does not exist.");
                              }
                         }
                         catch (Exception e)
                         {
                              //FormatException or ArgumentOutOfRangeException
                              Console.WriteLine(e.Message);
                              deleteBook();
                         }
                    }

               }
               else
               {
                    Console.WriteLine("Inventory is empty");
               }
          }
          public void Prompt()
          {
               Console.WriteLine("Enter preferred action");
               Console.WriteLine("0 - View All Books");
               Console.WriteLine("1 - View Book Details");
               Console.WriteLine("2 - Add Book");
               Console.WriteLine("3 - Edit Book");
               Console.WriteLine("4 - Delete Book");
               Console.WriteLine("h - print menu");
               Console.WriteLine("q - exit");
          }
          public void OpeningDay()
          {
               Console.WriteLine("Welcome to the CTG Book store!");
               Console.WriteLine($"Founded {foundedDate}");
               Console.WriteLine("How may we be of service?");
               Boolean open = true;
               Prompt();
               while (open)
               {
                    Console.Write("Command: ");
                    string? userInput = Console.ReadLine();
                    if (userInput == "0")
                    {
                         viewAllBooks();
                    }
                    else if (userInput == "1")
                    {
                         viewBook();
                    }
                    else if (userInput == "2")
                    {
                         addBook();
                    }
                    else if (userInput == "3")
                    {
                         editBook();
                    }
                    else if (userInput == "4")
                    {
                         deleteBook();
                    }
                    else if (userInput!.ToLower().Equals("h") || userInput.ToLower().Equals("help"))
                    {
                         Prompt();
                    }
                    else if (userInput!.ToLower() == "q" || userInput.ToLower() == "quit")
                    {
                         Console.WriteLine("Thank you for shopping at CTG Bookstore.");
                         open = false;
                    }
                    else
                    {
                         Console.WriteLine("Incorrect input...");
                    }
               }
          }
          public void setupDatabase()
          {
               FileInfo file = new FileInfo("setup.sql");
               string script = file.OpenText().ReadToEnd();
               executeMySqlQuery(script);
          }
     }
}
