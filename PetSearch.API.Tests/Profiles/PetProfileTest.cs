using JetBrains.Annotations;
using PetSearch.API.Entities;
using PetSearch.API.Models;
using PetSearch.API.Profiles;
using PetSearch.API.Tests.Data;


namespace PetSearch.API.Tests.Profiles;

[TestSubject(typeof(PetProfile))]
public class PetProfileTest
{
    [Theory]
    [ClassData(typeof(PetEntityData))]
    public void MapPetToPetDto_ReturnObject_ShouldBePetDto(Pet pet)
    {
        var petProfile = new PetProfile();
        // Action
        var petDto = petProfile.MapPetToPetDto(pet);
        // Assert
        Assert.IsType<PetDto>(petDto);
    }

    [Theory]
    [ClassData(typeof(PetEntityData))]
    public void MapPetToDto_ReturnObject_ShouldMatchTheValuesOfThePassedObject(Pet petEntity)
    {
        // Arrange
        var petProfile = new PetProfile();
        // Action
        var petDto = petProfile.MapPetToPetDto(petEntity);
        // Assert
        AssertObjectsHaveSameValues(petEntity, petDto);
    }

    [Theory]
    [ClassData(typeof(PetEntityData))]
    public void MapPetListToPetDtoList_ReturnObject_ShouldBeAList(Pet petEntity)
    {
        var petProfile = new PetProfile();
        var petList = GetPetList(petEntity);
        // Action
        var petDtoList = petProfile.MapPetListToPetDtoList(petList);
        // Assert
        Assert.IsType<List<PetDto>>(petDtoList);
    }

    [Theory]
    [ClassData(typeof(PetEntityData))]
    public void MapPetListToPetDtoList_ReturnObject_ShouldHaveSameValues(Pet petEntity)
    {
        var petProfile = new PetProfile();
        var petList = GetPetList(petEntity);
        // Action
        var petDtoList = petProfile.MapPetListToPetDtoList(petList);
        // Assert
        Assert.Equal(petList.Count, petDtoList.Count);
        for (int i = 0; i < petList.Count; i++)
        {
            AssertObjectsHaveSameValues(petList[i], petDtoList[i]);
        }
    }

    private void AssertObjectsHaveSameValues(Pet petEntity, PetDto petDto)
    {
        Assert.Equal(petEntity.Id, petDto.Id);
        Assert.Equal(petEntity.Url, petDto.Url);
        Assert.Equal(petEntity.Type, petDto.Type);
        Assert.Equal(petEntity.Age, petDto.Age);
        Assert.Equal(petEntity.Gender, petDto.Gender);
        Assert.Equal(petEntity.Size, petDto.Size);
        Assert.Equal(petEntity.Name, petDto.Name);
        Assert.Equal(petEntity.Description, petDto.Description);
        Assert.Equal(petEntity.Photos, petDto.Photos);
        Assert.Equal(petEntity.PrimaryPhotoCropped, petDto.PrimaryPhotoCropped);
        Assert.Equal(petEntity.Status, petDto.Status);
        Assert.Equal(petEntity.Distance, petDto.Distance);
    }

    private List<Pet> GetPetList(Pet petEntity)
    {
        List<Pet> petList = new List<Pet>();
        for (int i = 0; i < 3; i++)
        {
            petList.Add(petEntity);
        }

        return petList;
    }
}