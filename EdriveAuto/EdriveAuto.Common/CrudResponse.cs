using EdriveAuto.Common.Enums;

namespace EdriveAuto.Common;

public class CrudResponse(Crud result)
{
    public Crud MessageKey { get; set; } = result;

    public bool IsSuccess => MessageKey == Crud.Success;

    public string Message
    {
        get
        {
            return MessageKey switch
            {
                Crud.Success => "Success",
                Crud.Error => "Unexpected Error Occurred",
                Crud.ValidationError => "Validation Error",
                Crud.DuplicateEntryError => "Error: Duplicate Record",
                Crud.ItemNotFoundError => "Item Not Found",
                Crud.DeleteForeignKeyReferenceError => "Foreign Key Reference Error",
                Crud.AccessDeniedError => "Access Denied",
                _ => "Error Occurred"
            };
        }
    }
}