using ABPD_HW_02.Factories;

var builder = WebApplication.CreateBuilder(args);

//register DeviceManager as a singleton (loaded from file)
builder.Services.AddSingleton<ABPD_HW_02.Managers.DeviceManager>(sp =>
{
    string inputFilePath = "resources/input.txt";
    string outputFilePath = "resources/output.txt";
    return DeviceManagerFactory.Create(inputFilePath, outputFilePath);
});

//path to the CSV-like file that holds device information.
string filePath = "resources/input.txt";

string outputFilePath = "resources/output.txt";
//instantiate the DeviceManager, which should automatically loads devices from the file.
        
var deviceManager = DeviceManagerFactory.Create(filePath, outputFilePath);

Console.WriteLine("Initial Device List (Loaded from file):");
deviceManager.ShowAllDevices();
Console.WriteLine();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();