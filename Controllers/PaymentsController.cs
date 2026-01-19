using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElMosa3ed.Api.Data;
using ElMosa3ed.Api.Models;
using Stripe;
using Stripe.Checkout;

namespace ElMosa3ed.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public PaymentsController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("create-checkout-session")]
    public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutRequest req)
    {
        var domain = "http://localhost:5000"; // Change to your real domain in production
        
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = 500, // $5.00
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "ElMosa3ed Pro Subscription",
                            Description = "Unlimited access to AI features"
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment", // Use "subscription" if you set up a recurring price in Stripe Dashboard
            SuccessUrl = domain + "/success.html",
            CancelUrl = domain + "/cancel.html",
            Metadata = new Dictionary<string, string>
            {
                { "deviceId", req.DeviceId }
            }
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        return Ok(new { url = session.Url });
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var webhookSecret = _config["Stripe:WebhookSecret"];

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], webhookSecret);

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
                var deviceId = session.Metadata["deviceId"];

                if (!string.IsNullOrEmpty(deviceId))
                {
                    var user = await _db.Users.FirstOrDefaultAsync(u => u.DeviceId == deviceId);
                    if (user != null)
                    {
                        user.Plan = "Pro";
                        user.ProExpiryDate = DateTime.UtcNow.AddMonths(1);
                        await _db.SaveChangesAsync();
                    }
                }
            }

            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }
    }
}