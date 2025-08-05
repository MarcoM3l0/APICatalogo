using APICatalogo.context;
using APICatalogo.Controllers;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APICatalagoxUnitTests.UnitTests;
internal class ProdutosUnitTestController
{
    public IUnitOfWork repository;
    public IMapper mapper;
    public ILogger<ProdutosController> logger;
    public static DbContextOptions<AppDbContext> dbContextOptions { get; }

    public static string connectionString = "Server=localhost;Database=CatalogoDB;Uid=root;PWD=123456";

    static ProdutosUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;
    }

    public ProdutosUnitTestController()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ProdutoDTOMappingProfile());
        });

        mapper = config.CreateMapper();
        var context = new AppDbContext(dbContextOptions);
        repository = new UnitOfWork(context);
        logger = new Logger<ProdutosController>(new LoggerFactory());
    }
}
