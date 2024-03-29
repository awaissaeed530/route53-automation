using aws_service.Models;
using aws_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace aws_service.Controllers;

[ApiController]
[Route("[controller]")]
public class DomainController : ControllerBase
{
    private readonly IInstanceService _instanceService;
    private readonly IOperationCrudService _operationCrudService;
    private readonly IDomainRegistrationService _domainRegistrationService;
    private readonly IDomainAvailabilityService _domainAvailabilityService;

    public DomainController(
        IInstanceService instanceService,
        IOperationCrudService operationCrudService,
        IDomainRegistrationService domainRegistrationService,
        IDomainAvailabilityService domainAvailabilityService)
    {
        _instanceService = instanceService;
        _operationCrudService = operationCrudService;
        _domainRegistrationService = domainRegistrationService;
        _domainAvailabilityService = domainAvailabilityService;
    }

    /// <summary>
    /// Check Availability of given domain name and get list of suggested domain names
    /// </summary>
    /// <param name="name">The domain name</param>
    /// <returns></returns>
    [HttpGet]
    [Route("available/{name}")]
    public async Task<ActionResult<CheckAvailabilityResponse>> CheckAvailablity(string name)
    {
        return Ok(await _domainAvailabilityService.CheckAvailablity(name));
    }

    /// <summary>
    /// Register a domain by given name
    /// </summary>
    /// <param name="name">The name of domain to register</param>
    /// <returns></returns>
    [HttpPost]
    [Route("register/{name}")]
    public async Task<ActionResult<string>> RegisterDomain(string name)
    {
        return Ok(await _domainRegistrationService.RegisterDomain(name));
    }

    [HttpGet]
    [Route("/")]
    public ActionResult<List<Operation>> FindAll()
    {
        return Ok(_operationCrudService.GetAll());
    }

    [HttpPost]
    [Route("ec2/{domainName}/{instanceId}")]
    public async Task<ActionResult> MapInstanceWithDomain(string domainName, string instanceId)
    {
        await _instanceService.MapInstanceWithDomain(domainName, instanceId);
        return Ok();
    }
}