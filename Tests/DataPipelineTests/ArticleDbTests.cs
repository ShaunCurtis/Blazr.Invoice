using Blazr.App.Core;
using Blazr.App.Infrastructure;
using Blazr.OneWayStreet.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blazr.Diode.Test;

public class ArticleDbTests
{
    //private TestDataProvider _testDataProvider;

    //public ArticleDbTests()
    //    => _testDataProvider = TestDataProvider.Instance();
    //private IServiceProvider GetServiceProvider()
    //{
    //    var services = new ServiceCollection();
    //    //services.AddAppServerInfrastructureServices();
    //    services.AddAppServerMappedInfrastructureServices();
    //    services.AddLogging(builder => builder.AddDebug());

    //    var provider = services.BuildServiceProvider();

    //    // get the DbContext factory and add the test data
    //    var factory = provider.GetService<IDbContextFactory<InMemoryTestDbContext>>();
    //    if (factory is not null)
    //        TestDataProvider.Instance().LoadDbContext<InMemoryTestDbContext>(factory);

    //    return provider!;
    //}

    //[Fact]
    //public async void LoadAnArticle()
    //{
    //    var provider = GetServiceProvider();
    //    var dataBroker = provider.GetService<IDataBroker>()!;

    //    var testItem = _testDataProvider.Articles.First();
    //    var testUid = testItem.ArticleId;

    //    var request = new ItemQueryRequest(testUid);
    //    var loadResult = await dataBroker.ExecuteQueryAsync<ArticleComposite>(request);

    //    var composite = loadResult.Item;

    //    Assert.True(loadResult.Successful);
    //}

    //[Fact]
    //public async Task TestUpdatingRoot()
    //{
    //    var provider = GetServiceProvider();
    //    var dataBroker = provider.GetService<IDataBroker>()!;

    //    var testItem = _testDataProvider.Articles.First();
    //    var testUid = testItem.ArticleId;

    //    var request = new ItemQueryRequest(testUid);
    //    var loadResult = await dataBroker.ExecuteQueryAsync<ArticleComposite>(request);
    //    Assert.True(loadResult.Successful);

    //    var composite = loadResult.Item!;

    //    var articleEditContext = new DmoArticleEditContext(composite.Article);
    //    Assert.NotNull(articleEditContext);

    //    var newTitle = "C# for Total Beginners";
    //    articleEditContext.Title = newTitle;

    //    var updateResult = composite.DispatchArticle(articleEditContext.Mutate);
    //    Assert.True(updateResult.Successful);
    //    Assert.Equal(newTitle, composite.Article.Title);

    //    var command = new CommandRequest<ArticleComposite>(composite, CommandState.None);
    //    var executeResult = await dataBroker.ExecuteCommandAsync(command);
    //    Assert.True(executeResult.Successful);

    //    var testResult = await dataBroker.ExecuteQueryAsync<ArticleComposite>(request);
    //    Assert.True(testResult.Successful);

    //    var testComposite = testResult.Item!;
    //    Assert.Equal(newTitle, testComposite.Article.Title);
    //}

    //[Fact]
    //public async Task TestUpdatingSection()
    //{
    //    var provider = GetServiceProvider();
    //    var dataBroker = provider.GetService<IDataBroker>()!;

    //    var testItem = _testDataProvider.Articles.First();
    //    var testUid = testItem.ArticleId;

    //    var request = new ItemQueryRequest(testUid);
    //    var loadResult = await dataBroker.ExecuteQueryAsync<ArticleComposite>(request);
    //    Assert.True(loadResult.Successful);

    //    var composite = loadResult.Item!;

    //    var _testSectionUid = composite.Sections.First().SectionId;

    //    var section = composite.GetSection(_testSectionUid);
    //    Assert.NotNull(section);

    //    var sectionEditContext = new DmoSectionEditContext(section);
    //    Assert.NotNull(sectionEditContext);

    //    var newContent = "Testing, Testing, Testing, Testing, Testing, Testing";
    //    sectionEditContext.Content = newContent;

    //    var result = composite.DispatchSection(sectionEditContext.SectionUid, sectionEditContext.Mutate);
    //    Assert.True(result.Successful);

    //    var command = new CommandRequest<ArticleComposite>(composite, CommandState.None);
    //    var executeResult = await dataBroker.ExecuteCommandAsync(command);
    //    Assert.True(executeResult.Successful);

    //    var testResult = await dataBroker.ExecuteQueryAsync<ArticleComposite>(request);
    //    Assert.True(testResult.Successful);

    //    var testComposite = testResult.Item!;

    //    var testSection = composite.GetSection(_testSectionUid);
    //    Assert.NotNull(section);

    //    Assert.Equal(newContent, testSection!.Content);
    //}

    //[Fact]
    //public async Task TestAddingSection()
    //{
    //    // Get a Composite
    //    var provider = GetServiceProvider();
    //    var dataBroker = provider.GetService<IDataBroker>()!;

    //    var testItem = _testDataProvider.Articles.First();
    //    var testUid = testItem.ArticleId;

    //    var request = new ItemQueryRequest(testUid);
    //    var loadResult = await dataBroker.ExecuteQueryAsync<ArticleComposite>(request);
    //    Assert.True(loadResult.Successful);

    //    var composite = loadResult.Item!;

    //    // Creates a sectionEditContext with a new Section
    //    var section = new DmoSection() { ArticleId = composite.Article.ArticleId };
    //    var sectionId = section.SectionId;
    //    var sectionEditContext = new DmoSectionEditContext(section);
    //    Assert.NotNull(sectionEditContext);

