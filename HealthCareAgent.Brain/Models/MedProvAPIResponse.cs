using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HealthCareAgent.Brain.Models;

public class MedAPIResponse
{
    [JsonPropertyName("results")]
    public List<Result> Results { get; set; } = new List<Result>();

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("schema")]
    public Schema Schema { get; set; }

    [JsonPropertyName("query")]
    public Query Query { get; set; }
}

public class Result
{
    [JsonPropertyName("npi")]
    public string Npi { get; set; }

    [JsonPropertyName("ind_pac_id")]
    public string IndPacId { get; set; }

    [JsonPropertyName("ind_enrl_id")]
    public string IndEnrlId { get; set; }

    [JsonPropertyName("provider_last_name")]
    public string ProviderLastName { get; set; }

    [JsonPropertyName("provider_first_name")]
    public string ProviderFirstName { get; set; }

    [JsonPropertyName("provider_middle_name")]
    public string ProviderMiddleName { get; set; }

    [JsonPropertyName("suff")]
    public string Suff { get; set; }

    [JsonPropertyName("gndr")]
    public string Gndr { get; set; }

    [JsonPropertyName("cred")]
    public string Cred { get; set; }

    [JsonPropertyName("med_sch")]
    public string MedSch { get; set; }

    [JsonPropertyName("grd_yr")]
    public string GrdYr { get; set; }

    [JsonPropertyName("pri_spec")]
    public string PriSpec { get; set; }

    [JsonPropertyName("sec_spec_1")]
    public string SecSpec1 { get; set; }

    [JsonPropertyName("sec_spec_2")]
    public string SecSpec2 { get; set; }

    [JsonPropertyName("sec_spec_3")]
    public string SecSpec3 { get; set; }

    [JsonPropertyName("sec_spec_4")]
    public string SecSpec4 { get; set; }

    [JsonPropertyName("sec_spec_all")]
    public string SecSpecAll { get; set; }

    [JsonPropertyName("telehlth")]
    public string Telehlth { get; set; }

    [JsonPropertyName("facility_name")]
    public string FacilityName { get; set; }

    [JsonPropertyName("org_pac_id")]
    public string OrgPacId { get; set; }

    [JsonPropertyName("num_org_mem")]
    public string NumOrgMem { get; set; }

    [JsonPropertyName("adr_ln_1")]
    public string AdrLn1 { get; set; }

    [JsonPropertyName("adr_ln_2")]
    public string AdrLn2 { get; set; }

    [JsonPropertyName("ln_2_sprs")]
    public string Ln2Sprs { get; set; }

    [JsonPropertyName("citytown")]
    public string Citytown { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("zip_code")]
    public string ZipCode { get; set; }

    [JsonPropertyName("telephone_number")]
    public string TelephoneNumber { get; set; }

    [JsonPropertyName("ind_assgn")]
    public string IndAssgn { get; set; }

    [JsonPropertyName("grp_assgn")]
    public string GrpAssgn { get; set; }

    [JsonPropertyName("adrs_id")]
    public string AdrsId { get; set; }
}

public class Schema
{
    [JsonPropertyName("c55ce199-c853-5916-ae18-0f4631a82106")]
    public SchemaDetails C55ce199C8535916Ae180f4631a82106 { get; set; }
}

public class SchemaDetails
{
    [JsonPropertyName("fields")]
    public Dictionary<string, Field> Fields { get; set; }
}

public class Field
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("mysql_type")]
    public string MysqlType { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Query
{
    [JsonPropertyName("conditions")]
    public List<Condition> Conditions { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("resources")]
    public List<Resource> Resources { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("count")]
    public bool Count { get; set; }

    [JsonPropertyName("results")]
    public bool ResultsQuery { get; set; } // Renamed to avoid collision

    [JsonPropertyName("schema")]
    public bool SchemaQuery { get; set; } //Renamed to avoid collision

    [JsonPropertyName("keys")]
    public bool Keys { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("rowIds")]
    public bool RowIds { get; set; }

    [JsonPropertyName("properties")]
    public List<string> Properties { get; set; }
}

public class Condition
{
    [JsonPropertyName("resource")]
    public string Resource { get; set; }

    [JsonPropertyName("property")]
    public string Property { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("operator")]
    public string Operator { get; set; }
}

public class Resource
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("alias")]
    public string Alias { get; set; }
}
