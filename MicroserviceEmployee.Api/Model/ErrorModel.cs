namespace MicroserviceEmployee.Api.Model;

public class DetailErrorModel
{
  public int Row { get; set; }
  public required string MessageError {get;set;}
}

public class ErrorModel
{
  public int TotalRows { get; set; }
  public int TotalErrors { get; set; }
  public List<DetailErrorModel>? ErrorsList { get; set; }
}