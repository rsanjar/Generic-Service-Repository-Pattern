namespace EdriveAuto.Common.Enums;

public enum Crud
{
    Success,
    Error,
    ValidationError,
    DuplicateEntryError,
    ItemNotFoundError,
    DeleteForeignKeyReferenceError,
    AccessDeniedError
}