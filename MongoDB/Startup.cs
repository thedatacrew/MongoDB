using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace MongoDB
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<DatabaseSettings>(
            //    Configuration.GetSection(nameof(DatabaseSettings)));

            //services.AddSingleton<IDatabaseSettings>(sp =>
            //    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            //services.AddSingleton<MongoService>();

            //services.AddScoped<IProductService, ProductService>();
            services.AddControllers();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIForMongoDBQuickstart_WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIForMongoDBQuickstart_WebAPI v1"));
            }

            app.UseCors(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

          



            //var builder = WebApplication.CreateBuilder(args);

            //// Add services to the container.
            //builder.Services.AddRazorPages();
            //builder.Services.Configure<BookStoreDatabaseSettings>(
            //    builder.Configuration.GetSection("BookStoreDatabase"));
            ////builder.Services.AddSingleton<BooksService>();
            //builder.Services.AddControllers();

            //var app = builder.Build();

            //// Configure the HTTP request pipeline.
            //if (!app.Environment.IsDevelopment())
            //{
            //    app.UseExceptionHandler("/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();

            //app.UseRouting();

            //app.UseAuthorization();

            //app.MapRazorPages();
            //app.MapDefaultControllerRoute();
            //app.UseCors(builder =>
            //{
            //    builder
            //        .AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader();
            //});
            //app.Run();
        }

    }
}
