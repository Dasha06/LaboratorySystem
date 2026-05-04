using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IReferentialGroupRepository
{
    List<ReferentialGroup> GetAllReferentialGroups();
    ReferentialGroup GetReferentialGroupByRefGroupId(int refGroupId);
    bool CreateReferentialGroup(ReferentialGroup referentialGroup);
    bool DeleteReferentialGroup(int refGroupId);
}
