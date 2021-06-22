using DogGo.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IOwnerRepository
    {
        List<Owner> GetAllOwners();
        Owner GetOwnerById(int id);
        void AddOwner(Owner owner);
        void UpdateOwner(Owner owner);
        //Owner GetOwnerByEmail();
        void DeleteOwner(int ownerId);
    }
}
