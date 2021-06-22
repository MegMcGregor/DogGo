using DogGo.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IOwnerRepository
    {
        List<Owner> GetAllOwners();
        Owner GetOwnerById(int id);
        Owner AddOwner();
        Owner UpdateOwner();
        Owner GetOwnerByEmail();
        Owner DeleteOwner();
    }
}
