using Newtonsoft.Json;

namespace demo_app_rabbit.Services;

public interface IDemoSalesforceService
{
    Task<bool> AuthenticateAsync();
    Task<string> CreateOrderInSalesforceAsync(object order);
    Task<string> CreateCustomerInSalesforceAsync(object customer);
    bool IsAuthenticated { get; }
}

public class DemoSalesforceService : IDemoSalesforceService
{
    public bool IsAuthenticated { get; private set; }
    private string _accessToken;
    private readonly HttpClient _httpClient;

    public DemoSalesforceService()
    {
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<bool> AuthenticateAsync()
    {
        try
        {
            Console.WriteLine("🔐 Authenticating with Salesforce...");

            // Simulate OAuth2 flow
            await Task.Delay(800);

            Console.WriteLine("• Requesting OAuth2 token...");
            Console.WriteLine("• Validating credentials...");
            Console.WriteLine("• Getting instance URL...");

            _accessToken = "demo-token-" + Guid.NewGuid().ToString().Substring(0, 8);
            IsAuthenticated = true;

            Console.WriteLine($"✅ Salesforce authentication successful!");
            Console.WriteLine($"• Access Token: {_accessToken.Substring(0, 12)}...");
            Console.WriteLine($"• Instance: https://demo.salesforce.com");

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Salesforce authentication failed: {ex.Message}");
            return false;
        }
    }

    public async Task<string> CreateOrderInSalesforceAsync(object order)
    {
        if (!IsAuthenticated)
            return "Not authenticated with Salesforce";

        try
        {
            Console.WriteLine("☁️ Creating order in Salesforce...");

            // Simulate Salesforce REST API call
            var orderData = JsonConvert.SerializeObject(order);
            Console.WriteLine($"• Order data: {orderData}");

            await Task.Delay(600);

            // Simulate Salesforce response
            var salesforceId = $"a0B{DateTime.Now.Ticks.ToString().Substring(8)}";

            Console.WriteLine("• Calling: POST /services/data/v58.0/sobjects/Order__c");
            Console.WriteLine("• Status: 201 Created");
            Console.WriteLine($"• Salesforce ID: {salesforceId}");

            Console.WriteLine($"✅ Order created successfully in Salesforce!");

            return salesforceId;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error creating order in Salesforce: {ex.Message}");
            return $"Error: {ex.Message}";
        }
    }

    public async Task<string> CreateCustomerInSalesforceAsync(object customer)
    {
        if (!IsAuthenticated)
            return "Not authenticated with Salesforce";

        try
        {
            Console.WriteLine("☁️ Creating customer in Salesforce...");

            await Task.Delay(500);

            var customerId = $"001{DateTime.Now.Ticks.ToString().Substring(8)}";
            Console.WriteLine($"✅ Customer created in Salesforce! ID: {customerId}");

            return customerId;
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}