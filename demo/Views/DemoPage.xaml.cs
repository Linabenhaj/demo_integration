using demo_app_rabbit.Services;
using System.Text;

namespace demo_app_rabbit;

public partial class DemoPage : ContentPage
{
    private readonly IDemoRabbitMQService _rabbitMQService;
    private readonly IDemoSalesforceService _salesforceService;

    public DemoPage(IDemoRabbitMQService rabbitMQService,
                    IDemoSalesforceService salesforceService)
    {
        InitializeComponent();

        _rabbitMQService = rabbitMQService;
        _salesforceService = salesforceService;

        // Set up initial state
        Log("🚀 RabbitMQ ↔ Salesforce Demo Started");
        Log("🔗 MAUI App → RabbitMQ → Salesforce Integration");
        Log("🔄 CI/CD Pipeline: .github/workflows/ci-cd-demo.yml");
    }

    private void Log(string message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LogOutput.Text += $"[{DateTime.Now:HH:mm:ss}] {message}\n";
            LogOutput.CursorPosition = LogOutput.Text.Length;
        });
    }

    private async void OnSendToRabbitMQClicked(object sender, EventArgs e)
    {
        Log("📤 Step 1: Sending to RabbitMQ...");

        var customer = CustomerEntry.Text ?? "Demo Customer";
        var product = ProductEntry.Text ?? "Demo Product";

        // Connect to RabbitMQ
        var connected = await _rabbitMQService.ConnectAsync();
        if (!connected)
        {
            Log("❌ Failed to connect to RabbitMQ");
            return;
        }

        // Create order object
        var order = new
        {
            Id = $"ORD-{DateTime.Now.Ticks}",
            Customer = customer,
            Product = product,
            Quantity = 1,
            Amount = 99.99m,
            Timestamp = DateTime.Now
        };

        // Send to RabbitMQ
        var messageId = await _rabbitMQService.SendOrderAsync(order);

        Log($"✅ Order sent to RabbitMQ: {order.Id}");
        Log($"📬 Message ID: {messageId}");
        Log($"📊 Queue: salesforce_orders");
    }

    private async void OnSyncToSalesforceClicked(object sender, EventArgs e)
    {
        Log("☁️ Step 2: Syncing to Salesforce...");

        // Show progress
        await AnimateProgress(SalesforceProgress, 0, 1);

        // Authenticate with Salesforce
        var authenticated = await _salesforceService.AuthenticateAsync();
        if (!authenticated)
        {
            Log("❌ Salesforce authentication failed");
            return;
        }

        // Create order in Salesforce
        var order = new
        {
            OrderNumber = $"SO-{DateTime.Now.Ticks.ToString().Substring(8)}",
            CustomerName = CustomerEntry.Text ?? "Demo Customer",
            TotalAmount = 99.99m,
            Status = "New"
        };

        var salesforceId = await _salesforceService.CreateOrderInSalesforceAsync(order);

        Log($"✅ Order created in Salesforce!");
        Log($"📋 Salesforce ID: {salesforceId}");
        Log($"🔗 View at: https://demo.salesforce.com/{salesforceId}");
    }

    private async void OnRunFullDemoClicked(object sender, EventArgs e)
    {
        Log("🎬 Starting Complete Demo...");
        Log("=================================");

        // Step 1: RabbitMQ - VERWIJDER await
        await Task.Delay(500);
        OnSendToRabbitMQClicked(sender, e);  // 👈 GEEN await

        // Step 2: Salesforce - VERWIJDER await
        await Task.Delay(500);
        OnSyncToSalesforceClicked(sender, e);  // 👈 GEEN await

        // Step 3: CI/CD Demo - VERWIJDER await
        await Task.Delay(500);
        OnBuildDemoClicked(sender, e);  // 👈 GEEN await
        OnTestDemoClicked(sender, e);   // 👈 GEEN await  
        OnDeployDemoClicked(sender, e); // 👈 GEEN await

        Log("=================================");
        Log("🎉 COMPLETE DEMO FINISHED!");
        Log("✅ App → RabbitMQ → Salesforce ✅");
        Log("✅ CI/CD Pipeline ✅");
    }

    private async void OnBuildDemoClicked(object sender, EventArgs e)
    {
        Log("🏗️ CI Demo: Automated Build");

        await Task.Delay(300);
        Log("   • Trigger: Code push to GitHub");
        Log("   • Action: dotnet restore");
        Log("   • Action: dotnet build");
        Log("   • Result: MAUI APK generated");

        await Task.Delay(300);
        Log("✅ BUILD COMPLETED AUTOMATICALLY!");
    }

    private async void OnTestDemoClicked(object sender, EventArgs e)
    {
        Log("🧪 CI Demo: Automated Tests");

        await Task.Delay(300);
        Log("   • Test 1: RabbitMQ Service → ✅");
        Log("   • Test 2: Salesforce Service → ✅");
        Log("   • Test 3: Integration Flow → ✅");
        Log("   • All tests executed automatically");

        await Task.Delay(300);
        Log("✅ ALL TESTS PASSED!");
        Log("💡 If tests fail → Pipeline stops!");
    }

    private async void OnDeployDemoClicked(object sender, EventArgs e)
    {
        Log("🚀 CD Demo: Automated Deployment");

        await Task.Delay(300);
        Log("   • Condition: Tests passed");
        Log("   • Step 1: Deploy RabbitMQ config");
        Log("   • Step 2: Deploy Salesforce integration");
        Log("   • Step 3: Deploy MAUI app");

        await Task.Delay(300);
        Log("✅ DEPLOYMENT COMPLETED AUTOMATICALLY!");
        Log("📱 App available in stores immediately!");
    }

    private void OnViewQueueClicked(object sender, EventArgs e)
    {
        Log("👁️ Viewing RabbitMQ Queue Status:");
        Log("   • Queue: salesforce_orders");
        Log("   • Messages: 5 waiting");
        Log("   • Consumers: 1 active");
        Log("   • Status: Healthy ✅");
    }

    private void OnClearLogClicked(object sender, EventArgs e)
    {
        LogOutput.Text = "";
        Log("🗑️ Log cleared");
    }

    private async Task AnimateProgress(ProgressBar progressBar, double from, double to)
    {
        var animation = new Animation(v => progressBar.Progress = v, from, to);
        animation.Commit(progressBar, "ProgressAnimation", 16, 1000, Easing.Linear);
        await Task.Delay(1000);
    }
}