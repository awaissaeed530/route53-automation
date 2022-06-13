using Amazon;
using Amazon.Route53;
using Amazon.Route53Domains;
using Amazon.Route53Domains.Model;

namespace aws_service.Services;

public interface IDomainService
{
    Task<bool> CheckAvailablity(string name);
}

public class DomainService : IDomainService
{
    private readonly AmazonRoute53Client _client;
    private readonly AmazonRoute53DomainsClient _domainsClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DomainService> _logger;

    public DomainService(
        IConfiguration configuration,
        ILogger<DomainService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        _client = new AmazonRoute53Client(
            _configuration.GetValue<string>("Aws:Key"),
            _configuration.GetValue<string>("Aws:KeySecret"),
            RegionEndpoint.USEast2);

        _domainsClient = new AmazonRoute53DomainsClient(
            _configuration.GetValue<string>("Aws:Key"),
            _configuration.GetValue<string>("Aws:KeySecret"),
            RegionEndpoint.USEast2);
    }

    public async Task<bool> CheckAvailablity(string name)
    {
        var response = await _domainsClient.CheckDomainAvailabilityAsync(new CheckDomainAvailabilityRequest
        {
            DomainName = name
        });
        return response.Availability == DomainAvailability.AVAILABLE;
    }
}