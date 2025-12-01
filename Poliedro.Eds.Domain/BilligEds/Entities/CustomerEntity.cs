using System;
using System.Collections.Generic;
using System.Text;

namespace Poliedro.Eds.Domain.BilligEds.Entities;

public class CustomerEntity
{
    public string IdentificationNumber { get; set; }
    public string MultipleResolution {  get; set; }
    public string ApiKey { get; set; }
    public string Dv {  get; set; }
    public int Profit { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string MerchantRegistration { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public int TypeDocumentIdentificationId { get; set; }
    public int TypeOrganizationId { get; set; }
    public int TypeLiabilityId { get; set; }
    public int MunicipalityId { get; set; }
    public string MunicipalityCode {  get; set; }
    public int TypeRegimeId {  get; set; }
}
