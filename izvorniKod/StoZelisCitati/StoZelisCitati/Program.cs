using System.Transactions;
using Dapper;
using StoZelisCitati.Misc;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/account/denied";
        options.LoginPath = "/account/login";
    });

builder.Services.AddScoped<NpgsqlConnection>(_ =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<NpgsqlRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


NpgsqlRepository repo = 
new NpgsqlRepository(new NpgsqlConnection(builder.Configuration.GetConnectionString("Default")));

int bookId = 3;
string imagePath = "1984.jpg";
    
int? userId = await repo.GetOwnerOfBook(bookId)!;
if (userId == null)
    throw new Exception("book does not belong to user.");

byte[] image = File.ReadAllBytes($"C:/Users/lovri/Downloads/{imagePath}");
await repo.AddBookCover(image, "image/jpg", bookId);

//
// Dictionary<int, string> titleToFile = new()
// {
//     {1, "hitchhiker.jpg"},
//     {2, "harry.jpg"},
//     {4, "lotr.jpg"},
//     {5, "pride and prejudice.jpg"},
//     {6, "do androids.jpg"},
//     {7, "i robot.jpg"},
//     {8, "red october.jpg"},
//     {9, "foundation.jpg"},
//     {10, "fahrenheit.jpg"},
//     {12, "zločinikazna.jpg"},
//     {13, "hamlet.jpg"},
//     {15, "ana.jpg"},
//     {16, "stranac.jpg"},
//     {17, "don.jpg"},
//     {18, "trial.jpg"},
//     {19, "romeo.jpg"},
//     {20, "ubitipticu.jpg"},
//     {21, "lolita.jpg"},
//     {22, "war and peace.jpg"},
//     {23, "othello.jpg"},
//     {24, "idiot.jpg"},
//     {25, "divine.jpg"},
//     {26, "tale.jpg"},
//     {27, "belltolls.jpg"},
//     {28, "farewell.jpg"},
//     {29, "illiad.jpg"},
//     {30, "guliver.jpg"},
//     {31, "canterbury.jpg"},
//     {32, "ulyssey.jpg"},
//     {33, "king lear.jpg"},
//     {34, "odyssey.jpg"},
//     {35, "decameron.jpg"}
// };
//
// using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
// {
//     foreach (KeyValuePair<int,string> pair in titleToFile)
//     {
//         int bookId = pair.Key;
//         string imagePath = pair.Value;
//     
//         int? userId = await repo.GetOwnerOfBook(bookId)!;
//         if (userId == null)
//             throw new Exception("book does not belong to user.");
//
//         byte[] image = File.ReadAllBytes($"C:/Users/lovri/Downloads/{imagePath}");
//         await repo.AddBookCover(image, "image/jpg", bookId);
//     }
//     transactionScope.Complete();
// }

app.Run();