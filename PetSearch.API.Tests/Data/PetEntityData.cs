using PetSearch.API.Entities;

namespace PetSearch.API.Tests.Data;

public class PetEntityData: TheoryData<Pet>
{
    public PetEntityData()
    {
        Add(GetPetWithAllAttributesInitialized());
        Add(GetPetWithOnlyRequiredAttributesInitialized());
    }
    
    public static Pet GetPetWithAllAttributesInitialized()
    {
        return new Pet()
        {
            Id = PetInterfaceDefaultAttributes.Id,
            Url = PetInterfaceDefaultAttributes.Url,
            Type = PetInterfaceDefaultAttributes.Type,
            Age = PetInterfaceDefaultAttributes.Age,
            Gender = PetInterfaceDefaultAttributes.Gender,
            Size = PetInterfaceDefaultAttributes.Size,
            Name = PetInterfaceDefaultAttributes.Name,
            Description = PetInterfaceDefaultAttributes.Description,
            Photos = PetInterfaceDefaultAttributes.GetPhotoSizesList(),
            PrimaryPhotoCropped = PetInterfaceDefaultAttributes.GetPhotoSizes(),
            Status = PetInterfaceDefaultAttributes.Status,
            Distance = PetInterfaceDefaultAttributes.Distance,
        };
    }

    public static Pet GetPetWithOnlyRequiredAttributesInitialized()
    {
        return new Pet()
        {
            Id = PetInterfaceDefaultAttributes.Id,
            Url = PetInterfaceDefaultAttributes.Url,
            Type = PetInterfaceDefaultAttributes.Type,
            Age = PetInterfaceDefaultAttributes.Age,
            Gender = PetInterfaceDefaultAttributes.Gender,
            Size = PetInterfaceDefaultAttributes.Size,
            Name = PetInterfaceDefaultAttributes.Name,
            Photos = new List<PhotoSizes>(),
            Status = PetInterfaceDefaultAttributes.Status,
        };
    }
}