    //    // Update the section
    //    sectionEditContext.Title = "Test adding new Section";
    //    sectionEditContext.Content = "Adding a further five words";
    //    section = sectionEditContext.MutatedRecord;

    //    // Dispatch the section
    //    var addResult = composite.AddSection(sectionEditContext.MutatedRecord);
    //    Assert.True(addResult);

    //    // Create and execute the command 
    //    var command = new CommandRequest<ArticleComposite>(composite, CommandState.None);
    //    var executeResult = await dataBroker.ExecuteCommandAsync(command);
    //    Assert.True(executeResult.Successful);

    //    // Get the Composite from the database
    //    var testResult = await dataBroker.ExecuteQueryAsync<ArticleComposite>(request);
    //    Assert.True(testResult.Successful);

    //    var testComposite = testResult.Item!;

    //    Assert.Equal(3, testComposite.Sections.Count());
    //    Assert.Equal(section, testComposite.GetSection(sectionId));
    //}

    //[Fact]
    //public async Task TestDeleteASection()
    //{
    //    // Get a Composite
    //    var provider = GetServiceProvider();
    //    var dataBroker = provider.GetService<IDataBroker>()!;

    //    var testItem = _testDataProvider.Articles.First();
    //    var testUid = testItem.ArticleId;

    //    var request = new ItemQueryRequest(testUid);
    //    var loadResult = await dataBroker.ExecuteQueryAsync<ArticleComposite>(request);
    //    Assert.True(loadResult.Successful);

    //    var composite = loadResult.Item!;

    //    var _testSectionUid = composite.Sections.First().SectionId;

    //    var section = composite.GetSection(_testSectionUid);
    //    Assert.NotNull(section);
    //    var sectionId = section.SectionId;

    //    var deleteResult = composite.DeleteSection(sectionId);
    //    Assert.True(deleteResult);

    //    // Create and execute the command 
    //    var command = new CommandRequest<ArticleComposite>(composite, CommandState.Update);
    //    var executeResult = await dataBroker.ExecuteCommandAsync(command);
    //    Assert.True(executeResult.Successful);

    //    // Get the Composite from the database
    //    var testResult = await dataBroker.ExecuteQueryAsync<ArticleComposite>(request);
    //    Assert.True(testResult.Successful);

    //    var testComposite = testResult.Item!;

    //    Assert.Equal(1, testComposite.Sections.Count());
    //}


    //[Fact]
    //public async Task TestDeleteArticle()
    //{
    //    // Get a Composite
    //    var provider = GetServiceProvider();
    //    var dataBroker = provider.GetService<IDataBroker>()!;
    //    var dbFactory = provider.GetService<IDbContextFactory<InMemoryTestDbContext>>()!;
    //    var testItem = _testDataProvider.Articles.First();
    //    var testUid = testItem.ArticleId;

    //    var request = new ItemQueryRequest(testUid);
    //    var loadResult = await dataBroker.ExecuteQueryAsync<ArticleComposite>(request);
    //    Assert.True(loadResult.Successful);

    //    var composite = loadResult.Item!;

    //    var result = composite.DeleteArticle();
    //    Assert.True(result);

    //    // Create and execute the command 
    //    var command = new CommandRequest<ArticleComposite>(composite, CommandState.Delete);
    //    var executeResult = await dataBroker.ExecuteCommandAsync(command);
    //    Assert.True(executeResult.Successful);

    //    // Get the Composite from the database
    //    var testResult = await dataBroker.ExecuteQueryAsync<ArticleComposite>(request);
    //    Assert.False(testResult.Successful);

    //    using var dbContext = dbFactory.CreateDbContext();
    //    var count = await dbContext.Set<DboSection>().Where(item => item.ArticleId == testUid).CountAsync();
    //    Assert.Equal(0, count);
    //}


    //[Fact]
    //public async Task TestAddArticle()
    //{
    //    // Get a Composite
    //    var provider = GetServiceProvider();
    //    var dataBroker = provider.GetService<IDataBroker>()!;
    //    var dbFactory = provider.GetService<IDbContextFactory<InMemoryTestDbContext>>()!;

    //    var testItem = new DmoArticle() {  Title="New Article", Introduction="Welcome to my new article."};
    //    var testUid = testItem.ArticleId;

    //    var composite = new ArticleComposite(testItem, Enumerable.Empty<DmoSection>(), true);

    //    var section = new DmoSection() { ArticleId = composite.Article.ArticleId, SectionId = new(Guid.NewGuid()), Title = "Chapter 1", Content = "Blah, blah" };
    //    composite.AddSection(section);

    //    section = new DmoSection() { ArticleId = composite.Article.ArticleId, SectionId = new(Guid.NewGuid()), Title = "Chapter 2", Content = "Blah, blah" };
    //    composite.AddSection(section);

    //    // Create and execute the command 
    //    var command = new CommandRequest<ArticleComposite>(composite, CommandState.Add);
    //    var executeResult = await dataBroker.ExecuteCommandAsync(command);
    //    Assert.True(executeResult.Successful);

    //    using var dbContext = dbFactory.CreateDbContext();
    //    var list = await dbContext.Set<DboArticle>().ToListAsync();
    //    var list1 = await dbContext.Set<DboSection>().ToListAsync();


    //    // Get the Composite from the database
    //    var request = new ItemQueryRequest(testUid.Value);
    //    var testResult = await dataBroker.ExecuteQueryAsync<ArticleComposite>(request);
    //    Assert.True(testResult.Successful);

    //    var testComposite = testResult.Item!;

    //    Assert.Equal(2, testComposite.Sections.Count());
    //}

}
