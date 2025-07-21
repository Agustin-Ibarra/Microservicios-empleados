using System.ComponentModel.DataAnnotations;

namespace app.Services;

public class FileService
{
  public static void ValidateData(string[] column)
  {
    for (int i = 0; i < column.Length; i++)
    {
      if (column[i] == null || column[i] == "")
      {
        throw new Exception(message: "Todos los campos deben estar completos y en el formato adecuado");
      }
    }
  }

  public static long ValidatePhoneNumber(string phoneString)
  {
    phoneString = phoneString.Replace("-", "");
    if (phoneString.Length == 10)
    {
      try
      {
        long phoneNumber = long.Parse(phoneString);
        return phoneNumber;
      }
      catch (FormatException)
      {
        throw new Exception(message: $"El numero de telefono {phoneString} no esta un formato adecuado o contiene caracteres erroneos");
      }
    }
    else
    {
      throw new Exception(message: $"El numero de telefono {phoneString} no tiene una longitud adecuada");
    }
  }

  public static void ValidateEmail(string email)
  {
    bool validMail = new EmailAddressAttribute().IsValid(email);
    if (validMail == false)
    {
      throw new Exception(message: "El email no es valido");
    }
  }

  public static int GetJobId(string jobTitle)
  {
    jobTitle = jobTitle.Trim().ToLower();
    if (jobTitle == "frontend developer")
    {
      return 1;
    }
    else if (jobTitle == "backend developer")
    {
      return 2;
    }
    else if (jobTitle == "qa tester")
    {
      return 3;
    }
    else if (jobTitle == "devops engineer")
    {
      return 4;
    }
    else if (jobTitle == "human resources")
    {
      return 5;
    }
    else if (jobTitle == "help desk")
    {
      return 6;
    }
    else
    {
      throw new Exception(message:$"la propiedad {jobTitle} no esta en un formato correcto");
    }
  }
}