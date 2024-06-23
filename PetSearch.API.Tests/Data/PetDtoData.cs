using PetSearch.API.Entities;
using PetSearch.API.Models;

namespace PetSearch.API.Tests.Data;

public class PetDtoData : TheoryData<PetDto>
{
    public PetDtoData()
    {
        Add(GetPetWithAllAttributesInitialized());
        Add(GetPetWithOnlyRequiredAttributesInitialized());
    }

    public static PetDto GetPetWithAllAttributesInitialized()
    {
        return new PetDto()
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

    public static PetDto GetPetWithOnlyRequiredAttributesInitialized()
    {
        return new PetDto()
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