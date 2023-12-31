using GlobalShared;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddRazorPages();
services.AddControllersWithViews();
services.AddControllers();

var app = builder.Build();

#region Special Dir
IWebHostEnvironment env = app.Services.GetRequiredService<IWebHostEnvironment>();
//Make sure to allow read and write for all 'Special Dir' and sub folders

//This path will be used to store all data posted by employees.
//Files will automatically be classified by 'Department' => 'Activities' => 'FileFolder' => 'File'
//'Activities' can also be consider as 'Services' of the department
//'FileFolder' is the time parameter of the file posted

string clDir = Path.Combine(env.ContentRootPath, LibVariables.CompanyLibrary);
if (!Directory.Exists(clDir))
    Directory.CreateDirectory(clDir);

//This path will be used to store all data related to action request made by employees
//For more information about the feature please follow this link https://tiedapp.com/TiedAppUpdateDetail/1011
//The link provide illustrate how the feature was working without this API.
//The only update with this update is where file shared will be saved.
string actionRequestDir = Path.Combine(env.ContentRootPath, LibVariables.ActionRequest);
if (!Directory.Exists(actionRequestDir))
    Directory.CreateDirectory(actionRequestDir);

//This path will be used to store all data related to internal press release made by the direction or the management team of departments
//For more information about the feature please follow this link https://tiedapp.com/TiedAppUpdateDetail/1010
//The link provide illustrate how the feature was working without this API.
//The only update with this update is where file shared will be saved.
string internalComDir = Path.Combine(env.ContentRootPath, LibVariables.InternalCom);
if (!Directory.Exists(internalComDir))
    Directory.CreateDirectory(internalComDir);
#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});

app.Run();
