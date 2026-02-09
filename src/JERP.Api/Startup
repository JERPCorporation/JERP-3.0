public class Startup
{
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddMvc();
   }
   public void Configure(IApplicationBuilder app, IHostingEnvironment env)
   {
       if (env.IsDevelopment())
       {
           app.UseDeveloperExceptionPage();
       }
       else
       {
           app.UseExceptionHandler("/Error");
           app.UseHsts(); // Enable HTTP Strict Transport Security (HSTS)
       }
       app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
       app.UseStaticFiles();
       app.UseCookiePolicy();
       // Use NWebsec middleware to set security headers
       app.UseHsts(options => options.MaxAge(days: 18 * 7).IncludeSubdomains().Preload());
       app.UseHpkp(options => options
           .Sha256Pins(
               "Base64 encoded SHA-256 hash of your first certificate",
               "Base64 encoded SHA-256 hash of your second backup certificate")
           .MaxAge(days: 18 * 7)
           .IncludeSubdomains());
       app.UseCsp(options => options.UpgradeInsecureRequests());
       app.UseMvc();
   }
}
